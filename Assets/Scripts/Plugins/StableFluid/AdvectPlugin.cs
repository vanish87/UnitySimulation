using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
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
            var velocity = data.Data.Find<DoubleVelocityTexture2D>();
            var velocityDivergence = data.Data.Find<DoubleVelocityDivergenceTexture2D>();
            var density = data.Data.Find<DoubleDensityTexture2D>();
            var pressure = data.Data.Find<DoublePressureTexture2D>();


            var emitter = data.Plugins.Find<IEmitterController>();
            var boundary = data.Plugins.Find<IBoundaryController>();

            Assert.IsNotNull(velocity.Read.Data);
            Assert.IsNotNull(velocity.Write.Data);

            Assert.IsNotNull(density.Read.Data);
            Assert.IsNotNull(density.Write.Data);

            velocity.SwipeBuffer();

        }
    }
}