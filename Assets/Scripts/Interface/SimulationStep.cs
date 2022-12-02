namespace Simulation
{
    public enum SimulationStep
    {
        // Init = 0,//: 0 ~ 1000
        // DataInit = 100,
        // PluginInit = 500,

        Validate = 1000,

        // Simulation: 2000 ~ 10000

        //Emitter/Boundary Update
        BeforeSimulation = 2000,

        //Update sorted grid hash etc.
        PrepareData = 4000,

        //Density/Pressure/Integrate etc.
        OnSimulation = 5000,

        //Field Update etc.
        AfterSimulation = 6000,
        PostProcess = 7000,

        Render = 10000,

        // Deinit = 20000,// 20000 ~ 
        // PluginDeinit = 30000,
        // DataDeinit = 40000,
    }
}