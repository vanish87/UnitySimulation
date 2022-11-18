using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.Fluid
{
    public class GridDebugPlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader debugCS;
        protected const string Kernel = "Debug";

        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data);
            this.debugCS.SetVector("pos", this.transform.localPosition);

            var grid = data.Data.OfType<SPH.SPHGridBuffer>().FirstOrDefault();
            grid.SetupGridParameter(this.debugCS, Kernel);

            var k = this.debugCS.FindKernel("Reset");
            this.debugCS.SetBuffer(k, "_ParticleBufferRW", particle.Read.Data);
            DispatchTool.Dispatch(this.debugCS, "Reset", particle.Read.Size);

            DispatchTool.Dispatch(this.debugCS, Kernel, 1);
        }

        protected void SetBuffer(ComputeBuffer particle)
        {
            var cs = this.debugCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRW", particle);

        }
    }
}
