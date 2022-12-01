

using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    [RequireComponent(typeof(GridIndexSort))]
    public class SortObjectInGrid : MonoBehaviour
    {
        public enum Kernel
        {
            ObjectToGridIndex,
            ClearGridIndex,
            BuildGridIndex,
            BuildSortedObject,
        };
        [SerializeField] protected ComputeShader gridCS;
        protected GridIndexSort GridSort => this.gridSort ??= this.GetComponent<GridIndexSort>();
        protected GridIndexSort gridSort;
        protected GraphicsBuffer objectGridIndexBuffer;
        public void Sort(GraphicsBuffer objectBuffer, GraphicsBuffer grid, int3 size, float3 spacing, float3 min, float3 max, GraphicsBuffer sortedBuffer)
        {
            var count = objectBuffer.count;
            this.CheckBufferChanged(objectBuffer);

            this.gridCS.SetVector("_GridSize", new Vector4(size.x, size.y, size.z, 0));
            this.gridCS.SetVector("_GridSpacing", new Vector4(spacing.x, spacing.y, spacing.z, 0));
            this.gridCS.SetVector("_GridMin", new Vector4(min.x, min.y, min.z, 0));
            this.gridCS.SetVector("_GridMax", new Vector4(max.x, max.y, max.z, 0));

            var k = this.gridCS.FindKernel(Kernel.ObjectToGridIndex.ToString());
            this.SetBuffer(k, objectBuffer, grid, sortedBuffer);
            this.Dispatch(k, count, 1, 1);
            this.GridSort.Sort(ref this.objectGridIndexBuffer);

            k = this.gridCS.FindKernel(Kernel.ClearGridIndex.ToString());
            this.SetBuffer(k, objectBuffer, grid, sortedBuffer);
            this.Dispatch(k, size.x, size.y, size.z);

            k = this.gridCS.FindKernel(Kernel.BuildGridIndex.ToString());
            this.SetBuffer(k, objectBuffer, grid, sortedBuffer);
            this.Dispatch(k, count, 1, 1);

            k = this.gridCS.FindKernel(Kernel.BuildSortedObject.ToString());
            this.SetBuffer(k, objectBuffer, grid, sortedBuffer);
            this.Dispatch(k, count, 1, 1);
        }
        protected int GetDispatchSize(int desired, uint threadNum)
        {
            UnityEngine.Assertions.Assert.IsTrue(desired > 0);
            UnityEngine.Assertions.Assert.IsTrue(threadNum > 0);
            return (int)((desired + threadNum - 1) / threadNum);
        }

        protected void SetBuffer(int kernel, GraphicsBuffer objectBuffer, GraphicsBuffer grid, GraphicsBuffer sorted)
        {
            this.gridCS.SetBuffer(kernel, "_ObjectBufferRead", objectBuffer);
            this.gridCS.SetBuffer(kernel, "_ObjectBufferSorted", sorted);
            this.gridCS.SetBuffer(kernel, "_ObjectGridIndexBuffer", this.objectGridIndexBuffer);

            this.gridCS.SetBuffer(kernel, "_GridBuffer", grid);
        }
        protected void CheckBufferChanged(GraphicsBuffer objectBuffer)
        {
            if (this.objectGridIndexBuffer == null || this.objectGridIndexBuffer.count != objectBuffer.count)
            {
                this.objectGridIndexBuffer?.Release();
                //create new buffer for object index
				//int2(grid index, particle index)
                this.objectGridIndexBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, objectBuffer.count, Marshal.SizeOf<uint2>());
            }
        }
        protected void Dispatch(int k, int X, int Y, int Z)
        {
            uint threadNumX = 0; uint threadNumY = 0; uint threadNumZ = 0;
            this.gridCS.GetKernelThreadGroupSizes(k, out threadNumX, out threadNumY, out threadNumZ);

            this.gridCS.SetInt("_DispatchedX", X);
            this.gridCS.SetInt("_DispatchedY", Y);
            this.gridCS.SetInt("_DispatchedZ", Z);
            this.gridCS.Dispatch(k, this.GetDispatchSize(X, threadNumX), this.GetDispatchSize(Y, threadNumY), this.GetDispatchSize(Z, threadNumZ));
        }

        protected virtual void OnDestroy()
        {
            this.objectGridIndexBuffer?.Release();
        }
    }
}