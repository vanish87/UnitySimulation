using System.Collections;
using System.Collections.Generic;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Simulation.Fluid.StableFluid
{
    public class AddForcePlugin : MonoBehaviour, IPlugin
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
            var config = data.Configures.Find<IStableFluidConfigure>();
            var space = data.Spaces.Find<StableFluidSimulationSpace>();

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

            this.AddForce(mat, velocity.Read.Data, velocity.Write.Data, space, boundary);
            velocity.SwipeBuffer();

        }

        protected void AddForce(Material mat, RenderTexture velocity, RenderTexture dest, StableFluidSimulationSpace space, IBoundaryController boundary)
        {
            var min = space.Center - 0.5f * space.Scale;
            var max = space.Center + 0.5f * space.Scale;
            mat.SetVector("_SimMin", new Vector4(min.x, min.y, min.z));
            mat.SetVector("_SimMax", new Vector4(max.x, max.y, max.z));
            mat.SetTexture("_Source", velocity);
            mat.SetBuffer("_BoundaryBuffer", boundary.BoundaryBuffer);
            mat.SetInt("_BoundaryBufferCount", boundary.BoundaryBuffer.count);
            Graphics.Blit(null, dest, mat, 0);
        }

    }
}