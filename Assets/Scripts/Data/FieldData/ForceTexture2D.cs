using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class ForceTexture2D : GPUTexture<float2>
    {
        public override string Identifier => Fluid.DataType.ForceTexture.ToString();
        public override Dimension Dim => Dimension.Dim2D;

        protected override void OnCreateBuffer()
        {
            base.OnCreateBuffer();
            this.Data.enableRandomWrite = true;
        }
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