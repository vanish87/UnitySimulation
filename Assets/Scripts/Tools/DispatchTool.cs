
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class DispatchTool
    {
        public static void Dispatch(ComputeShader cs, string kernel, int3 dsize)
        {
            var k = cs.FindKernel(kernel);
            DispatchTool.Dispatch(cs, k, dsize);
        }
        public static void DispatchNoGroup(ComputeShader cs, string kernel, int3 dsize)
        {
            var k = cs.FindKernel(kernel);
            DispatchTool.DispatchNoGroup(cs, k, dsize);
        }
        public static void DispatchNoGroup(ComputeShader cs, int k, int3 dsize)
        {
            cs.SetInt("_DispatchedX", dsize.x);
            cs.SetInt("_DispatchedY", dsize.y);
            cs.SetInt("_DispatchedZ", dsize.z);
            cs.Dispatch(k, dsize.x, dsize.y, dsize.z);
        }
        public static void Dispatch(ComputeShader cs, int k, int3 dsize)
        {
            uint threadNumX = 0; uint threadNumY = 0; uint threadNumZ = 0;
            cs.GetKernelThreadGroupSizes(k, out threadNumX, out threadNumY, out threadNumZ);
            var dx = GetDispatchSize(dsize.x, threadNumX);
            var dy = GetDispatchSize(dsize.y, threadNumY);
            var dz = GetDispatchSize(dsize.z, threadNumZ);
            cs.SetInt("_DispatchedX", dsize.x);
            cs.SetInt("_DispatchedY", dsize.y);
            cs.SetInt("_DispatchedZ", dsize.z);
            cs.Dispatch(k, dx, dy, dz);
        }
        public static int GetDispatchSize(int desired, uint threadNum)
        {
            UnityEngine.Assertions.Assert.IsTrue(desired > 0);
            UnityEngine.Assertions.Assert.IsTrue(threadNum > 0);
            return math.max(1, (int)((desired + threadNum - 1) / threadNum));
        }

    }
}