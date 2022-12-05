
using Simulation.Tool;
using Unity.Mathematics;

namespace Simulation.MPM
{
    public struct Cell
    {
        public uint2 index;
        public float mass;
        public float3 mv;
        public float3 velocity;
        public float3 force;
    }
    public class MPMGridBuffer : GPUGridBuffer<Cell>
    {
        public override string Identifier => Fluid.DataType.MPMGrid.ToString();

        protected override IGridConfigure OnGetGridConfigure(object[] parameter)
        {
            return parameter.Find<ISimulationData>().Configures.Find<MPMSimulationSpace>();
        }
    }

}