using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class BoundarySDFField : Boundary, ISDFFieldBoundary
    {
		public float4 ST { get => this.parameter; set => this.parameter = value; }
		public Texture Texture => this.sdfField;
		[SerializeField] protected Texture sdfField; //3D noise maybe
        public override void Init(params object[] parameter)
        {
            base.Init(parameter);
            this.type = BoundaryType.SDFField;
        }
    }
}
