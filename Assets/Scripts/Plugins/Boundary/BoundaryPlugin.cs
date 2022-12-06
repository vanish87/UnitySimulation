using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class BoundaryPlugin : BoundaryControllerBase<Boundary_S>, IPlugin
    {
        public string Identifier => this.ToString();
        public virtual bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.BeforeSimulation };
        public override bool Inited => this.inited;
        protected bool inited = false;
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);
            this.inited = true;
        }
        public override void Deinit(params object[] parameter)
        {
            base.Deinit(parameter);
            this.inited = false;
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            this.OnUpdateBoundaryBuffer(this.BoundaryBuffer);
            this.OnCombineBoundaryField();

            var space = data.Spaces.Find<ISimulationSpace>();
            if (space != null) this.OnUpdateBoundaryParticleBuffer(space);
        }
        protected override void OnUpdateBoundaryBuffer(GraphicsBuffer boundary)
        {
            var bid = 0;
            foreach (var b in this.Boundaries)
            {
                Debug.Assert(math.determinant(b.TRS) != 0);

                this.BoundaryCPU[bid].uuid = b.UUID;
                this.BoundaryCPU[bid].type = b.Type;
                this.BoundaryCPU[bid].localToWorld = b.TRS;
                this.BoundaryCPU[bid].worldToLocal = math.inverse(b.TRS);
                this.BoundaryCPU[bid].parameter = b.Parameter;
                bid++;
            }

            boundary.SetData(this.BoundaryCPU);
        }
    }
}