
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class BoundaryParticleBufferInSortedGrid : DoubleBufferInGrid<BoundaryParticle, uint2>
    {
        public override string Identifier => Fluid.DataType.BoundaryParticle.ToString();

        public override void SetData(BoundaryParticle[] data)
        {
            Debug.Assert(data.Length == this.Read.Data.count);
            Debug.Assert(data.Length == this.Write.Data.count);
            this.Read.Data.SetData(data);
            this.Write.Data.SetData(data);
        }

        // public override void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        // {
        //     // Only need to Sort particle when boundary changes
        //     // base.OnSimulationStep(stepIndex, sim, data);
        // }
    }
}