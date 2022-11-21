
namespace Simulation
{
    public class BoundaryParticleDoubleBufferInSortedGrid : DoubleBufferInGrid<BoundaryParticle>
    {
        public override string Identifier => Fluid.DataType.BoundaryParticle.ToString();
    }
}