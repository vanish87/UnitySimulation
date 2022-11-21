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
    public interface ISDFFieldBoundary : IBoundary
    {
        float4 ST { get; set; }
        Texture Field { get; }
    }
    public interface IParticleBoundary : IBoundary
    {
        IEnumerable<float3> Sample();
    }
    public interface IBoundaryController : IInitialize
    {

    }
    public abstract class BoundaryControllerBase<T> : MonoBehaviour, IBoundaryController
    {
        public abstract bool Inited { get; }
        public virtual void Init(params object[] parameter)
        {
            this.UpdateCombinedTexture();
        }
        public virtual void Deinit(params object[] parameter)
        {
            if (this.combinedTexture != null) GameObject.Destroy(this.combinedTexture);
        }
        protected virtual IEnumerable<IBoundary> Boundaries => this.boundaries ??= this.GetComponentsInChildren<IBoundary>();
        protected IEnumerable<IBoundary> boundaries;
        protected virtual IEnumerable<ISDFFieldBoundary> SDFFieldBoundaries => this.GetComponentsInChildren<ISDFFieldBoundary>();
        protected T[] BoundaryCPU => this.boundaryCPU ??= new T[this.Boundaries.Count()];
        protected T[] boundaryCPU;
        protected Texture CombinedTexture => this.combinedTexture;
        [SerializeField] protected RenderTexture combinedTexture;
        [SerializeField] protected Material combinedTextureMat;

        protected virtual void UpdateCombinedTexture()
        {
            var total = default(int2);
            var st = default(List<float4>);
            var input = this.SDFFieldBoundaries.Select(et => et.Field).ToList();
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
                Graphics.Blit(et.Field, this.combinedTexture, this.combinedTextureMat, 0);
            }
        }
        protected abstract void OnUpdateBoundaryBuffer(ComputeBuffer emitter);

    }
}