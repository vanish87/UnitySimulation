using Simulation.Tool;

namespace Simulation.Fluid
{
    public class PressureTexture2D : GridSpaceTexture<float>
    {
        public override string Identifier => Fluid.DataType.PressureTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
    }

}