using System.Linq;
using Unity.Mathematics;

namespace Simulation.Fluid.SPH
{
    public struct ParticleForce
    {
		public float3 force;
		public float3 transferForce;
		public float3 transferTorque;
    }
    public class ParticleForceBuffer : GPUBuffer<ParticleForce>
    {
        public override string Identifier => DataType.ParticleForce.ToString();
        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            return data.Configures.OfType<ParticleConfigure>().FirstOrDefault();
        }
    }
}