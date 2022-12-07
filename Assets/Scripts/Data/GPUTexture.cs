
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class GPUTexture<T> : MonoBehaviour, IStructuredData<RenderTexture, T>
    {
        public abstract string Identifier { get; }
        // public abstract Access Access { get; }
        public virtual RenderTexture Data => this.data;
        public virtual int3 Size
        {
            get => this.size; 
            set
            {
                if (math.all(this.size == value)) return;

                this.size = value;
                this.OnCreateBuffer();
            }
        }
        public virtual bool Inited => this.inited;
        public virtual int Length => this.Size.x * this.Size.y * this.Size.z;
        public virtual Dimension Dim => this.Size.z > 1 ? Dimension.Dim3D : this.Size.y > 1 ? Dimension.Dim2D : Dimension.Dim1D;
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
            Debug.Assert(this.Length > 0);
            
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
