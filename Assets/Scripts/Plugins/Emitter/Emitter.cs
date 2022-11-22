using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class Emitter : MonoBehaviour, IEmitter
    {
        public virtual string Identifier => this.name;
        public virtual float3 Center => this.transform.localPosition;
        public virtual quaternion Rotation => this.transform.localRotation;
        public virtual float3 Scale => this.transform.localScale;
        public virtual float4x4 TRS => this.transform.localToWorldMatrix;
        public virtual float4 Parameter { get => this.parameter; set => this.parameter = value; }
        public virtual int ParticlePerEmit => this.particlePreEmit;
        public virtual bool Inited => this.inited;
        public virtual EmitterType Type => this.isActiveAndEnabled ? EmitterType.SpaceBound : EmitterType.Disabled;
        public virtual float2 LifeMinMax => this.lifeMinMax;
        public virtual int UUID => this.uuid;
        [SerializeField] protected int uuid = -1;
        [SerializeField] protected int particlePreEmit;
        [SerializeField] protected float2 lifeMinMax = 1;
        [SerializeField] protected float4 parameter;
        protected bool inited = false;

        public virtual void Init(params object[] parameter)
        {
            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            this.inited = false;
        }
        protected virtual void OnDrawGizmos()
        {
            if (this.Type == EmitterType.Disabled) return;

            Gizmos.matrix = this.TRS;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}