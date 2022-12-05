

using Unity.Mathematics;
using UnityEngine;

namespace Simulation.MPM
{
    public class MPMSimulationSpace : MonoBehaviour, ISimulationSpace, IGridConfigure
    {
        public float3 Center => this.transform.localPosition;
        public quaternion Rotation => this.transform.localRotation;
        public float3 Scale => this.transform.localScale;
        public float4x4 TRS => this.transform.localToWorldMatrix;
        public ISpace Space => this;
        public float3 Spacing => this.spacing;
        public bool Inited => this.inited;
        [SerializeField] protected float3 spacing = 1;
        protected bool inited = false;
        protected virtual void OnDrawGizmos()
        {
            Gizmos.matrix = this.TRS;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        public void Init(params object[] parameter)
        {
            this.inited = true;
        }

        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }

    }

}