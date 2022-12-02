using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public class BoundaryParticleRender : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.Render };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        public string Identifier => this.ToString();
        public Material mat;
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var particle = data.Data.OfType<DoubleBuffer<GraphicsBuffer, BoundaryParticle>>().FirstOrDefault();

            this.mat.SetBuffer("_ParticleBuffer", particle.Read.Data);
            Graphics.DrawProcedural(this.mat, new Bounds(Vector3.zero, Vector3.one * 10000), MeshTopology.Points, particle.Read.Data.count);
        }
    }
}