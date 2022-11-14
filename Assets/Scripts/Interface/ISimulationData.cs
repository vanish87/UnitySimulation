
using System.Collections.Generic;

namespace Simulation
{
    public interface ISimulationData
    {
        IEnumerable<IConfigure> Configures { get; }
        IEnumerable<ISpace> Spaces { get; }
        IEnumerable<IData> Data { get; }
        IEnumerable<IPlugin> Plugins { get; }
    }
}