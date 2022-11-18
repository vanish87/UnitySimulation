using Unity.Mathematics;
namespace Simulation
{
    public class VelocityTexture3D : GPUTexture<float3>
    {
        public override string Identifier => Fluid.DataType.VelocityTexture3D.ToString();
    }

}