using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;
namespace Simulation.Fluid.SPH
{
    public class IntegratePlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Integrate };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader integrateCS;
        protected const string Kernel = "Integrate";
        
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var sphConfigure = data.Configures.OfType<ISPHConfigure>().FirstOrDefault();
            var simSpace = data.Spaces.OfType<SimulationSpace>().FirstOrDefault();
            this.SetConstant(sphConfigure, simSpace);

            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            var force = data.Data.OfType<ParticleForceBuffer>().FirstOrDefault();
            var density = data.Data.OfType<ParticleDensityBuffer>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data, particle.Write.Data, force.Data, density.Data);

            var append = data.Data.OfType<ParticleAppendIndexBuffer>().FirstOrDefault();
            if(append != null) this.SetAppendIndexBuffer(append.Data);

            var grid = data.Data.OfType<SPHGridBuffer>().FirstOrDefault();
            grid.SetupGridParameter(this.integrateCS, Kernel);

            DispatchTool.Dispatch(this.integrateCS, Kernel, particle.Read.Size);

            particle.SwipeBuffer();
        }

        protected void SetConstant(ISPHConfigure config, ISpace simSpace)
        {
            var cs = this.integrateCS;
            var k = cs.FindKernel(Kernel);

			cs.SetFloat("_H", config.SmoothLength);
            cs.SetFloat("_ParticleMass", config.ParticleMass);
            cs.SetFloat("_TimeStep", config.PreferredTimeStep);
            // cs.SetFloat("_TimeStep", Tool.GetCFL(config.SmoothLength, config.MaxSpeed));
            cs.SetFloat("_MaxSpeed", config.MaxSpeed);
            cs.SetVector("_Theta", new Vector4(config.Theta.x, config.Theta.y, config.Theta.z, 0));
            cs.SetVector("_NU_EXT", new Vector4(config.NU_EXT.x, config.NU_EXT.y, config.NU_EXT.z, 0));
            cs.SetVector("_Gravity", new Vector4(config.Gravity.x, config.Gravity.y, config.Gravity.z, 0));

            var min = simSpace.Center - 0.5f * simSpace.Scale;
			var max = simSpace.Center + 0.5f * simSpace.Scale;
			cs.SetVector("_SpaceMin", new Vector4(min.x, min.y, min.z, 0));
			cs.SetVector("_SpaceMax", new Vector4(max.x, max.y, max.z, 0));
        }
        protected void SetBuffer(ComputeBuffer particleRead, ComputeBuffer particleWrite, ComputeBuffer force, ComputeBuffer density)
        {
            var cs = this.integrateCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRead", particleRead);
            cs.SetBuffer(k, "_ParticleBufferWrite", particleWrite);
            cs.SetBuffer(k, "_ParticleForceBufferRW", force);
            cs.SetBuffer(k, "_ParticleDensityBufferRead", density);

        }
        protected void SetAppendIndexBuffer(ComputeBuffer append)
        {
            var cs = this.integrateCS;
            var k = cs.FindKernel(Kernel);

            //IntegrateCS will rebuild current particle pool
            append.SetCounterValue(0);
            cs.SetBuffer(k, "_ParticleAppendIndexBuffer", append);
        }
    }
}