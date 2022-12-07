using Simulation.Tool;

namespace Simulation
{
    public class DensityTexture2D : GPUTexture<float>
    {
        public override string Identifier => Fluid.DataType.DensityTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;

        protected override void OnCreateBuffer()
        {
            base.OnCreateBuffer();
            this.Data.enableRandomWrite = true;
        }
        protected override IGPUBufferConfigure OnGetConfigure(params object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var space = data.Spaces.Find<ISimulationSpace>();

            var config = base.OnGetConfigure(parameter);
            config?.Init(space);

            return config;
        }
    }

}