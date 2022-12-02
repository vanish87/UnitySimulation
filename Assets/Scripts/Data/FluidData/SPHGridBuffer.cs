using Unity.Mathematics;
using System.Linq;
using Simulation.Tool;

namespace Simulation.Fluid.SPH
{
    public class SPHGridBuffer : GPUGridBuffer<uint2>
    {
        public override string Identifier => this.name;
        public override void Init(params object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var sph = data.Configures.Find<ISPHConfigure>();
            var space = data.Spaces.Find<SimulationSpace>();

            this.Setup(space, sph.SmoothLength);
        }
    }

}
