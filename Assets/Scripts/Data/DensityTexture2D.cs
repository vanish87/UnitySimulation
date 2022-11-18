using Unity.Mathematics;
namespace Simulation
{
    public class DensityTexture2D : GPUTexture<float2>
    {
        public override string Identifier => Fluid.DataType.DensityTexture2D.ToString();
    }

}