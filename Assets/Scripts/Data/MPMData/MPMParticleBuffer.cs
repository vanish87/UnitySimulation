
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.MPM
{
    public struct Particle
    {
        public uint uuid;
        public float3 pos;
        public float3 vel;
        public ParticleType type;
        public float3 w;
        public float life;
        public float4 col;
        public float3x3 J;
    }
    public class MPMParticleBuffer : GPUBuffer<Particle>
    {
        public override string Identifier => Fluid.DataType.Particle.ToString();

        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            return data.Configures.Find<ParticleConfigure>();
        }
    }
}