#ifndef _CS_MPM_MPMKERNEL_HLSL
#define _CS_MPM_MPMKERNEL_HLSL

#include "../Constant.hlsl"

inline float N(float x)
{
    x = abs(x);

    if (x < 0.5f) return 0.75f - x * x;
    if (x < 1.5f) return 0.5f * (1.5f - x) * (1.5f - x);
    return 0;
}

inline float DevN(float x)
{
    float absx = abs(x);
    if (absx < 0.5f) return -2 * x;
    if (absx < 1.5f) return x > 0 ? absx - 1.5f : -(absx - 1.5f);
    return 0;
}

inline float3x3 InvD(float3 h)
{
	return 4.0f * Identity3x3 * h.x * h.x;
}

inline float GetWeight(float3 p_pos, float3 c_pos, float3 h)
{
	float3 dis = p_pos - c_pos;
	float3 invH = 1.0f / h;
	dis *= invH;

	return  N(dis.x) * N(dis.y) * N(dis.z);
}


#endif /* _CS_MPM_MPMKERNEL_HLSL */
