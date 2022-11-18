using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class Emitter : MonoBehaviour, IEmitter
    {
        public string Identifier => this.name;
        public float3 Center => this.transform.localPosition;
        public quaternion Rotation => this.transform.localRotation;
        public float3 Scale => this.transform.localScale;
        public float4x4 TRS => this.transform.localToWorldMatrix;
        public float4 Parameter { get => this.parameter; set => this.parameter = value; }
        public virtual int ParticlePerEmit => this.particlePreEmit;
        public bool Inited => true;
        public virtual EmitterType Type => this.isActiveAndEnabled ? EmitterType.SpaceBound : EmitterType.Disabled;
        public virtual float2 LifeMinMax => this.lifeMinMax;
        [SerializeField] protected int particlePreEmit;
        [SerializeField] protected float2 lifeMinMax = 1;
        [SerializeField] protected float4 parameter;
        public virtual void Init(params object[] parameter)
        {
        }
        public virtual void Deinit(params object[] parameter)
        {
        }
        protected virtual void OnDrawGizmos()
        {
            Gizmos.matrix = this.TRS;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}