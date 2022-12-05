using System.Collections.Generic;

namespace Simulation.MPM
{
    public class MPMParticleDoubleBufferInGrid : DoubleBufferInGrid<Particle, Cell>, IPlugin
    {
        public virtual bool Enabled => this.isActiveAndEnabled;
        public override string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int> { (int)SimulationStep.PrepareData };
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            this.OnSortParticle();
        }
    }
}
