
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public abstract class DoubleBuffer<S, E> : MonoBehaviour, IData
    {
        public abstract string Identifier { get; }
        public abstract void SetData(E[] data);
        public virtual bool Inited => this.inited;
        protected bool inited = false;
        public virtual IStructuredData<S, E> Read { get; protected set; }
        public virtual IStructuredData<S, E> Write { get; protected set; }
        public virtual void Init(params object[] parameter)
        {
            var buffers = this.GetComponentsInChildren<IStructuredData<S, E>>();
            Debug.Assert(buffers.Count() == 2);

            this.Read = buffers.First();
            this.Write = buffers.Last();
        }
        public virtual void Deinit(params object[] parameter)
        {

        }
        public virtual void SwipeBuffer()
        {
            var temp = this.Read;
            this.Read = this.Write;
            this.Write = temp;
        }
        public virtual void ResetAll(E value = default)
        {
            this.Read.Reset(value);
            this.Write.Reset(value);
        }
    }
}