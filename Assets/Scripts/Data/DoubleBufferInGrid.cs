
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class DoubleBufferInGrid<T> : DoubleBuffer<T>, IData, IPlugin
    {
        public bool Enabled => this.isActiveAndEnabled;
        public IEnumerable<int> Steps => new List<int> { (int)SimulationStep.PrepareData };
        public GPUGridBuffer<uint2> Grid { get; protected set; }
        protected SortObjectInGrid GridSorter { get; set; }
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);

            this.Grid = this.GetComponentInChildren<GPUGridBuffer<uint2>>();
            this.GridSorter = this.GetComponentInChildren<SortObjectInGrid>();
        }
        public override void Deinit(params object[] parameter)
        {
            base.Deinit(parameter);
        }
        public virtual void OnSimulationStep(int stepIndex, ISimulation sim, ISimulationData data)
        {
            this.GridSorter.Sort(this.Read.Data, this.Grid.Data, this.Grid.Size, this.Grid.Spacing, this.Grid.Min, this.Grid.Max, this.Write.Data);
            this.SwipeBuffer();
        }
    }
}