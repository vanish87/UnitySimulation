
using System.Collections.Generic;
using System.Linq;
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
        float4 ST { get; internal set; }
    }

    public interface IEmitterController 
    {
        // void OnEmit(ISimulation sim, ISimulationData data);
        // void OnUpdateEmitterBuffer(ComputeBuffer emitter);
    }

    public abstract class EmitterControllerBase<T> : MonoBehaviour, IEmitterController
    {
        protected IEnumerable<IEmitter> Emitters => this.emitters ??= this.GetComponentsInChildren<IEmitter>();
        protected IEnumerable<IEmitter> emitters;
        protected T[] EmitterCPU => this.emitterCPU ??= new T[this.Emitters.Count()];
        protected T[] emitterCPU;
        protected abstract void OnUpdateEmitterBuffer(ComputeBuffer emitter);
    }

}