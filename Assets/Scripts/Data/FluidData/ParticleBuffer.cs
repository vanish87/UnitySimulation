
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Simulation
{
    public enum ParticleType
    {
        Inactive = 0,
        Fluid = 1,
        Boundary
    }
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Particle
    {
        public uint uuid;
        public float3 pos;
        public float3 vel;
        public ParticleType type;
        public float3 w;
        public float life;
        public float4 col;
    }
    public class ParticleBuffer : GPUBuffer<Particle>
    {
        public override string Identifier => Fluid.DataType.Particle.ToString();
        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            return data.Configures.OfType<ParticleConfigure>().FirstOrDefault();
        }
    }
}