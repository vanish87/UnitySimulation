
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    [SelectionBase]
    public class DoublePressureTexture2D : DoubleBuffer<RenderTexture, float>, ITextureField
    {
        public override string Identifier => Fluid.DataType.PressureTexture.ToString();
        public Texture Texture => this.Read.Data;
        public float3 Center => this.transform.localPosition;
        public quaternion Rotation => this.transform.localRotation;
        public float3 Scale => this.transform.localScale;
        public float4x4 TRS => this.transform.localToWorldMatrix;

        public override void SetData(float[] data)
        {
            throw new System.NotImplementedException();
        }
    }

}