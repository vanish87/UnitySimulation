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

    public class Identity : ISpace
    {
        public static readonly ISpace Instance = new Identity();
        public float3 Center => 0;
        public quaternion Rotation => quaternion.identity;
        public float3 Scale => 1;
        public float4x4 TRS => float4x4.identity;
        public bool Inited => true;
        public void Deinit(params object[] parameter) { }
        public void Init(params object[] parameter) { }
    }
}