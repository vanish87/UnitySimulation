using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public interface IGPUBufferConfigure : IConfigure
    {
        public int3 Size { get; }
    }
    public abstract class GPUBuffer<T> : MonoBehaviour, IStructuredData<ComputeBuffer, T>
    {
        public abstract string Identifier { get; }
        // public abstract Access Access { get; }
        public ComputeBuffer Data => this.data;
        public int3 Size => this.size;
        public int Length => this.Size.x * this.Size.y * this.Size.z;
        public bool Inited => this.inited;
        [SerializeField] protected int3 size = new int3(1, 1, 1);
        [SerializeField] protected ComputeShader initCS;
        protected bool inited = false;
        protected ComputeBuffer data;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = this.OnGetConfigure(parameter);
            if (config != null) this.size = config.Size;

            Debug.Assert(this.Length > 0);
            this.OnCreateBuffer(this.Length);

            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            this.data?.Release();
            this.inited = false;
        }

        protected virtual void OnCreateBuffer(int size, ComputeBufferType type = ComputeBufferType.Default)
        {
            this.data?.Release();
            this.data = new ComputeBuffer(size, Marshal.SizeOf<T>(), type);
            this.data.SetCounterValue(0);

            Debug.Log(this.name);

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