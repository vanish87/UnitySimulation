using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Simulation.Fluid.StableFluid
{
    public class AdvectPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Advect };
        public bool Inited => this.inited;
        protected bool inited = false;
        public void Init(params object[] parameter)
        {
            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var velocity = data.Data.OfType<VelocityTexture2D>().FirstOrDefault();
            var density = data.Data.OfType<DensityTexture2D>().FirstOrDefault();
            Assert.IsNotNull(velocity.Data);
            Assert.IsNotNull(density.Data);

        }
    }
}