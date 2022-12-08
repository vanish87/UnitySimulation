using System.Collections.Generic;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Simulation.Fluid.StableFluid
{
    public class AdvectPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.Advect };
        public bool Inited => this.inited;
        [SerializeField] protected Material mat;
        [SerializeField] protected Shader shader;
        protected bool inited = false;
        public void Init(params object[] parameter)
        {
            this.mat = new Material(this.shader);
            this.inited = true;
        }
        public void Deinit(params object[] parameter)
        {
            GameObject.Destroy(this.mat);
            this.inited = false;
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var config = data.Configures.Find<IStableFluidConfigure>();

            var velocity = data.Data.Find<DoubleVelocityTexture2D>();
            var velocityDivergence = data.Data.Find<DoubleVelocityDivergenceTexture2D>();
            var density = data.Data.Find<DoubleDensityTexture2D>();
            var pressure = data.Data.Find<DoublePressureTexture2D>();


            var emitter = data.Plugins.Find<IEmitterController>();
            var boundary = data.Plugins.Find<IBoundaryController>();

            Assert.IsNotNull(velocity.Read.Data);
            Assert.IsNotNull(velocity.Write.Data);

            Assert.IsNotNull(density.Read.Data);
            Assert.IsNotNull(density.Write.Data);

            this.Advect(this.mat, velocity.Read.Data, velocity.Read.Data, velocity.Write.Data, config.VelocityDissipation, new float3(1) / velocity.Read.Size, config.Timestep);
            velocity.SwipeBuffer();

        }

        protected void Advect(Material mat, RenderTexture velocity, RenderTexture src, RenderTexture dest, float dissipation, float3 invSize, float dt)
        {
            mat.SetVector("_InverseSize", new Vector4(invSize.x, invSize.y, invSize.z,0));
            mat.SetFloat("_TimeStep", dt);
            mat.SetFloat("_Dissipation", dissipation);
            mat.SetTexture("_Velocity", velocity);
            mat.SetTexture("_Source", src);
            Graphics.Blit(null, dest, mat, 0);
        }

    }
}