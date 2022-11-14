using Unity.Mathematics;

namespace Simulation
{
    public interface IGrid
    {
        float3 Origin { get; }
        int3 Size { get; }
        float3 Spacing { get; }
    }
}