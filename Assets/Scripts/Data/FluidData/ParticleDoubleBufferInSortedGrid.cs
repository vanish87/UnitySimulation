
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class ParticleDoubleBufferInSortedGrid : DoubleBufferInGrid<Particle, uint2>, IPlugin
    {
        public virtual bool Enabled => this.isActiveAndEnabled;
        public override string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int> { (int)SimulationStep.PrepareData };
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            this.OnSortParticle();
        }
    }
}