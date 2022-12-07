
namespace Simulation.Fluid.StableFluid
{
    public enum Step
    {
        Advect,
        Diffusion,
        AddForce,
        Divergence,
        PressureSolver,
        ProjectionSubtract,
        Boundary,
    }

}