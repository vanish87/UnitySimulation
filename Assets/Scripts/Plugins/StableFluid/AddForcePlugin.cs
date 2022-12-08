using System.Collections;
using System.Collections.Generic;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Simulation.Fluid.StableFluid
{
    public class AddForcePlugin : MonoBehaviour
    {
        public string Identifier => this.ToString();
        public bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.AddForce };
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
            return;
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

            for (var i = 0; i < config.JacobiIterationForDiffusion; i++)
            {
                this.Diffusion(mat, velocity.Read.Data, velocity.Write.Data, new float3(1) / velocity.Read.Size, config.Viscosity, config.Timestep);
                velocity.SwipeBuffer();
            }

        }

        protected void Diffusion(Material mat, RenderTexture velocity, RenderTexture dest, float3 invSize, float viscosity, float dt)
        {
            mat.SetVector("_InverseSize", new Vector4(invSize.x, invSize.y, invSize.z));
            float alpha = 1.0f / (viscosity * dt + Mathf.Epsilon);
            mat.SetFloat("_Alpha", alpha);
            mat.SetFloat("_InverseBeta", 1.0f / (4.0f + alpha));
            mat.SetTexture("_Source", velocity);
            mat.SetTexture("_ConstantVector", velocity);
            Graphics.Blit(null, dest, mat, 0);
        }

    }
}