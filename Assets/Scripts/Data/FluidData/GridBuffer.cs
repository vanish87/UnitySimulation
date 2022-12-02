using Unity.Mathematics;
using Simulation.Tool;

namespace Simulation
{
    public class GridBuffer : GPUGridBuffer<uint2>
    {
        public override string Identifier => this.name;
        public override void Init(params object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var grid = data.Configures.Find<IGridConfigure>();
            this.Setup(grid.Space, grid.Spacing);
        }
    }

}
