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
}