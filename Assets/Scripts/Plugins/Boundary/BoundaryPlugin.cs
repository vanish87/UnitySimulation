using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation
{
    public class BoundaryPlugin : BoundaryControllerBase<Boundary_S>, IPlugin
    {
        public virtual bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.BeforeSimulation };
        public override bool Inited => this.inited;
        protected const string Kernel = "UpdateBoundaryParticle";
        [SerializeField] protected ComputeShader boundaryCS;
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
            var boundary = data.Data.OfType<GPUBuffer<Boundary_S>>().FirstOrDefault();
            var particle = data.Data.OfType<DoubleBuffer<Particle>>().FirstOrDefault();

            this.OnUpdateBoundaryBuffer(boundary.Data);
            this.OnCombineBoundaryField();

            this.OnSetupBuffer(boundary.Data, this.CombinedTexture, particle.Read.Data);
            DispatchTool.Dispatch(this.boundaryCS, Kernel, boundary.Size);
        }
        protected override void OnUpdateBoundaryBuffer(ComputeBuffer boundary)
        {
            var bid = 0;
            foreach (var b in this.Boundaries)
            {
                this.BoundaryCPU[bid].uuid = b.UUID;
                this.BoundaryCPU[bid].type = b.Type;
                this.BoundaryCPU[bid].localToWorld = b.TRS;
                this.BoundaryCPU[bid].parameter = b.Parameter;
                bid++;
            }

            boundary.SetData(this.BoundaryCPU);
        }
        protected virtual void OnSetupBuffer(ComputeBuffer boundary, Texture boundaryTexture, ComputeBuffer particle)
        {
            var cs = this.boundaryCS;
            var k = cs.FindKernel(Kernel);

            cs.SetTexture(k, "_Boundary", boundaryTexture);
            cs.SetVector("_BoundaryTextureSize", new Vector4(boundaryTexture.width, boundaryTexture.height, 0, 0));

            cs.SetBuffer(k, "_BoundaryBuffer", boundary);
            cs.SetInt("_BoundaryBufferCount", boundary.count);
            cs.SetBuffer(k, "_ParticleBufferRW", particle);
        }
    }
}