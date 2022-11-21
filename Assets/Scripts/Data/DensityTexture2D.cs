using Unity.Mathematics;
namespace Simulation
{
    public class DensityTexture2D : GPUTexture<float2>
    {
        public override string Identifier => Fluid.DataType.DensityTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
    }

}