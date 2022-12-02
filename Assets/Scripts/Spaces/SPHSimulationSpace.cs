
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Fluid.SPH
{
    public class SPHSimulationSpace : SPHConfigure, ISimulationSpace, IGridConfigure
    {
        public float3 Center => this.transform.localPosition;
        public quaternion Rotation => this.transform.localRotation;
        public float3 Scale => this.transform.localScale;
        public float4x4 TRS => this.transform.localToWorldMatrix;
        public ISpace Space => this;
        public float3 Spacing => this.SmoothLength;

        protected virtual void OnDrawGizmos()
        {
            Gizmos.matrix = this.TRS;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}