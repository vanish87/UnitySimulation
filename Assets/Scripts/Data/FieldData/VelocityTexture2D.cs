using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
namespace Simulation.Fluid
{
    public class VelocityTexture2D : GridSpaceTexture<float2>
    {
        public override string Identifier => Fluid.DataType.VelocityTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
    }

}