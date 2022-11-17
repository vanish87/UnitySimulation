
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public interface IParticleConfigure : IConfigure
    {
        int NumOfParticles { get; }
    }
    public class ParticleConfigure : MonoBehaviour, IGPUBufferConfigure, IParticleConfigure
    {
        public int3 Size => new int3(this.numOfParticle, 1, 1);
        public bool Inited => this.inited;
        public int NumOfParticles => this.numOfParticle;
        [SerializeField] protected int numOfParticle = 1024 * 64;
        protected bool inited = false;
        public virtual void Init(params object[] parameter)
        {
            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }

    }
}