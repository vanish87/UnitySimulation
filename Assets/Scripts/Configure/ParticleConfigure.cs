
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
        public virtual int3 Size => new int3(this.numOfParticle, 1, 1);
        public virtual bool Inited => this.inited;
        public virtual int NumOfParticles => this.numOfParticle;
        public virtual string Identifier => this.ToString();
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