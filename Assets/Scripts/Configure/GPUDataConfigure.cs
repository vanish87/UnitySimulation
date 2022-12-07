
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class GPUDataConfigure : MonoBehaviour, IGPUBufferConfigure
    {
        public virtual bool Inited => this.inited;
        public virtual int3 Size { get => this.size;}
        public virtual string Identifier => this.ToString();

        [SerializeField] protected int3 size = 1;
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            this.inited = true;
        }
        public virtual void Deinit(params object[] parameter)
        {
            this.inited = false;
        }
    }
}