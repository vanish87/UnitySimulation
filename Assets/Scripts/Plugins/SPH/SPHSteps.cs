
namespace Simulation.Fluid.SPH
{
    public enum Step
    {
        Density = 0,
        Vorticity,
        VorticityConfinement,
        Viscosity,
        Pressure,
        Integrate
    }
}