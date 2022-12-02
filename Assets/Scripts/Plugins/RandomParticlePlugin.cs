using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation
{
    public class RandomParticlePlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int> { };
        public bool Inited => this.inited;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader randomCS;
        protected bool inited = false;
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }

        public void Init(params object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            var particle = data.Data.OfType<DoubleBuffer<GraphicsBuffer, Particle>>().FirstOrDefault();
            var space = data.Spaces.OfType<ISimulationSpace>().FirstOrDefault();
            var k = this.randomCS.FindKernel("RandomParticle");
            this.randomCS.SetBuffer(k, "_Buffer", particle.Read.Data);
            this.randomCS.SetInt("_BufferCount", particle.Read.Data.count);
            this.randomCS.SetMatrix("_SimSpaceLocalToWorld", space.TRS);
            DispatchTool.Dispatch(this.randomCS, k, particle.Read.Size);

            this.inited = true;
        }

        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
        }

    }
}