
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class ParticleRenderPlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.Render };
        public bool Inited => true;
        public Material mat;
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            // if (this.Particle == null) this.Particle = data.Data.OfType<ParticleGPUDataDouble>().FirstOrDefault();
            // var buffer = this.Particle.Read;

            // this.mat.SetBuffer("_ParticleBuffer", buffer.Data);
            // Graphics.DrawProcedural(this.mat, new Bounds(Vector3.zero, Vector3.one * 10000), MeshTopology.Points, buffer.Data.count);
        }
    }

}