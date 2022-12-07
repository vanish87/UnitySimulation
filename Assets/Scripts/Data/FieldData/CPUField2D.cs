
using UnityEngine;
using Unity.Mathematics;
using System.Linq;

namespace Simulation
{
    public interface ICPUDataConfigure : IConfigure
    {
        public int3 Size { get; }
    }
    public class CPUField2D : MonoBehaviour, IStructuredData<float2[,], float2>, IField<float2, float2>
    {
        public virtual string Identifier => this.ToString();
        public virtual float2[,] Data => this.data;
        public virtual int3 Size
        {
            get => this.size;
            set
            {
                this.size = value;
                Debug.Assert(this.Length > 0);
                this.OnCreateBuffer();
            }
        }
        public virtual bool Inited => this.inited;
        public virtual int Length => this.Data.Length;
        public virtual Dimension Dim => Dimension.Dim2D;
        public int3 size = 1;
        protected float2[,] data = new float2[1, 1];
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = this.OnGetConfigure(parameter);
            Debug.Assert(config != null);

            this.Size = config.Size;
            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            this.inited = false;
            this.data = null;
        }
        public virtual float2 Gradient(float2 uv, SampleType sp = SampleType.Center)
        {
            return default;
        }
        public virtual float2 Sample(float2 uv, SampleType sp = SampleType.Center)
        {
            return default;
        }
        protected virtual ICPUDataConfigure OnGetConfigure(object[] parameter)
        {
            return this.GetComponent<ICPUDataConfigure>();
        }
        protected virtual void OnCreateBuffer()
        {
            this.data = new float2[this.Size.x, this.Size.y];
        }
    }
}
