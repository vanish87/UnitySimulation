
using Simulation.Tool;

namespace Simulation.MPM
{
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