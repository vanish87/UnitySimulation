
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.MPM
{
    public enum ParticleType
    {
        None = 0,
        Fluid = 1,
        ELastic = 2,
        Snow = 3,
    }
    public struct Particle
    {
        public uint uuid;
        public ParticleType type;
        public float mass;
        public float volume;
        public float3 position;
        public float3 velocity;
        public float3x3 C;
        public float3x3 Fe;
        public float Jp;
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