
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class CPUDataConfigure : MonoBehaviour, ICPUDataConfigure
    {
        public int3 Size => this.size;
        public bool Inited => true;
        [SerializeField] protected int3 size = 1;
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
    }
}