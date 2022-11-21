
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Simulation
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class BoundaryParticle
    {
        public uint bid;
        public float3 localPos;
    }
    public class BoundaryParticleBuffer : GPUBuffer<BoundaryParticle>
    {
        public override string Identifier => Fluid.DataType.BoundaryParticle.ToString();
    }
}