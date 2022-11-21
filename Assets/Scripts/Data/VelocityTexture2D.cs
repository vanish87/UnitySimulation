using Unity.Mathematics;
namespace Simulation
{
    public class VelocityTexture2D : GPUTexture<float2>
    {
        public override string Identifier => Fluid.DataType.VelocityTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
    }

}