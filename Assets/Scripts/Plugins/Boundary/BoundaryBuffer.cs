using Unity.Mathematics;

namespace Simulation
{
    public struct Boundary_S
    {
        public int uuid;
        public BoundaryType type;
        public float4x4 localToWorld;
        public float4 parameter;
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