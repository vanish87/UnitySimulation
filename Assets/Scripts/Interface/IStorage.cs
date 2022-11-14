
namespace Simulation
{
    public enum Storage
    {
        CPU,
        GPU,
    }
    public interface IStorage
    {
        Storage Storage { get; }
    }
}