
using Unity.Mathematics;

namespace Simulation.Fluid.SPH
{
    public interface ISPHConfigure : IConfigure
    {
        public float SmoothLength { get; }
		public float2 PressureK { get; }
		public float2 ParticleGamma { get; }
		public float RestDensity { get; }
		public float ParticleMass { get; }
		public float Viscosity { get; }
		public float VorticityConfinement { get; }
		public float3 NU_T { get; }
		public float3 NU_EXT { get; }
		public float3 Theta { get; }
		public float3 Gravity { get; }
		public float PreferredTimeStep { get; }
		public float MaxSpeed { get; }

    }
}