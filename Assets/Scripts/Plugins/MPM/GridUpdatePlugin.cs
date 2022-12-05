using System.Collections;
using System.Collections.Generic;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.MPM
{
    public class GridUpdatePlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.GridUpdate };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader gridUpdateCS;
        protected const string Kernel = "GridUpdate";

        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var particle = data.Data.Find<DoubleBufferInGrid<Particle, Cell>>();

            Debug.Assert(particle != null);
            Debug.Assert(particle.Grid != null);
        }
    }
}
