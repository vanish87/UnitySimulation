
#include "Constant.hlsl"

void MinMaxScaleDissipation(inout float value, float4 minMaxScaleDissipation)
{
	const float s = minMaxScaleDissipation.x;
	const float2 minMax = minMaxScaleDissipation.yz;
	const float dissipation = minMaxScaleDissipation.w;
	value = s > 0? clamp(value, minMax.x*s, minMax.y*s):value;
	value *= dissipation;
}

void MinMaxScaleDissipation(inout float3 value, float4 minMaxScaleDissipation)
{
	MinMaxScaleDissipation(value.x, minMaxScaleDissipation);
	MinMaxScaleDissipation(value.y, minMaxScaleDissipation);
	MinMaxScaleDissipation(value.z, minMaxScaleDissipation);
}

float wang_hash01(uint seed)
{
	seed = (seed ^ 61) ^ (seed >> 16);
	seed *= 9;
	seed = seed ^ (seed >> 4);
	seed *= 0x27d4eb2d;
	seed = seed ^ (seed >> 15);
	return float(seed) / 4294967295.0; // 2^32-1
}

float3 GetPos(float4x4 localToWorld)
{
	return float3(localToWorld[0][3], localToWorld[1][3], localToWorld[2][3]);
}

float3 GetScale(float4x4 localToWorld)
{
	return float3(localToWorld[0][0], localToWorld[1][1], localToWorld[2][2]);
}

#include "UnityCG.cginc"
float3 GenerateRandomPos01(int seed)
{
	uint t = (uint)fmod(_Time * 8, 65535.0);
	int3 offset = int3(0, 173, 839) + t;
	return float3(wang_hash01(seed + offset.x), wang_hash01(seed + offset.y), wang_hash01(seed + offset.z));
}

float3 TransformPoint(float3 pos, float4x4 martix)
{
	float4 p = float4(pos, 1);
	p = mul(martix, p);
	p /= p.w;
	return p.xyz;
}

float3 WorldPosToUV(float3 pos, float4x4 martix)
{
	float3 uv = TransformPoint(pos, martix);
	return uv + 0.5f;
}

float3 GenerateRandomPos(int seed, float4x4 localToWorld = Identity)
{
	return TransformPoint(GenerateRandomPos01(seed) - 0.5f, localToWorld);
}

// void DeactiveParticle(inout Particle p, float4x4 localToWorld = IDENTITY)
// {
// 	int seed = p.UUID();
// 	p.SetActive(false);
// 	p.SetType(0);
// 	p.pos = GenerateRandomPos(seed, localToWorld);
// }