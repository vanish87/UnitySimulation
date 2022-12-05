using System.Collections;
using System.Collections.Generic;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.MPM
{
    public class InitParticlePlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int>() { };
        public bool Inited => this.inited;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader initParticleCS;
        protected bool inited = false;
        protected const string Kernel = "InitParticleCS";

        public void Init(params object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var particle = data.Data.Find<DoubleBufferInGrid<Particle, Cell>>();

            Debug.Assert(particle != null);
            Debug.Assert(particle.Grid != null);

            var simSpace = data.Spaces.Find<MPMSimulationSpace>();
            var min = simSpace.Center - 0.5f * simSpace.Scale;
			var max = simSpace.Center + 0.5f * simSpace.Scale;
            var cs = this.initParticleCS;
			cs.SetVector("_SpaceMin", new Vector4(min.x, min.y, min.z, 0));
			cs.SetVector("_SpaceMax", new Vector4(max.x, max.y, max.z, 0));

            this.SetupBuffer(particle.Read.Data);

            DispatchTool.Dispatch(this.initParticleCS, Kernel, particle.Read.Size);

            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {

        }

        protected virtual void SetupBuffer(GraphicsBuffer particle)
        {
            var cs = this.initParticleCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRW", particle);
        }
    }
}
