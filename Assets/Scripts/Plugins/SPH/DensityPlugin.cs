
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
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
            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            var density = data.Data.OfType<ParticleDensityBuffer>().FirstOrDefault();
            var grid = data.Data.OfType<SPHGridBuffer>().FirstOrDefault();

            var sphConfigure = data.Configures.OfType<ISPHConfigure>().FirstOrDefault();
            var particleConfigure = data.Configures.OfType<IParticleConfigure>().FirstOrDefault();
            var simSpace = data.Configures.OfType<SimulationSpace>().FirstOrDefault();

            this.SetConstant(sphConfigure, particleConfigure, simSpace);
            this.SetBuffer(particle.Read.Data, density.Data);
            this.OnSetupGridParameter(grid);
        }

        protected void SetConstant(ISPHConfigure config, IParticleConfigure particleConfigure, ISpace simSpace)
        {
            var cs = this.densityCS;
            var k = cs.FindKernel(Kernel);

            cs.SetInt("_NumberOfParticle", particleConfigure.NumOfParticles);
            cs.SetFloat("_H", config.SmoothLength);
            cs.SetVector("_PressureK", new Vector4(config.PressureK.x, config.PressureK.y, 0, 0));
            cs.SetVector("_ParticleGamma", new Vector4(config.ParticleGamma.x, config.ParticleGamma.y, 0, 0));
            cs.SetFloat("_RestDensity", config.RestDensity);
            cs.SetFloat("_ParticleMass", config.ParticleMass);
            cs.SetFloat("_Viscosity", config.Viscosity);
            cs.SetFloat("_VorticityConfinement", config.VorticityConfinement);
            cs.SetVector("_NU_T", new Vector4(config.NU_T.x, config.NU_T.y, config.NU_T.z, 0));
            cs.SetVector("_NU_EXT", new Vector4(config.NU_EXT.x, config.NU_EXT.y, config.NU_EXT.z, 0));
            cs.SetVector("_Theta", new Vector4(config.Theta.x, config.Theta.y, config.Theta.z, 0));
            cs.SetVector("_Gravity", new Vector4(config.Gravity.x, config.Gravity.y, config.Gravity.z, 0));

            var min = simSpace.Center;
            var max = simSpace.Center + simSpace.Scale;
            cs.SetVector("_SpaceMin", new Vector4(min.x, min.y, min.z, 0));
            cs.SetVector("_SpaceMax", new Vector4(max.x, max.y, max.z, 0));

            cs.SetMatrix("_SimSpaceLocalToWorld", simSpace.TRS);
            cs.SetMatrix("_SimSpaceWorldToLocal", math.inverse(simSpace.TRS));

            cs.SetFloat("_MaxSpeed", config.MaxSpeed);
            cs.SetFloat("_TimeStep", config.PreferredTimeStep);

        }
        protected virtual void OnSetupGridParameter(SPHGridBuffer grid)
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
            cs.SetBuffer(k, "_ParticleBuffer", particleRead);
            cs.SetBuffer(k, "_ParticleDensityBuffer", density);
        }
    }
}