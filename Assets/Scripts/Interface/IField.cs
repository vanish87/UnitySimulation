
using UnityEngine;

namespace Simulation
{
    public enum SampleType
    {
        Center,
        LeftDown,
        RightUp,
    }
    public interface IField<Dim, Element>
    {
        Element Sample(Dim uv, SampleType sp = SampleType.Center);
        Element Gradient(Dim uv, SampleType sp = SampleType.Center);

    }
    public interface ITextureField : ISpace
    {
        Texture Texture { get; }
    }
}
