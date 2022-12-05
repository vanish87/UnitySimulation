
using Unity.Mathematics;

namespace Simulation
{
    public interface IGridConfigure : IConfigure
    {
        ISpace Space { get; }
        float3 Spacing { get; }
    }
}