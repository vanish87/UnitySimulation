
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
        EmitterType Type { get; }
        string Identifier { get; }
        int ParticlePerEmit { get; }
        float4 Parameter { get; set; }
    }

    public interface IEmitterTexture
    {
        Texture Texture { get; }
        float4 ST { get; set; }
    }

    public interface IEmitterController : IInitialize
    {
        // void OnEmit(ISimulation sim, ISimulationData data);
        // void OnUpdateEmitterBuffer(ComputeBuffer emitter);
    }

    public abstract class EmitterControllerBase<T> : MonoBehaviour, IEmitterController
    {
        public abstract bool Inited { get; }
        public abstract void Init(params object[] parameter);
        public abstract void Deinit(params object[] parameter);
        protected IEnumerable<IEmitter> Emitters => this.emitters ??= this.GetComponentsInChildren<IEmitter>();
        protected IEnumerable<IEmitter> emitters;
        protected IEnumerable<IEmitterTexture> EmitterTextures => this.GetComponentsInChildren<IEmitterTexture>();
        // protected IEnumerable<IEmitterTexture> emitterTextures;
        protected T[] EmitterCPU => this.emitterCPU ??= new T[this.Emitters.Count()];
        protected T[] emitterCPU;

        protected Texture CombinedTexture => this.combinedTexture;
        protected RenderTexture combinedTexture;

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
        protected abstract void OnUpdateEmitterBuffer(ComputeBuffer emitter);
    }

}