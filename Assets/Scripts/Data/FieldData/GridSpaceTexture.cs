
using Simulation.Tool;

namespace Simulation.Fluid
{
    public abstract class GridSpaceTexture<T> : GPUTexture<T>
    {
        protected override IGPUBufferConfigure OnGetConfigure(object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var space = data.Spaces.Find<IGridSimulationSpace>();

            var config = base.OnGetConfigure(parameter);
            config?.Init(space);

            return config;
        }
    }
}