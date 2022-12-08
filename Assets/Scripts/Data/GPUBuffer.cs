using System.Linq;
using System.Runtime.InteropServices;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public interface IGPUBufferConfigure : IConfigure
    {
        public int3 Size { get; }
    }
    public abstract class GPUBuffer<T> : MonoBehaviour, IStructuredData<GraphicsBuffer, T>
    {
        public abstract string Identifier { get; }
        // public abstract Access Access { get; }
        public virtual GraphicsBuffer Data => this.data;
        public virtual int3 Size
        {
            get => this.size; 
            set
            {
                if (math.all(this.size == value)) return;
                
                this.size = value;
                this.OnCreateBuffer(this.Length);
            }
        }
        public virtual int Length => this.Size.x * this.Size.y * this.Size.z;
        public virtual bool Inited => this.inited;
        public virtual Dimension Dim => this.Size.z > 1 ? Dimension.Dim3D : this.Size.y > 1 ? Dimension.Dim2D : Dimension.Dim1D;
        [SerializeField] protected int3 size = new int3(1, 1, 1);
        [SerializeField] protected ComputeShader initCS;
        protected bool inited = false;
        protected GraphicsBuffer data;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = this.OnGetConfigure(parameter);
            if (config != null) this.Size = config.Size;
            else this.OnCreateBuffer(this.Length);

            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            this.Data?.Release();
            this.inited = false;
        }

        public virtual void Reset(T defaultValue)
        {
            this.Data.SetData(Enumerable.Repeat(defaultValue, this.Length).ToArray());
        }

        protected virtual void OnCreateBuffer(int size, GraphicsBuffer.Target target = GraphicsBuffer.Target.Structured)
        {
            Debug.Assert(size > 0);
            
            this.Data?.Release();

            this.data = new GraphicsBuffer(target, size, Marshal.SizeOf<T>());
            this.Data.SetCounterValue(0);

            if(this.initCS != null)
            {
                var k = this.initCS.FindKernel("InitBuffer");
                this.initCS.SetBuffer(k, "_Buffer", this.Data);
                this.initCS.SetInt("_BufferCount", this.Data.count);
                DispatchTool.Dispatch(this.initCS, k, this.Size);
            }
        }

        protected virtual IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            return this.GetComponent<IGPUBufferConfigure>();
        }

    }
}