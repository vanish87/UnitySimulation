
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class GPUDataConfigure : MonoBehaviour, IGPUDataConfigure
    {
        public bool Inited => this.inited;
        public int3 Size => this.size;
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