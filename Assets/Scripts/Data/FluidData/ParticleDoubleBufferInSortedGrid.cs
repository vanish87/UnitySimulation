
namespace Simulation
{
    public class ParticleDoubleBufferInSortedGrid : DoubleBufferInGrid<Particle>
    {
        public override string Identifier => Fluid.DataType.ParticleDouble.ToString();
    }
}