using Simulation.Tool;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Fluid.SPH
{
    public class SPHConfigure : MonoBehaviour, ISPHConfigure
    {
        [System.Serializable]
        public class Data
        {
            public float smoothLength = 0.3f;
            public float2 pressureK = new float2(400, 3);
            public float2 particleGamma = new float2(20, 1);
            public float restDensity = 1000;
            public float particleMass = 18f;
            public float viscosity = 10f;
            public float vorticityConfinement = 1f;
            //for Micropolar Model
            public float3 nu_t = 0.05f;
            public float3 nu_ext = 0f;
            public float3 theta = 1f;

            public bool useCFL = false;
            public float preferredTimeStep = 0.05f;
            //x is max speed to assure cfl condition
            public float maxSpeed = 1f;
        }
        public virtual bool Inited => true;
        public virtual float SmoothLength => this.data.smoothLength;
        public virtual float2 PressureK => this.data.pressureK;
        public virtual float2 ParticleGamma => this.data.particleGamma;
        public virtual float RestDensity => this.data.restDensity;
        public virtual float ParticleMass => this.data.particleMass;
        public virtual float Viscosity => this.data.viscosity;
        public virtual float VorticityConfinement => this.data.vorticityConfinement;
        public virtual float3 NU_T => this.data.nu_t;
        public virtual float3 NU_EXT => this.data.nu_ext;
        public virtual float3 Theta => this.data.theta;
        public virtual float PreferredTimeStep => this.data.useCFL ? this.data.preferredTimeStep = FluidTool.GetCFL(this.SmoothLength, this.MaxSpeed) : this.data.preferredTimeStep;
        public virtual float MaxSpeed => this.data.maxSpeed;
        public virtual string Identifier => this.ToString();

        [SerializeField] protected Data data = new Data();
        public virtual void Init(params object[] parameter)
        {
        }
        public virtual void Deinit(params object[] parameter)
        {
        }
    }
}
