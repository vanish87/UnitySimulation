using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.Fluid.SPH
{
    public class AddSDFForcePlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.AddSDFForce };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader addSDFForceCS;
        protected const string Kernel = "AddSDFForce";

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
            var force = data.Data.OfType<ParticleForceBuffer>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data, force.Data);

            var boundary = data.Plugins.OfType<IBoundaryController>().FirstOrDefault();
            boundary?.OnSetupBuffer(this.addSDFForceCS, Kernel);

            DispatchTool.Dispatch(this.addSDFForceCS, Kernel, particle.Read.Size);
        }

        protected void SetConstant(ISPHConfigure config)
        {
            var cs = this.addSDFForceCS;
            var k = cs.FindKernel(Kernel);
            cs.SetVector("_Gravity", new Vector4(config.Gravity.x, config.Gravity.y, config.Gravity.z, 0));
        }
        protected void SetBuffer(ComputeBuffer particleRead, ComputeBuffer particleForceRW)
        {
            var cs = this.addSDFForceCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRead", particleRead);
            cs.SetBuffer(k, "_ParticleForceBufferRW", particleForceRW);
        }
    }
}
