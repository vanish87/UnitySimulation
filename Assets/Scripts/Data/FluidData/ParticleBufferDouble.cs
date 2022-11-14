
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public class ParticleBufferDouble : MonoBehaviour, IData
    {
        public virtual string Identifier => Fluid.DataType.ParticleDouble.ToString();
        public bool Inited => this.inited;
        protected bool inited = false;
        public ParticleBuffer Read { get; protected set; }
        public ParticleBuffer Write { get; protected set; }
        public virtual void Init(params object[] parameter)
        {
            var particles = this.GetComponents<ParticleBuffer>();
            this.Read = particles.First();
            this.Write = particles.Last();
        }
        public virtual void Deinit(params object[] parameter)
        {
        }
        public void SwipeBuffer()
        {
            var temp = this.Read;
            this.Read = this.Write;
            this.Write = temp;
        }
    }
}