using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace Simulation.Fluid
{
    public class SortedParticlePlugin : SortObjectInGrid, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.PrepareData };
        public bool Inited => this.inited;
        public bool Enabled => this.isActiveAndEnabled;
        protected bool inited = false;
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var grid = data.Data.OfType<GPUGridBuffer<uint2>>().FirstOrDefault();
            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();

            this.Sort(particle.Read.Data, grid.Data, grid.Size, grid.Spacing, grid.Min, grid.Max, particle.Write.Data);
            particle.SwipeBuffer();
        }

        public void Init(params object[] parameter)
        {
        }

        public void Deinit(params object[] parameter)
        {
        }

    }
}
