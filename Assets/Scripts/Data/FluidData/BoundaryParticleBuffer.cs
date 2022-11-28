
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Simulation
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundaryParticle
    {
        public int bid;
        public float3 localPos;
        public float3 worldPos;
    }
    public class BoundaryParticleBuffer : GPUBuffer<BoundaryParticle>
    {
        public override string Identifier => Fluid.DataType.BoundaryParticle.ToString();
    }
}