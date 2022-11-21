
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public struct Emitter_S
    {
        public int uuid;
        public EmitterType type;
        public int particlePreEmit;
        public float2 lifeMinMax;
        public float4x4 localToWorld;
        public float4 parameter;
    }
    public class EmitterBuffer : GPUBuffer<Emitter_S>
    {
        public override string Identifier => Fluid.DataType.Emitter.ToString();

        // protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        // {
        //     var data = parameter.OfType<ISimulationData>().FirstOrDefault();
        //     return data.Configures.OfType<EmitterConfigure>().FirstOrDefault();
        // }
    }
}