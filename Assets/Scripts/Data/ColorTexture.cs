
using UnityEngine;

namespace Simulation
{
    public class ColorTexture : GPUTexture<Color>
    {
        public override string Identifier => this.ToString();
        protected virtual void OnEnable()
        {
            this.Init();
        }
        protected virtual void OnDisable()
        {
            this.Deinit();
        }
    }

}