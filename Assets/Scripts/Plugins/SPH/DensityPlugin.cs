
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using UnityEngine;

namespace Simulation.Fluid.SPH
{
    public class DensityPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Density };
        public bool Inited => true;
        public bool Enabled => this.isActiveAndEnabled;
        [SerializeField] protected ComputeShader densityCS;
        protected const string Kernel = "Density";
        
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var sphConfigure = data.Configures.OfType<ISPHConfigure>().FirstOrDefault();
            this.SetConstant(sphConfigure);

            var particle = data.Data.OfType<DoubleBufferInGrid<Particle>>().FirstOrDefault();
            var density = data.Data.OfType<ParticleDensityBuffer>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data, density.Data);

            var grid = particle.Grid;
            grid.SetupGridParameter(this.densityCS, Kernel);

            DispatchTool.Dispatch(this.densityCS, Kernel, particle.Read.Size);
        }

        protected void SetConstant(ISPHConfigure config)
        {
            var cs = this.densityCS;
            var k = cs.FindKernel(Kernel);

            cs.SetVector("_ParticleGamma", new Vector4(config.ParticleGamma.x, config.ParticleGamma.y, 0, 0));
            cs.SetFloat("_H", config.SmoothLength);
            cs.SetFloat("_RestDensity", config.RestDensity);
            cs.SetFloat("_ParticleMass", config.ParticleMass);
        }

        protected void SetBuffer(ComputeBuffer particleRead, ComputeBuffer density)
        {
            var cs = this.densityCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRead", particleRead);
            cs.SetBuffer(k, "_ParticleDensityBuffer", density);
        }
    }
}