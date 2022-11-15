
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulation.Fluid.SPH
{
    public class DensityPlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Density };
        public bool Inited => true;
        public bool Enabled => this.enabled;
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

            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            var density = data.Data.OfType<ParticleDensityBuffer>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data, density.Data);

            var grid = data.Data.OfType<SPHGridBuffer>().FirstOrDefault();
            this.OnSetupGridParameter(grid);

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
        protected void OnSetupGridParameter(SPHGridBuffer grid)
        {
            // this.cs.SetInt("_GridCenterMode", grid.centerMode);
            var cs = this.densityCS;
            cs.SetVector("_GridSize", new Vector4(grid.Size.x, grid.Size.y, grid.Size.z, 0));
            cs.SetVector("_GridSpacing", new Vector4(grid.Spacing.x, grid.Spacing.y, grid.Spacing.z, 0));
            cs.SetVector("_GridMin", new Vector4(grid.Min.x, grid.Min.y, grid.Min.z, 0));
            cs.SetVector("_GridMax", new Vector4(grid.Max.x, grid.Max.y, grid.Max.z, 0));
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_GridBuffer", grid.Data);
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