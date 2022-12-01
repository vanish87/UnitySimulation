
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Simulation
{
    public class ParticleAppendIndexBuffer : GPUBuffer<uint>
    {
        public override string Identifier => Fluid.DataType.ParticleIndex.ToString();
        public GraphicsBuffer AppendIndexCounterBuffer => this.appendIndexCounterBuffer;
        protected GraphicsBuffer appendIndexCounterBuffer;
        protected const int CounterBufferSize = 5;
        public override void Deinit(params object[] parameter)
        {
            this.appendIndexCounterBuffer?.Release();
            base.Deinit(parameter);
        }

        protected override void OnCreateBuffer(int size, GraphicsBuffer.Target target = GraphicsBuffer.Target.Append)
        {
            // Debug.Assert(target == GraphicsBuffer.Target.Append);
            base.OnCreateBuffer(size, GraphicsBuffer.Target.Append);
            this.appendIndexCounterBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, CounterBufferSize, Marshal.SizeOf<int>());

            // ComputeBuffer.CopyCount(this.Data, this.AppendIndexCounterBuffer,0);
            // var counter = new int[5];
            // this.AppendIndexCounterBuffer.GetData(counter);
            // Debug.Log(counter[0]);
        }
        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            return data.Configures.OfType<ParticleConfigure>().FirstOrDefault();
        }
    }
}
