
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class SimulationSpace : MonoBehaviour, ISpace
    {
        public float3 Center => this.transform.localPosition;
        public quaternion Rotation => this.transform.localRotation;
        public float3 Scale => this.transform.localScale;
        public float4x4 TRS => this.transform.localToWorldMatrix;
        public bool Inited => true;
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.matrix = this.TRS;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}