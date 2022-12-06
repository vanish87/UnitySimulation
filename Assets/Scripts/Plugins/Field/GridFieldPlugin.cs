using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class GridFieldPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public bool Enabled => this.isActiveAndEnabled;
        // public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Fluid.SPH.Step.Integrate - 1 };
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.AfterSimulation };
        public bool Inited => this.inited;
        protected const string Kernel = "GridToTexture";
        [SerializeField] protected ComputeShader gridCS;
        protected bool inited = false;
        protected ForceTexture2D Force => this.force ??= this.GetComponent<ForceTexture2D>();
        protected ForceTexture2D force;
        protected VelocityTexture2D Velocity => this.velocity ??= this.GetComponent<VelocityTexture2D>();
        protected VelocityTexture2D velocity;
        protected DensityTexture2D Density => this.density ??= this.GetComponent<DensityTexture2D>();
        protected DensityTexture2D density;
        protected VorticityTexture2D Vorticity => this.vorticity ??= this.GetComponent<VorticityTexture2D>();
        protected VorticityTexture2D vorticity;

        public void Init(params object[] parameter)
        {
            var data = parameter.Find<ISimulationData>();
            var grid = data.Data.Find<IGrid>();

            this.Force.Size = new int3(grid.Size.xy, 1);
            this.Force.Data.enableRandomWrite = true;

            this.Velocity.Size = new int3(grid.Size.xy, 1);
            this.Velocity.Data.enableRandomWrite = true;

            this.Density.Size = new int3(grid.Size.xy, 1);
            this.Density.Data.enableRandomWrite = true;

            this.Vorticity.Size = new int3(grid.Size.xy, 1);
            this.Vorticity.Data.enableRandomWrite = true;

            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }

        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var k = this.gridCS.FindKernel(Kernel);
            var particle = data.Data.OfType<DoubleBuffer<GraphicsBuffer, Particle>>().FirstOrDefault();
            var density = data.Data.OfType<Fluid.SPH.ParticleDensityBuffer>().FirstOrDefault();
            var force = data.Data.OfType<Fluid.SPH.ParticleForceBuffer>().FirstOrDefault();

            this.gridCS.SetBuffer(k, "_ParticleBufferRead", particle.Read.Data);
            this.gridCS.SetBuffer(k, "_ParticleDensityBuffer", density.Data);
            this.gridCS.SetBuffer(k, "_ParticleForceBuffer", force.Data);

            var grid = data.Data.Find<IGrid>();
            grid.OnSetupGridParameter(this.gridCS, Kernel);

            this.gridCS.SetTexture(k, "_Velocity", this.Velocity.Data);
            this.gridCS.SetTexture(k, "_Force", this.Force.Data);
            this.gridCS.SetTexture(k, "_Density", this.Density.Data);
            this.gridCS.SetTexture(k, "_Vorticity", this.Vorticity.Data);
            var size = this.Velocity.Size;
            DispatchTool.Dispatch(this.gridCS, Kernel, size);

            //upsample to larger field texture
            var densityField = data.Data.OfType<DoubleDensityTexture2D>().FirstOrDefault();
            var velocityField = data.Data.OfType<DoubleVelocityTexture2D>().FirstOrDefault();
            var vorticityField = data.Data.OfType<DoubleVorticityTexture2D>().FirstOrDefault();
            var forceField = data.Data.OfType<DoubleForceTexture2D>().FirstOrDefault();
            Graphics.Blit(this.Density.Data, densityField.Read.Data);
            Graphics.Blit(this.Velocity.Data, velocityField.Read.Data);
            Graphics.Blit(this.Vorticity.Data, vorticityField.Read.Data);
            Graphics.Blit(this.Force.Data, forceField.Read.Data);
        }
    }
}