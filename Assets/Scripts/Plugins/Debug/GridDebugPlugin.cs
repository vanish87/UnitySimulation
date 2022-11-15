using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Fluid.Debug
{
    public class GridDebugPlugin : MonoBehaviour, IPlugin
    {
        public IEnumerable<int> Steps => new List<int>() { (int)SimulationStep.OnSimulation };
        public bool Inited => true;
        public bool Enabled => this.enabled;
        [SerializeField] protected ComputeShader debugCS;
        protected const string Kernel = "Debug";

        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
        public void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            var particle = data.Data.OfType<ParticleBufferDouble>().FirstOrDefault();
            this.SetBuffer(particle.Read.Data);
            this.debugCS.SetVector("pos", this.transform.localPosition);

            var grid = data.Data.OfType<SPH.SPHGridBuffer>().FirstOrDefault();
            this.OnSetupGridParameter(grid);

            var k = this.debugCS.FindKernel("Reset");
            this.debugCS.SetBuffer(k, "_ParticleBufferRW", particle.Read.Data);
            DispatchTool.Dispatch(this.debugCS, "Reset", particle.Read.Size);

            DispatchTool.Dispatch(this.debugCS, Kernel, 1);
        }

        protected void SetBuffer(ComputeBuffer particle)
        {
            var cs = this.debugCS;
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_ParticleBufferRW", particle);

        }
        protected void OnSetupGridParameter(SPH.SPHGridBuffer grid)
        {
            // this.cs.SetInt("_GridCenterMode", grid.centerMode);
            var cs = this.debugCS;
            cs.SetVector("_GridSize", new Vector4(grid.Size.x, grid.Size.y, grid.Size.z, 0));
            cs.SetVector("_GridSpacing", new Vector4(grid.Spacing.x, grid.Spacing.y, grid.Spacing.z, 0));
            cs.SetVector("_GridMin", new Vector4(grid.Min.x, grid.Min.y, grid.Min.z, 0));
            cs.SetVector("_GridMax", new Vector4(grid.Max.x, grid.Max.y, grid.Max.z, 0));
            var k = cs.FindKernel(Kernel);
            cs.SetBuffer(k, "_GridBuffer", grid.Data);
        }
    }
}
