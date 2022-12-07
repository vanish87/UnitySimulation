

using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class EmitterConfigure : MonoBehaviour, IGPUBufferConfigure
    {
        public virtual bool Inited => true;
        public virtual int3 Size => new int3(128, 1, 1);
        public virtual string Identifier => this.ToString();

        public void Deinit(params object[] parameter)
        {
        }

        public void Init(params object[] parameter)
        {
        }
    }
}