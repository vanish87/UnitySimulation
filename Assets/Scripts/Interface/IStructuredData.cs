
using Unity.Mathematics;

namespace Simulation
{
    public interface IStructuredData<StructType, ElementType> : IData
    {
        // Dimension Dim { get; }
        // Access Access { get; }
        StructType Data { get; }
        int3 Size { get; }
        int Length { get; }
    }
}
