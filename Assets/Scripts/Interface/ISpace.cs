using Unity.Mathematics;

namespace Simulation
{
    public interface ISpace : IInitialize
    {
        float3 Center { get; }
        quaternion Rotation { get; }
        float3 Scale { get; }
        float4x4 TRS { get; }
    }
}