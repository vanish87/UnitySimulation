using System.Linq;

namespace Simulation.Fluid.SPH
{
    public struct ParticleDensity
    {
        public float density;
    }
    public class ParticleDensityBuffer : GPUBuffer<ParticleDensity>
    {
        public override string Identifier => DataType.ParticleDensity.ToString();
        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            return data.Configures.OfType<ParticleConfigure>().FirstOrDefault();
        }
    }
}