
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class GPUGridBuffer<Cell> : GPUBuffer<Cell>, IGrid
    {
        public static int3 ScaleSpacingToSize(float3 scale, float3 spacing)
        {
			var newSize = scale / spacing;
			var size = new int3(Mathf.CeilToInt(newSize.x), Mathf.CeilToInt(newSize.y), Mathf.CeilToInt(newSize.z));
			return math.max(size, 1);
        }

        public float3 Spacing => this.spacing;
        public float3 Min => this.min;
        public float3 Max => this.max;
        [SerializeField] protected float3 spacing = 0.1f;
        [SerializeField, Attributes.DisableEdit] protected float3 min;
        [SerializeField, Attributes.DisableEdit] protected float3 max;
        [SerializeField] protected bool drawGizmos = false;

        public override void Init(params object[] parameter)
        {
            this.min = 0;
            this.max = this.min + this.Size * this.Spacing;
            this.OnCreateBuffer(this.Length);
        }
        public virtual void Setup(ISpace space, float3 spacing)
        {
            this.spacing = spacing;
            this.size = ScaleSpacingToSize(space.Scale, this.spacing);
            this.min = space.Center - 0.5f * space.Scale;
            this.max = this.min + this.Size * this.Spacing;
            this.OnCreateBuffer(this.Length);
        }
        public virtual void SetupGridParameter(ComputeShader cs, string kernel)
        {
            // cs.SetInt("_GridCenterMode", grid.centerMode);
            cs.SetVector("_GridSize", new Vector4(this.Size.x, this.Size.y, this.Size.z, 0));
            cs.SetVector("_GridSpacing", new Vector4(this.Spacing.x, this.Spacing.y, this.Spacing.z, 0));
            cs.SetVector("_GridMin", new Vector4(this.Min.x, this.Min.y, this.Min.z, 0));
            cs.SetVector("_GridMax", new Vector4(this.Max.x, this.Max.y, this.Max.z, 0));
            var k = cs.FindKernel(kernel);
            cs.SetBuffer(k, "_GridBuffer", this.Data);
        }

        protected virtual void OnDrawGizmos()
        {
			if (Application.isPlaying && this.drawGizmos) GizmosTool.OnDrawGrid(this.Min, this.Max, this.Spacing);
        }
    }
}
