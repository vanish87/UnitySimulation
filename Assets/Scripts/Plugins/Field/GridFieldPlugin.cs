using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class GridFieldPlugin : MonoBehaviour, IPlugin, ITextureField
    {
        public string Identifier => this.ToString();
        public bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.AfterSimulation };
        public bool Inited => this.inited;
        protected const string Kernel = "GridToTexture";
        public Texture Texture => this.Velocity.Data;
        public float3 Center => this.transform.localPosition;
        public quaternion Rotation => this.transform.localRotation;
        public float3 Scale => this.transform.localScale;
        public float4x4 TRS => this.transform.localToWorldMatrix;
        [SerializeField] protected ComputeShader gridCS;
        protected bool inited = false;
        protected VelocityTexture2D Velocity => this.velocity ??= this.GetComponent<VelocityTexture2D>();
        protected VelocityTexture2D velocity;

        public void Init(params object[] parameter)
        {
            var data = parameter.OfType<ISimulationData>().FirstOrDefault();
            var grid = data.Data.OfType<GPUGridBuffer<uint2>>().FirstOrDefault();

            if(!grid.Inited) return;

            this.Velocity.Size = new int3(grid.Size.xy, 1);
            this.Velocity.Data.enableRandomWrite = true;

            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            this.inited = false;
        }

        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var k = this.gridCS.FindKernel(Kernel);
            var particle = data.Data.OfType<DoubleBuffer<Particle>>().FirstOrDefault();
            var density = data.Data.OfType<Fluid.SPH.ParticleDensityBuffer>().FirstOrDefault();

            this.gridCS.SetBuffer(k, "_ParticleBufferRead", particle.Read.Data);
            this.gridCS.SetBuffer(k, "_ParticleDensityBuffer", density.Data);

            var grid = data.Data.OfType<GPUGridBuffer<uint2>>().FirstOrDefault();
            grid.SetupGridParameter(this.gridCS, Kernel);

            this.gridCS.SetTexture(k, "_Velocity", this.Velocity.Data);
            DispatchTool.Dispatch(this.gridCS, Kernel, this.Velocity.Size);
        }
    }
}