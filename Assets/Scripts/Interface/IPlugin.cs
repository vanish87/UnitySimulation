using System.Collections.Generic;

namespace Simulation
{
    public interface IPlugin : IInitialize, ISerializable
    {
        // string Identifier { get; }
        bool Enabled { get; }
        IEnumerable<int> Steps { get; }
        void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data);
    }
}