using System.Collections.Generic;

namespace Simulation
{

    public interface ISimulation : IInitialize
    {
        void SimulationStep();

        // void Register(SimulationStep s, SimulationStepFunction step);
        // void Unregister(SimulationStep s, SimulationStepFunction step);
    }

}
