
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulation
{
    public class ParticleRenderPlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.Render };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        public Material mat;
        protected ParticleBufferDouble particleBuffer;
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            if (this.particleBuffer == null) this.particleBuffer = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            var buffer = this.particleBuffer.Read;

            this.mat.SetBuffer("_ParticleBuffer", buffer.Data);
            Graphics.DrawProcedural(this.mat, new Bounds(Vector3.zero, Vector3.one * 10000), MeshTopology.Points, buffer.Data.count);
        }
    }

}