
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class CPUDataConfigure : MonoBehaviour, ICPUDataConfigure
    {
        public virtual int3 Size => this.size;
        public virtual bool Inited => true;
        public virtual string Identifier => this.ToString();

        [SerializeField] protected int3 size = 1;
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
    }
}