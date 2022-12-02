
using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class GPUTexture2DConfigure : GPUDataConfigure
    {
        public int BasePixelWidth => this.basePixelWidth;
        [SerializeField] protected int basePixelWidth = 1024;

        public override void Init(params object[] parameter)
        {
            base.Init(parameter);
            
            var space = parameter.OfType<ISpace>().FirstOrDefault();
            if (space != null) this.size = new int3(TextureTool.SpaceToPixelSize(space, this.BasePixelWidth), 1);
        }
    }
}