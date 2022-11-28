
int _NumberOfParticle;
float _H;
float2 _PressureK;// k1 and k2 in State Equation Pressure
float _RestDensity;
float _ParticleMass;
float2 _ParticleGamma;// x is boundary gamma; y is fluid gamma
float _Viscosity;
// float3 _Gravity;
float _TimeStep;
int _StepIteration;
float _MaxSpeed;
float2 _ParticleLife;

float _VorticityConfinement;
float3 _NU_T;
float3 _NU_EXT;
float3 _Theta;
float4 _TransferForceParameter;
float4 _TransferTorqueParameter;
float4 _AngularVelocityParameter;
float4 _LinearVelocityParameter;

float3 _SpaceMin;
float3 _SpaceMax;
float4x4 _SimSpaceLocalToWorld;
float4x4 _SimSpaceWorldToLocal;