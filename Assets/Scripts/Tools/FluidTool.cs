
namespace Simulation.Tool
{
    public class FluidTool
    {
        public static float GetCFL(float h, float maxSpeed)
        {
            UnityEngine.Assertions.Assert.IsTrue(maxSpeed > 0);
            
            if (maxSpeed > 0)
            {
                const float lambda = 0.4f;
                return lambda * (h / maxSpeed);
            }
            return 0;
        }

    }
}