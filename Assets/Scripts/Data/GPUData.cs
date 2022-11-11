using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public interface IGPUDataConfigure : IInitialize
    {
        public int3 Size { get; }
    }
    public abstract class GPUData<T> : MonoBehaviour, IStructuredData<ComputeBuffer, T>
    {
        public abstract string Identifier { get; }
        // public abstract Access Access { get; }
        public ComputeBuffer Data => this.data;
        public int3 Size => this.size;
        public int Length => this.Size.x * this.Size.y * this.Size.z;
        public bool Inited => this.inited;
        [SerializeField] protected int3 size = new int3(1, 1, 1);
        protected bool inited = false;
        protected ComputeBuffer data;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = parameter.OfType<IGPUDataConfigure>().First();
            Debug.Assert(config != null);

            this.size = config.Size;
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
        }

    }
}