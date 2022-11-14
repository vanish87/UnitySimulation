using Unity.Mathematics;
using System.Linq;

namespace Simulation.Fluid.SPH
{
    public class SPHGPUGridBuffer : GPUGridBuffer<uint2>
    {
        public override string Identifier => DataType.Grid.ToString();

        public override void Init(params object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            var sph = data.Configures.OfType<SPHConfigure>().FirstOrDefault();
            var space = data.Spaces.OfType<SimulationSpace>().FirstOrDefault();

            this.spacing = sph.D.smoothLength;
            this.size = ScaleSpacingToSize(space.Scale, this.spacing);
            base.Init(parameter);
        }
    }

}
