
using UnityEngine;

namespace Simulation
{
    public class EmitterNoiseTexture : EmitterTexture
    {
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);
            //Do Noise here
            // this.emitterTex = new RenderTexture(new RenderTextureDescriptor(256, 256, RenderTextureFormat.RGFloat));
        }
        public override void Deinit(params object[] parameter)
        {
            base.Deinit(parameter);
        }

        protected virtual void Update()
        {

        }
    }
}