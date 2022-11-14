
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
        public string Identifier => this.ToString();
        public float2[,] Data => this.data;
        public int3 Size => new int3(this.Data.GetLength(0), this.Data.GetLength(1), 1);
        public bool Inited => this.inited;
        public int Length => this.Data.Length;
        protected float2[,] data = new float2[1, 1];
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            if (this.Inited) return;

            var config = this.GetComponent<ICPUDataConfigure>();
            if (config == null)
            {
                config = parameter.OfType<ICPUDataConfigure>().FirstOrDefault();
            }
            Debug.Assert(config != null);

            this.OnCreateBuffer();

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
        protected virtual void OnCreateBuffer()
        {
            this.data = new float2[this.Size.x, this.Size.y];
        }
    }
}
