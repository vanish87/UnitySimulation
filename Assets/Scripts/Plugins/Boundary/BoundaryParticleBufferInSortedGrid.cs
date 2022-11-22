
namespace Simulation
{
    public class BoundaryParticleBufferInSortedGrid : DoubleBufferInGrid<BoundaryParticle>
    {
        public override string Identifier => Fluid.DataType.BoundaryParticle.ToString();

        public override void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            // Only need to Sort particle when boundary changes
            // base.OnSimulationStep(stepIndex, sim, data);
        }
    }
}