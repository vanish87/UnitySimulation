
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public abstract class DoubleBuffer<T> : MonoBehaviour, IData
    {
        public abstract string Identifier { get; }
        public virtual bool Inited => this.inited;
        protected bool inited = false;
        public virtual GPUBuffer<T> Read { get; protected set; }
        public virtual GPUBuffer<T> Write { get; protected set; }

        public virtual void Init(params object[] parameter)
        {
            var buffers = this.GetComponentsInChildren<GPUBuffer<T>>();
            this.Read = buffers.First();
            this.Write = buffers.Last();
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