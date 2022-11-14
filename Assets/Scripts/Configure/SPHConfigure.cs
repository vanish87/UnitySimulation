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
            public float3 gravity = new float3(0, -9.8f, 0);
            public float preferredTimeStep = 0.05f;
            //x is max speed to assure cfl condition
            public float maxSpeed = 1f;
        }
        public bool Inited => true;
        public float SmoothLength => this.data.smoothLength;
        public float2 PressureK => this.data.pressureK;
        public float2 ParticleGamma => this.data.particleGamma;
        public float RestDensity => this.data.restDensity;
        public float ParticleMass => this.data.particleMass;
        public float Viscosity => this.data.viscosity;
        public float VorticityConfinement => this.data.vorticityConfinement;
        public float3 NU_T => this.data.nu_t;
        public float3 NU_EXT => this.data.nu_ext;
        public float3 Theta => this.data.theta;
        public float3 Gravity => this.data.gravity;
        public float PreferredTimeStep => this.data.preferredTimeStep;
        public float MaxSpeed => this.data.maxSpeed;
        [SerializeField] protected Data data = new Data();
        public void Init(params object[] parameter)
        {
        }
        public void Deinit(params object[] parameter)
        {
        }
    }
}