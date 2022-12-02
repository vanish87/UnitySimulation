using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.MPM
{
    public struct Particle
    {
        public uint uuid;
        public float3 pos;
        public float3 vel;
        public ParticleType type;
        public float3 w;
        public float life;
        public float4 col;
        public float3x3 J;
    }
    public class MPMParticleDoubleBufferInGrid : DoubleBufferInGrid<Particle, Cell>, IPlugin
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
