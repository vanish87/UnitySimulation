using Unity.Mathematics;
namespace Simulation
{
    public class VelocityTexture2D : GPUTexture<float2>
    {
        public override string Identifier => Fluid.DataType.VelocityTexture2D.ToString();
    }

}