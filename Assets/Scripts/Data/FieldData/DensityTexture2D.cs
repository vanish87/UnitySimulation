using Simulation.Tool;

namespace Simulation.Fluid
{
    public class DensityTexture2D : GridSpaceTexture<float>
    {
        public override string Identifier => Fluid.DataType.DensityTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
    }

}