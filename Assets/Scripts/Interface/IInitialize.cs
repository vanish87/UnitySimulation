
namespace Simulation
{
    public interface IInitialize
    {
        bool Inited { get; }
        void Init(params object[] parameter);
        void Deinit(params object[] parameter);
    }
}
