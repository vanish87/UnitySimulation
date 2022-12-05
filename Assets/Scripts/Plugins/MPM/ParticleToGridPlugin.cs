using System.Collections;
using System.Collections.Generic;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.MPM
{
    public class ParticleToGridPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.ParticleToGrid };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader particleToGridCS;
        protected const string Kernel = "ParticleToGrid";

        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var particle = data.Data.Find<DoubleBufferInGrid<Particle, Cell>>();

            Debug.Assert(particle != null);
            Debug.Assert(particle.Grid != null);

            particle.Grid.OnSetupGridParameter(this.particleToGridCS, Kernel);
            this.SetupBuffer(particle.Read.Data);

            DispatchTool.Dispatch(this.particleToGridCS, Kernel, particle.Grid.Size);
        }

        protected virtual void SetupBuffer(GraphicsBuffer particle)
        {
            var cs = this.particleToGridCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRead", particle);
        }
    }
}
