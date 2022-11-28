using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class DoubleBufferInGrid<T> : DoubleBuffer<T>, IData
    {
        public GPUGridBuffer<uint2> Grid { get; protected set; }
        protected SortObjectInGrid GridSorter { get; set; }
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);

            this.Grid = this.GetComponentInChildren<GPUGridBuffer<uint2>>();
            this.GridSorter = this.GetComponentInChildren<SortObjectInGrid>();

            Debug.Assert(this.Grid != null);
            Debug.Assert(this.GridSorter != null);
        }
        public override void Deinit(params object[] parameter)
        {
            base.Deinit(parameter);
        }

        protected virtual void OnSortParticle()
        {
            this.GridSorter.Sort(this.Read.Data, this.Grid.Data, this.Grid.Size, this.Grid.Spacing, this.Grid.Min, this.Grid.Max, this.Write.Data);
            this.SwipeBuffer();
        }
    }
}