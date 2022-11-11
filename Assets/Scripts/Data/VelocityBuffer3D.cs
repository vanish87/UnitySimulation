using Unity.Mathematics;

namespace Simulation
{
    public class VelocityBuffer3D : GPUData<float3>
    {
        public override string Identifier => this.ToString();
    }
}