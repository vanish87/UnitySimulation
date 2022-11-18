
namespace Simulation
{
    public class EmitterNoiseTexture : EmitterTexture
    {
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);
            //Do Noise here
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