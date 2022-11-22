using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class Boundary : MonoBehaviour, IBoundary
    {
        public virtual int UUID => this.uuid;
        public virtual string Identifier => this.name;
        public virtual BoundaryType Type => this.isActiveAndEnabled ? this.type : BoundaryType.Disabled;
        public virtual float4 Parameter { get => this.parameter; set => this.parameter = value; }
        public virtual float3 Center => this.transform.localPosition;
        public virtual quaternion Rotation => this.transform.localRotation;
        public virtual float3 Scale => this.transform.localScale;
        public virtual float4x4 TRS => this.transform.localToWorldMatrix;
        public virtual bool Inited => this.inited;
        [SerializeField] protected int uuid = -1;
        [SerializeField] protected float4 parameter;
        [SerializeField] protected BoundaryType type = BoundaryType.Disabled;
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
            if(this.Type == BoundaryType.Disabled) return;

            Gizmos.matrix = this.TRS;
            if (this.Type == BoundaryType.SDFSphere)
            {
                Gizmos.DrawWireSphere(Vector3.zero, 1);
            }
            else
            {
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }

    }

}