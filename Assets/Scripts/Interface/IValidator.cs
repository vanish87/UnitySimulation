
using System.Collections.Generic;

namespace Simulation
{
    public interface IValidator
    {
        Dictionary<SimulationStep, IEnumerable<System.Type>> StepAndPlugins { get; }
        void Validate();

    }
}