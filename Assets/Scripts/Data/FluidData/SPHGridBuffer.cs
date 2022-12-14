using Unity.Mathematics;
using System.Linq;

namespace Simulation.Fluid.SPH
{
    public class SPHGridBuffer : GPUGridBuffer<uint2>
    {
        public override string Identifier => DataType.Grid.ToString();

        public override void Init(params object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            var sph = data.Configures.OfType<ISPHConfigure>().FirstOrDefault();
            var space = data.Spaces.OfType<SimulationSpace>().FirstOrDefault();

            this.Setup(space, sph.SmoothLength);
        }
    }

}
