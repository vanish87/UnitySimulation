using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Fluid.StableFluid
{
    public class StableFluidSimulationSpace : MonoBehaviour, IGridSimulationSpace
    {
        public virtual float3 Center => this.transform.localPosition;
        public virtual quaternion Rotation => this.transform.localRotation;
        public virtual float3 Scale => this.transform.localScale;
        public virtual float4x4 TRS => this.transform.localToWorldMatrix;
        public virtual ISpace Space => this;
        public virtual bool Inited => this.inited;
        protected bool inited = false;
        public void Init(params object[] parameter)
        {
            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }
        protected virtual void OnDrawGizmos()
        {
            Gizmos.matrix = this.TRS;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}