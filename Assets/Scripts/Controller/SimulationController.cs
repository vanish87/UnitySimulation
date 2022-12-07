
namespace Simulation
{
    public class SimulationController : SimulationControllerBase
    {
        protected virtual void OnEnable()
        {
            this.Init();
        }
        protected virtual void OnDisable()
        {
            this.Deinit();
        }
        protected virtual void Update()
        {
            this.SimulationStep();
        }
    }
}