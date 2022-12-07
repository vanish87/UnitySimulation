using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;

namespace Simulation.Fluid
{
    public class ForceTexture2D : GridSpaceTexture<float2>
    {
        public override string Identifier => Fluid.DataType.ForceTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
    }

}