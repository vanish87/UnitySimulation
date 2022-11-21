
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
        public int3 Size
        {
            get => this.size; 
            set
            {
                this.size = value;
                Debug.Assert(this.Length > 0);
                this.OnCreateBuffer();
            }
        }
        public bool Inited => this.inited;
        public int Length => this.Size.x * this.Size.y * this.Size.z;
        [SerializeField] protected int3 size = new int3(1, 1, 1);
        [SerializeField] protected RenderTexture data;
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = this.OnGetConfigure(parameter);
            if (config != null) this.Size = config.Size;
            else this.OnCreateBuffer();

            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            if (this.data != null) GameObject.Destroy(this.data);
            this.inited = false;
        }
        protected virtual IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            return this.GetComponent<IGPUBufferConfigure>();
        }

        protected virtual void OnCreateBuffer()
        {
            if(this.data != null )GameObject.Destroy(this.data);
            this.data = new RenderTexture(this.Size.x, this.Size.y, this.Size.z, this.Format);
        }
        protected virtual RenderTextureFormat Format
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
