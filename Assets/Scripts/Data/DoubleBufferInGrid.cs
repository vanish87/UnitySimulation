using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public abstract class DoubleBufferInGrid<T, Cell> : DoubleBuffer<GraphicsBuffer, T>
    {
        public GPUGridBuffer<Cell> Grid { get; protected set; }
        protected SortObjectInGrid GridSorter { get; set; }
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);

            this.Grid = this.GetComponentInChildren<GPUGridBuffer<Cell>>();
            this.GridSorter = this.GetComponentInChildren<SortObjectInGrid>();

            Debug.Assert(this.Grid != null);
            Debug.Assert(this.GridSorter != null);
        }
        public override void Deinit(params object[] parameter)
        {
            base.Deinit(parameter);
        }

        public virtual void OnSortParticle()
        {
            this.GridSorter.Sort(this.Read.Data, this.Grid.Data, this.Grid.Size, this.Grid.Spacing, this.Grid.Min, this.Grid.Max, this.Write.Data);
            this.SwipeBuffer();
        }
        public override void SetData(T[] data)
        {
            Debug.Assert(data.Length == this.Read.Data.count);
            Debug.Assert(data.Length == this.Write.Data.count);
            this.Read.Data.SetData(data);
            this.Write.Data.SetData(data);
        }
    }
}