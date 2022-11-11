using System.Collections.Generic;

namespace Simulation
{
    public interface IPlugin : IInitialize
    {
        IEnumerable<int> Steps { get; }
        void OnSimulationStep(int stepIndex, ISimulation sim, IFluidData data);
    }
}