using Simulation.Tool;
using Unity.Mathematics;

namespace Simulation
{
    public class ParticleGridBuffer : GPUGridBuffer<uint2>
    {
        public override string Identifier => this.name;

        protected override IGridConfigure OnGetGridConfigure(params object[] parameter)
        {
            return parameter.Find<ISimulationData>().Configures.Find<IGridConfigure>();
        }
    }

}
