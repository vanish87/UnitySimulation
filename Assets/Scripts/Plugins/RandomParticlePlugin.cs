using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public class RandomParticlePlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int> { (int)SimulationStep.Init };
        public bool Inited => true;

        [SerializeField] protected ComputeShader randomCS;

        public void Deinit(params object[] parameter)
        {
        }

        public void Init(params object[] parameter)
        {
        }

        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            var k = this.randomCS.FindKernel("RandomParticle");
            this.randomCS.SetBuffer(k, "_Buffer", particle.Read.Data);
            this.randomCS.SetInt("_BufferCount", particle.Read.Data.count);
            DispatchTool.Dispatch(this.randomCS, k, particle.Read.Size);

            particle.SwipeBuffer();
        }

    }
}