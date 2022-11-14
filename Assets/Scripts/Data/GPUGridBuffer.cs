
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class GPUGridBuffer<Cell> : GPUBuffer<Cell>, IGrid
    {
        public float3 Spacing => this.spacing;
        public float3 Origin => this.transform.localPosition;
        public float3 Min => this.min;
        public float3 Max => this.max;
        [SerializeField] protected float3 spacing = 0.1f;
        [SerializeField] protected float3 min;
        [SerializeField] protected float3 max;

        public override void Init(params object[] parameter)
        {
            this.min = this.Origin;
            this.max = this.min + this.Size * this.Spacing;
            this.OnCreateBuffer(this.Length);
        }

        public static int3 ScaleSpacingToSize(float3 scale, float3 spacing)
        {
			var newSize = scale / spacing;
			var size = new int3(Mathf.CeilToInt(newSize.x), Mathf.CeilToInt(newSize.y), Mathf.CeilToInt(newSize.z));
			return math.max(size, 1);
        }
    }
}
