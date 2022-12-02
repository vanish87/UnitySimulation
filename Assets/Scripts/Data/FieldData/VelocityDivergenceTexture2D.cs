using System.Linq;
using Unity.Mathematics;
namespace Simulation
{
    public class VelocityDivergenceTexture2D : GPUTexture<float2>
    {
        public override string Identifier => Fluid.DataType.VelocityDivergenceTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;
        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            var space = data.Spaces.OfType<SimulationSpace>().FirstOrDefault();

            var config = this.GetComponent<IGPUBufferConfigure>();
            config?.Init(space);

            return config;
        }
    }

}