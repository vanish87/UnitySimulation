
namespace Simulation.Fluid.StableFluid
{
    public interface IStableFluidConfigure : IConfigure
    {
        float Timestep { get; }
        float VelocityDissipation { get; }
        int JacobiIterationForDiffusion { get; }
        int JacobiIterationForPressure { get; }
        float Viscosity { get; }
    }
}