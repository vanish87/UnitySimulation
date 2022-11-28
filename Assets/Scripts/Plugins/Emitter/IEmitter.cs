
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public enum EmitterType
    {
        Disabled = 0,
        SpaceBound,
        Texture
    }
    public interface IEmitter : ISpace
    {
        int UUID { get; }
        EmitterType Type { get; }
        string Identifier { get; }
        int ParticlePerEmit { get; }
        float2 LifeMinMax { get; }
        float4 Parameter { get; set; }
    }

    public interface IEmitterTexture : ITextureField
    {
        float4 ST { get; set; }
    }

    public interface IEmitterController : IInitialize
    {
        IEnumerable<IEmitter> Emitters { get; }
        Texture EmitterTexture { get; }
        ComputeBuffer EmitterBuffer { get; }
        // void OnEmit(ISimulation sim, ISimulationData data);
        void OnSetupBuffer(ComputeShader cs, string kernel);
    }

    public abstract class EmitterControllerBase<T> : MonoBehaviour, IEmitterController
    {
        public abstract bool Inited { get; }
        public virtual IEnumerable<IEmitter> Emitters => this.emitters ??= this.GetComponentsInChildren<IEmitter>();
        public virtual Texture EmitterTexture => this.combinedTexture;
        public virtual ComputeBuffer EmitterBuffer => (this.emitterBuffer ??= this.GetComponentInChildren<GPUBuffer<T>>()).Data;
        public virtual void Init(params object[] parameter)
        {
            this.UpdateCombinedTexture();
        }
        public virtual void Deinit(params object[] parameter)
        {
            if (this.combinedTexture != null) GameObject.Destroy(this.combinedTexture);
        }
        protected IEnumerable<IEmitter> emitters;
        protected virtual IEnumerable<IEmitterTexture> EmitterTextures => this.GetComponentsInChildren<IEmitterTexture>();
        protected T[] EmitterCPU => this.emitterCPU ??= new T[this.Emitters.Count()];
        protected T[] emitterCPU;
        protected GPUBuffer<T> emitterBuffer;
        [SerializeField] protected RenderTexture combinedTexture;
        [SerializeField] protected Material combinedTextureMat;
        public virtual void OnSetupBuffer(ComputeShader cs, string kernel)
        {
            var k = cs.FindKernel(kernel);

            cs.SetTexture(k, "_EmitterTexture", this.EmitterTexture);
            cs.SetVector("_EmitterTextureSize", new Vector4(this.EmitterTexture.width, this.EmitterTexture.height, 0, 0));

            cs.SetBuffer(k, "_EmitterBuffer", this.EmitterBuffer);
            cs.SetInt("_EmitterBufferCount", this.EmitterBuffer.count);

        }
        protected virtual void UpdateCombinedTexture()
        {
            var total = default(int2);
            var st = default(List<float4>);
            var input = this.EmitterTextures.Select(et => et.Texture).ToList();
            TextureTool.CalculateTextureSizeAndOffset(input, out total, out st);

            if (this.combinedTexture != null) GameObject.Destroy(this.combinedTexture);

            var desc = new RenderTextureDescriptor(total.x, total.y);
            this.combinedTexture = new RenderTexture(desc);

            var i = 0;
            foreach (var et in this.EmitterTextures)
            {
                et.ST = st[i++];
            }
        }
        protected virtual void OnCombineEmitterTexture()
        {
            foreach (var et in this.EmitterTextures)
            {
                this.combinedTextureMat.SetVector("_ST", et.ST);
                Graphics.Blit(et.Texture, this.combinedTexture, this.combinedTextureMat, 0);
            }
        }

        protected abstract void OnUpdateEmitterBuffer(ComputeBuffer emitter);

    }

}