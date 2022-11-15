using Unity.Mathematics;

namespace Simulation
{
    public interface IGrid
    {
        float3 Min { get; }
        float3 Max { get; }
        int3 Size { get; }
        float3 Spacing { get; }
    }
}