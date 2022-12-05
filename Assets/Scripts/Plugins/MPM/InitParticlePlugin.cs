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
        [SerializeField] protected ComputeShader particleToGridCS;
        [SerializeField] protected ComputeShader calculateDensityCS;
        protected bool inited = false;
        protected const string InitKernel = "InitParticle";
        protected const string P2GKernel = "ParticleToGrid";
        protected const string CalculateDensityKernel = "CalculateDensity";

        public void Init(params object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var particle = data.Data.Find<DoubleBufferInGrid<Particle, Cell>>();
            var simSpace = data.Spaces.Find<MPMSimulationSpace>();

            Debug.Assert(particle != null);
            Debug.Assert(particle.Grid != null);

            this.InitParticle(simSpace, particle);

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
        }

        protected virtual void InitParticle(ISpace simSpace, DoubleBufferInGrid<Particle, Cell> particle)
        {
            var min = simSpace.Center - 0.5f * simSpace.Scale;
			var max = simSpace.Center + 0.5f * simSpace.Scale;
            var cs = this.initParticleCS;
			cs.SetVector("_SpaceMin", new Vector4(min.x, min.y, min.z, 0));
			cs.SetVector("_SpaceMax", new Vector4(max.x, max.y, max.z, 0));

            var k = cs.FindKernel(InitKernel);
            cs.SetBuffer(k, "_ParticleBufferRW", particle.Read.Data);

            DispatchTool.Dispatch(this.initParticleCS, InitKernel, particle.Read.Size);

        }
        protected virtual void InitGrid()
        {

        }
    }
}
