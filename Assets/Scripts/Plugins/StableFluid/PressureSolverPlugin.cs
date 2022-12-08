using System.Collections.Generic;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Simulation.Fluid.StableFluid
{
    public class PressureSolverPlugin : MonoBehaviour, IPlugin
    {
        public string Identifier => this.ToString();
        public bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation + (int)Step.PressureSolver };
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

            // pressure.ResetAll();
            for (var i = 0; i < config.JacobiIterationForPressure; i++)
            {
                this.SolvePressure(mat, pressure.Read.Data, velocityDivergence.Read.Data, pressure.Write.Data, new float3(1) / velocity.Read.Size);
                pressure.SwipeBuffer();
            }

        }

        protected void SolvePressure(Material mat, RenderTexture pressure, RenderTexture divergence, RenderTexture dest, float3 invSize)
        {
            mat.SetVector("_InverseSize", new Vector4(invSize.x, invSize.y, invSize.z));
            mat.SetFloat("_Alpha", -1.0f);
            mat.SetFloat("_InverseBeta", 0.25f);
            mat.SetTexture("_Source", pressure);
            mat.SetTexture("_ConstantVector", divergence);
            Graphics.Blit(null, dest, mat, 0);
        }

    }
}