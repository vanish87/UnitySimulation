
namespace Simulation
{
    public enum Access
    {
        CPUReadWrite,
        GPURead,
        GPUReadWrite,
    }
    public interface IAccess
    {
        Access Access { get; }
    }
}
