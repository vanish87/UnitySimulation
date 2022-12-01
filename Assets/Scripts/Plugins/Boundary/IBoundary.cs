using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public enum BoundaryType
    {
        Disabled = 0,
        SDFSphere,
        SDFCube,
        SDFField,
        Particle,
    }
    public interface IBoundary : ISpace
    {
        int UUID { get; }
        BoundaryType Type { get; }
        float4 Parameter { get; set; }
    }
    public interface ISDFFieldBoundary : IBoundary, ITextureField
    {
        float4 ST { get; set; }
    }
    public interface IParticleBoundary : IBoundary
    {
        IEnumerable<float3> Sample();
    }
    public interface IBoundaryController : IInitialize
    {
        IEnumerable<IBoundary> Boundaries { get; }
        IEnumerable<ISDFFieldBoundary> SDFFieldBoundaries { get; }
        IEnumerable<IParticleBoundary> ParticleBoundaries { get; }
        Texture BoundaryTexture { get; }
        GraphicsBuffer BoundaryBuffer { get; }
        DoubleBufferInGrid<BoundaryParticle> BoundaryParticleBuffer { get; }
        void OnSetupBuffer(ComputeShader cs, string kernel);
    }
    public abstract class BoundaryControllerBase<T> : MonoBehaviour, IBoundaryController
    {
        public abstract bool Inited { get; }
        public virtual IEnumerable<IBoundary> Boundaries => this.boundaries ??= this.GetComponentsInChildren<IBoundary>();
        public virtual IEnumerable<ISDFFieldBoundary> SDFFieldBoundaries => this.Boundaries.OfType<ISDFFieldBoundary>();
        public virtual IEnumerable<IParticleBoundary> ParticleBoundaries => this.Boundaries.OfType<IParticleBoundary>();
        public virtual Texture BoundaryTexture => this.combinedTexture;
        public virtual GraphicsBuffer BoundaryBuffer => (this.boundaryBuffer ??= this.GetComponentInChildren<GPUBuffer<T>>()).Data;
        public virtual DoubleBufferInGrid<BoundaryParticle> BoundaryParticleBuffer => this.boundaryParticle ??= this.GetComponentInChildren<BoundaryParticleBufferInSortedGrid>();
        protected IEnumerable<IBoundary> boundaries;
        protected T[] BoundaryCPU => this.boundaryCPU ??= new T[this.Boundaries.Count()];
        protected T[] boundaryCPU;
        protected GPUBuffer<T> boundaryBuffer;
        protected BoundaryParticleBufferInSortedGrid boundaryParticle;
        [SerializeField] protected RenderTexture combinedTexture;
        [SerializeField] protected Material combinedTextureMat;
        [SerializeField] protected ComputeShader updateBoundaryParticleCS;
        protected const string UpdateBoundaryParticleKernel = "UpdateBoundaryParticle";
        public virtual void Init(params object[] parameter)
        {
            this.OnUpdateBoundaryBuffer(this.BoundaryBuffer);
            this.UpdateCombinedTexture();
            this.OnSampleBoundary();
        }
        public virtual void Deinit(params object[] parameter)
        {
            if (this.combinedTexture != null) GameObject.Destroy(this.combinedTexture);
        }
        public virtual void OnSetupBuffer(ComputeShader cs, string kernel)
        {
            var k = cs.FindKernel(kernel);

            cs.SetTexture(k, "_BoundaryTexture", this.BoundaryTexture);
            cs.SetVector("_BoundaryTextureSize", new Vector4(this.BoundaryTexture.width, this.BoundaryTexture.height, 0, 0));

            cs.SetBuffer(k, "_BoundaryBuffer", this.BoundaryBuffer);
            cs.SetInt("_BoundaryBufferCount", this.BoundaryBuffer.count);

            this.BoundaryParticleBuffer.Grid.SetupGridParameter(cs, kernel);
            cs.SetBuffer(k, "_BoundaryParticleBuffer", this.BoundaryParticleBuffer.Read.Data);
        }
        protected virtual void UpdateCombinedTexture()
        {
            var total = default(int2);
            var st = default(List<float4>);
            var input = this.SDFFieldBoundaries.Select(et => et.Texture).ToList();
            TextureTool.CalculateTextureSizeAndOffset(input, out total, out st);

            if (this.combinedTexture != null) GameObject.Destroy(this.combinedTexture);

            var desc = new RenderTextureDescriptor(total.x, total.y);
            this.combinedTexture = new RenderTexture(desc);

            var i = 0;
            foreach (var et in this.SDFFieldBoundaries)
            {
                et.ST = st[i++];
            }
        }
        protected virtual void OnCombineBoundaryField()
        {
            foreach (var et in this.SDFFieldBoundaries)
            {
                this.combinedTextureMat.SetVector("_ST", et.ST);
                Graphics.Blit(et.Texture, this.combinedTexture, this.combinedTextureMat, 0);
            }
        }
        protected virtual void OnSampleBoundary()
        {
            var data = new List<BoundaryParticle>();
            foreach(var b in this.ParticleBoundaries)
            {
                var sample = b.Sample();
                foreach(var p in sample)
                {
                    data.Add(new BoundaryParticle() { bid = b.UUID, localPos = p });
                }
            }

            while (data.Count < this.BoundaryParticleBuffer.Read.Length) data.Add(new BoundaryParticle() { bid = -1 });

            this.BoundaryParticleBuffer.SetData(data.ToArray());

            this.OnUpdateBoundaryParticleBuffer();
        }
        protected virtual void OnUpdateBoundaryParticleBuffer()
        {
            var cs = this.updateBoundaryParticleCS;
            var k = cs.FindKernel(UpdateBoundaryParticleKernel);

            cs.SetBuffer(k, "_BoundaryBuffer", this.BoundaryBuffer);
            cs.SetInt("_BoundaryBufferCount", this.BoundaryBuffer.count);

            cs.SetBuffer(k, "_BoundaryParticleBuffer", this.BoundaryParticleBuffer.Read.Data);
            cs.SetInt("_BoundaryParticleBufferCount", this.BoundaryParticleBuffer.Read.Data.count);
            DispatchTool.Dispatch(cs, k, this.BoundaryParticleBuffer.Read.Size);

            this.BoundaryParticleBuffer.OnSortParticle();
        }
        protected abstract void OnUpdateBoundaryBuffer(GraphicsBuffer boundary);

    }
}