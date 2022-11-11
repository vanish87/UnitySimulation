
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class GPUTexture<T> : MonoBehaviour, IStructuredData<RenderTexture, T>
    {
        public abstract string Identifier { get; }
        // public abstract Access Access { get; }
        public virtual RenderTexture Data => this.data;
        public int3 Size => this.size;
        public bool Inited => this.inited;
        public int Length => this.Size.x * this.Size.y * this.Size.z;
        [SerializeField] protected int3 size = new int3(1, 1, 1);
        [SerializeField] protected RenderTexture data;
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = parameter.OfType<IGPUDataConfigure>().First();
            this.size = config.Size;
            var format = this.Format;
            this.data = new RenderTexture(this.size.x, this.size.y, this.size.z, format);

            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            if (this.data is RenderTexture) GameObject.Destroy(this.data);
            this.inited = false;
        }
        protected RenderTextureFormat Format
        {
            get
            {
                switch (default(T))
                {
                    case float v: return RenderTextureFormat.RFloat;
                    case float2 v: return RenderTextureFormat.RGFloat;
                    case float3 v: return RenderTextureFormat.ARGBFloat;
                    case float4 v: return RenderTextureFormat.ARGBFloat;
                    case Color v: return RenderTextureFormat.ARGB32;
                }
                Debug.Assert(false, "Not supported");
                return RenderTextureFormat.Default;
            }
        }
    }
}
