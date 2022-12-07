using System.Collections.Generic;

namespace Simulation
{

    public interface ISimulation : IInitialize
    {
        ISimulationData SimulationData { get; }
        void SimulationStep();

        // void Register(SimulationStep s, SimulationStepFunction step);
        // void Unregister(SimulationStep s, SimulationStepFunction step);
    }

}
