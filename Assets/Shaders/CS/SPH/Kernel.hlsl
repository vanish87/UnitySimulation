
#define USE_2D_KERNEL

static const float PI = 3.141592653f;

float Poly6W()
{
	return 0;
}

float CubicSplineW(float3 r, float h)
{
	#if defined(USE_2D_KERNEL)
	float c = 40.0/(7 * PI * pow(h,2));
	// r.z = 0;
	#else
	float c = 8.0/(PI * pow(h,3));
	#endif
	float q = length(r)/h;

	if(0 <= q && q <= 0.5) return c * (6 * (pow(q,3) - pow(q,2)) +1);
	if(0.5 < q && q <= 1) return c * 2 * pow(1-q,3);

	return 0;

}
float3 CubicSplineWGrad(float3 r, float h)
{
	#if defined(USE_2D_KERNEL)
	float c = 40.0/(7 * PI * pow(h,2));
	// r.z = 0;
	#else
	float c = 8.0/(PI * pow(h,3));
	#endif

	float rl = length(r);
	if(rl > 0)
	{
		float q = rl/h;
		float3 grandq = 1/h * (r / rl);

		if(0 <= q && q <= 0.5) return c * 6 * ( 3 * pow(q,2) -  2*q) * grandq;
		if(0.5 < q && q <= 1) return - c * 2 * 3 * pow(1-q,2) * grandq;
	}

	return 0;

}

float W(float3 r, float h)
{
	return CubicSplineW(r, h);
}

float3 WGrad(float3 r, float h)
{
	return CubicSplineWGrad(r, h);
}

//State Equation Pressure
float CalculateSEPressure(float density, float k1, float k2, float restDensity)
{
	return k1 * max(pow(max(density / restDensity, 0), k2) - 1, 0);
}

float3 CalculateGradPressure(float h, float mass, float3 r, float P_pressure, float N_pressure, float P_density, float N_density) 
{
	float3 grandW = WGrad(r, h);
	float symmetic = (P_pressure / pow(P_density,2)) + (N_pressure/pow(N_density,2)); 

	return mass * symmetic * grandW;
}

float3 CalculateLapVelocity(float h, float mass, float viscosity, float3 r, float3 P_velocity, float3 N_velocity, float P_density, float N_density) 
{
	float rl = length(r);
	if(rl > 0)
	{
		float nu = viscosity/P_density;
		float3 gradW = WGrad(r, h);
		float3 Vij = P_velocity - N_velocity;

		#if defined(USE_2D_KERNEL)
		// Vij.z = 0;
		#endif

		float3 lap = - mass/N_density * Vij * (2*length(gradW)/rl);
		return mass * nu * lap;
	}
	else
	{
		return 0;
	}
}

float GetReynoldsNumber(float3 velocity, float viscosity, float density, float L)
{
		float nu = viscosity/density;
		return length(velocity) * L / nu;
}
