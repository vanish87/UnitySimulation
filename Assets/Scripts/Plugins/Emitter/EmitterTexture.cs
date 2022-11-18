
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class EmitterTexture : Emitter, IEmitterTexture
    {
        public float4 ST { get => this.parameter; set => this.parameter = value; }
        public Texture Texture => this.emitterTex;
        [SerializeField] protected Texture2D emitterTex;
    }
}