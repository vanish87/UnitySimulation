using System.Collections.Generic;

namespace Simulation
{
    public interface IPlugin : IInitialize
    {
        bool Enabled { get; }
        IEnumerable<int> Steps { get; }
        void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data);
    }
}