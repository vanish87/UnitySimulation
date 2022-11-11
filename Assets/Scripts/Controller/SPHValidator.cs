
using System;
using System.Collections.Generic;

namespace Simulation
{
    public class SPHValidator : IValidator
    {
        public Dictionary<SimulationStep, IEnumerable<Type>> StepAndPlugins => throw new NotImplementedException();

        public void OnSimulationStep(SimulationStep step, ISimulation sim, IFluidData data)
        {
            if(step == SimulationStep.BeforeSimulation)
            {

            }
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}