
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Simulation
{
    public class AppendIndexBuffer : GPUBuffer<uint>
    {
        public override string Identifier => this.ToString();
        public ComputeBuffer AppendIndexCounterBuffer => this.appendIndexCounterBuffer;
        protected ComputeBuffer appendIndexCounterBuffer;
        protected const int CounterBufferSize = 5;
        public override void Deinit(params object[] parameter)
        {
            this.appendIndexCounterBuffer?.Release();
            base.Deinit(parameter);
        }

        protected override void OnCreateBuffer(int size, ComputeBufferType type = ComputeBufferType.Default)
        {
            base.OnCreateBuffer(size, ComputeBufferType.Append);
            this.appendIndexCounterBuffer = new ComputeBuffer(CounterBufferSize, Marshal.SizeOf<int>(), ComputeBufferType.IndirectArguments);

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
