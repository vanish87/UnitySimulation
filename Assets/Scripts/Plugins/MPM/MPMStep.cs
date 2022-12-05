
namespace Simulation.MPM
{
    public enum Step
    {
        InitParticle,
        ParticleToGrid = 10,
        GridUpdate,
        GridToParticle,
    }
}