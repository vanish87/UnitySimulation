using System.Collections;
using System.Collections.Generic;
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class BoundaryMeshParticle : Boundary, IParticleBoundary
    {
        [SerializeField] protected Mesh mesh;
        public IEnumerable<float3> Sample()
        {
            return Sampler.SampleMeshSurface(this.mesh, Mathf.CeilToInt(this.Parameter.x));
        }
    }

}