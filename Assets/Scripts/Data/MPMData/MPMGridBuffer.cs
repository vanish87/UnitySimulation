
using Unity.Mathematics;

namespace Simulation.MPM
{
    public struct Cell
    {
        public uint2 index;
        public float3 velocity;
    }
    public class MPMGridBuffer : GPUGridBuffer<Cell>
    {
        public override string Identifier => Fluid.DataType.MPMGrid.ToString();
    }

}