
using Unity.Mathematics;

namespace Simulation
{
    public class CPUField2D : IStructuredData<float2[,], float2>, IField<float2, float2>
    {
        public float2[,] Data => throw new System.NotImplementedException();

        public int3 Size => throw new System.NotImplementedException();

        public string Identifier => throw new System.NotImplementedException();

        public bool Inited => throw new System.NotImplementedException();

        public Access Access => Access.CPUReadWrite;

        public int Length => throw new System.NotImplementedException();

        public void Deinit(params object[] parameter)
        {
        }

        public float2 Gradient(float2 uv, SampleType sp = SampleType.Center)
        {
            return default;
        }

        public void Init(params object[] parameter)
        {
        }

        public float2 Sample(float2 uv, SampleType sp = SampleType.Center)
        {
            return default;
        }
    }
}
