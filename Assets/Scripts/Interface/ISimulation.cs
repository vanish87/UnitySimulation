using System.Collections.Generic;

namespace Simulation
{
    public enum SimulationStep
    {
        // Init: 0 ~ 1000
        // DataInit = 100,
        // PluginInit = 500,

        Validate = 1000,

        // Simulation: 2000 ~ 10000
        BeforeSimulation = 2000,
        OnSetupConstants = 3000,
        OnSetupBuffer = 4000,
        OnSimulation = 5000,
        AfterSimulation = 6000,
        PostProcess = 7000,

        Render = 10000,

        // Deinit: 10000 ~ 20000
        // PluginDeinit = 10000,
        // DataDeinit = 20000,
    }


    public interface ISimulation: IInitialize
    {
        // void SimulationStep();

        // void Register(SimulationStep s, SimulationStepFunction step);
        // void Unregister(SimulationStep s, SimulationStepFunction step);
    }

    public interface IFluidData
    {
        IEnumerable<IConfigure> Configures { get; }
        IEnumerable<ISpace> Spaces { get; }
        IEnumerable<IData> Data { get; }
        IEnumerable<IPlugin> Plugins { get; }
    }



}
