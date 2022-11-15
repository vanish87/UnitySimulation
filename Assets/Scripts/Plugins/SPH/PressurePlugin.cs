using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Simulation.Fluid.SPH
{
    public class PressurePlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Pressure };
        public bool Inited => true;
        public bool Enabled => this.enabled;
        [SerializeField] protected ComputeShader pressureCS;
        protected const string Kernel = "Pressure";
        
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
            var force = data.Data.OfType<ParticleForceBuffer>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data, density.Data, force.Data);

            var grid = data.Data.OfType<SPHGridBuffer>().FirstOrDefault();
            this.OnSetupGridParameter(grid);

            DispatchTool.Dispatch(this.pressureCS, Kernel, particle.Read.Size);
        }

        protected void SetConstant(ISPHConfigure config)
        {
            var cs = this.pressureCS;
            var k = cs.FindKernel(Kernel);

			cs.SetFloat("_H", config.SmoothLength);
            cs.SetFloat("_ParticleMass", config.ParticleMass);
            cs.SetFloat("_RestDensity", config.RestDensity);
            cs.SetVector("_PressureK", new Vector4(config.PressureK.x, config.PressureK.y, 0, 0));
        }
        protected virtual void OnSetupGridParameter(SPHGridBuffer grid)
        {
            // this.cs.SetInt("_GridCenterMode", grid.centerMode);
            var cs = this.pressureCS;
            cs.SetVector("_GridSize", new Vector4(grid.Size.x, grid.Size.y, grid.Size.z, 0));
            cs.SetVector("_GridSpacing", new Vector4(grid.Spacing.x, grid.Spacing.y, grid.Spacing.z, 0));
            cs.SetVector("_GridMin", new Vector4(grid.Min.x, grid.Min.y, grid.Min.z, 0));
            cs.SetVector("_GridMax", new Vector4(grid.Max.x, grid.Max.y, grid.Max.z, 0));
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_GridBuffer", grid.Data);
        }

        protected void SetBuffer(ComputeBuffer particleRead, ComputeBuffer densityRead, ComputeBuffer force)
        {
            var cs = this.pressureCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRead", particleRead);
            cs.SetBuffer(k, "_ParticleDensityBufferRead", densityRead);
            cs.SetBuffer(k, "_ParticleForceBuffer", force);
        }
    }
}