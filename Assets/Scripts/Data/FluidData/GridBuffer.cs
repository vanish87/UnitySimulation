using Unity.Mathematics;

namespace Simulation
{
    public class GridBuffer : GPUGridBuffer<uint2>
    {
        public override string Identifier => this.name;
    }

}
