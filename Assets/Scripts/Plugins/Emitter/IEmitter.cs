
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
        GraphicsBuffer EmitterBuffer { get; }
        // void OnEmit(ISimulation sim, ISimulationData data);
        void OnSetupBuffer(ComputeShader cs, string kernel);
    }
}