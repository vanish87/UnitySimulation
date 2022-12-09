using Unity.Mathematics;

namespace Simulation
{
    [System.Serializable]
    public struct Boundary_S
    {
        public int uuid;
        public BoundaryType type;
        public float4x4 localToWorld;
        public float4x4 worldToLocal;
        public float4 parameter;
        public float3 velocity;
    }
    public class BoundaryBuffer : GPUBuffer<Boundary_S>
    {
        public override string Identifier => Fluid.DataType.Boundary.ToString();

        // protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        // {
        //     var data = parameter.OfType<ISimulationData>().FirstOrDefault();
        //     return data.Configures.OfType<EmitterConfigure>().FirstOrDefault();
        // }
    }
}