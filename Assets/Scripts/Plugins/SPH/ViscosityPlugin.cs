using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;
namespace Simulation.Fluid.SPH
{
    public class ViscosityPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Viscosity };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader viscosityCS;
        protected const string Kernel = "Viscosity";
        
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var sphConfigure = data.Configures.OfType<ISPHConfigure>().FirstOrDefault();
            this.SetConstant(sphConfigure);

            var particle = data.Data.OfType<DoubleBuffer<Particle>>().FirstOrDefault();
            var density = data.Data.OfType<ParticleDensityBuffer>().FirstOrDefault();
            var force = data.Data.OfType<ParticleForceBuffer>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data, density.Data, force.Data);

            var grid = data.Data.OfType<SPHGridBuffer>().FirstOrDefault();
            grid.SetupGridParameter(this.viscosityCS, Kernel);

            DispatchTool.Dispatch(this.viscosityCS, Kernel, particle.Read.Size);
        }

        protected void SetConstant(ISPHConfigure config)
        {
            var cs = this.viscosityCS;
            var k = cs.FindKernel(Kernel);

			cs.SetFloat("_H", config.SmoothLength);
            cs.SetFloat("_ParticleMass", config.ParticleMass);
            cs.SetFloat("_Viscosity", config.Viscosity);
        }
        protected void SetBuffer(GraphicsBuffer particleRead, GraphicsBuffer densityRead, GraphicsBuffer force)
        {
            var cs = this.viscosityCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRead", particleRead);
            cs.SetBuffer(k, "_ParticleDensityBufferRead", densityRead);
            cs.SetBuffer(k, "_ParticleForceBuffer", force);
        }
    }
}