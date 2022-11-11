using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public enum Access
    {
        CPUReadWrite,
        GPURead,
        GPUReadWrite,
    }
    public interface IAccess
    {
        Access Access { get; }
    }
    public interface IData : IInitialize
    {
        string Identifier { get; }
        // Storage Storage { get; }
    }
}