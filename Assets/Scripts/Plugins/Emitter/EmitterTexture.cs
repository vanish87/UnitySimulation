
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class EmitterTexture : Emitter, IEmitterTexture
    {
        public override EmitterType Type => this.isActiveAndEnabled ? EmitterType.Texture : EmitterType.Disabled;
        public virtual float4 ST { get => this.parameter; set => this.parameter = value; }
        public virtual Texture Texture => this.emitterTex;
        [SerializeField] protected Texture emitterTex;
    }
}