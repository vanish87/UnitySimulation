
using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Fluid.StableFluid
{
    public class StableFluidConfigure : MonoBehaviour, IStableFluidConfigure
    {
        [System.Serializable]
        public class Data
        {
            public float preferredTimeStep = 0.05f;
            public float velocityDissipation = 0.999f;
            public int jacobiIterationForDiffusion = 20;
            public int jacobiIterationForPressure = 50;
            public float viscosity = 1f;
        }
        public virtual bool Inited => true;
        public virtual string Identifier => this.ToString();
        public virtual float Timestep => this.data.preferredTimeStep;
        public virtual float VelocityDissipation => this.data.velocityDissipation;
        public virtual int JacobiIterationForDiffusion => this.data.jacobiIterationForDiffusion;
        public virtual int JacobiIterationForPressure => this.data.jacobiIterationForPressure;
        public virtual float Viscosity => this.data.viscosity;

        [SerializeField] protected Data data = new Data();
        public void Init(params object[] parameter)
        {
        }

        public void Deinit(params object[] parameter)
        {
        }
    }
}