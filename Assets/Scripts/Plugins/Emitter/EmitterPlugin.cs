
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation
{
    public class EmitterPlugin : EmitterControllerBase<Emitter_S>, IPlugin
    {
        public bool Enabled => this.enabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.BeforeSimulation };
        public override bool Inited => this.inited;
        protected const string Kernel = "Emit";
        [SerializeField] protected ComputeShader emitterCS;
        protected bool inited = false;
        public override void Init(params object[] parameter)
        {
            this.inited = true;
        }
        public override void Deinit(params object[] parameter)
        {
            this.inited = false;
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var emitter = data.Data.OfType<GPUBuffer<Emitter_S>>().FirstOrDefault();
            var consume = data.Data.OfType<ParticleAppendIndexBuffer>().FirstOrDefault();
            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();

            this.OnUpdateEmitterBuffer(emitter.Data);
            this.OnSetupBuffer(emitter.Data, consume.Data, particle.Read.Data);
            DispatchTool.DispatchNoGroup(this.emitterCS, Kernel, emitter.Size);
        }
        protected override void OnUpdateEmitterBuffer(ComputeBuffer emitter)
        {
            var eid = 0;
            foreach (var e in this.Emitters)
            {
                this.EmitterCPU[eid].type = e.Type;
                this.EmitterCPU[eid].localToWorld = e.TRS;
                this.EmitterCPU[eid].particlePreEmit = e.ParticlePerEmit;
                this.EmitterCPU[eid].parameter = e.Parameter;
                eid++;
            }

            emitter.SetData(this.EmitterCPU);
        }
        protected virtual void OnSetupBuffer(ComputeBuffer emitter, ComputeBuffer consumeIndex, ComputeBuffer particle)
        {
            var cs = this.emitterCS;
            var k = cs.FindKernel(Kernel);

            cs.SetBuffer(k, "_EmitterBuffer", emitter);
            cs.SetBuffer(k, "_ParticleBufferRW", particle);
            cs.SetBuffer(k, "_ParticleConsumeIndexBuffer", consumeIndex);
        }
    }
}