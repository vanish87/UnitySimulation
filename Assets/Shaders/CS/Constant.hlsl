#ifndef _SHADERS_CS_CONSTANT_HLSL
#define _SHADERS_CS_CONSTANT_HLSL

// static const float4x4 IDENTITY = float4x4 (1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1);
static const float PI = 3.141592653f;

static const float4x4 Identity =
{
	{ 1, 0, 0, 0 },
	{ 0, 1, 0, 0 },
	{ 0, 0, 1, 0 },
	{ 0, 0, 0, 1 }
}; 

static const float3x3 Identity3x3 =
{
	{ 1, 0, 0},
	{ 0, 1, 0},
	{ 0, 0, 1},
};
static const float3x3 Identity3x32D =
{
	{ 1, 0, 0},
	{ 0, 1, 0},
	{ 0, 0, 0},
};
static const float2x2 Identity2x2 =
{
	{ 1, 0},
	{ 0, 1},
};

#endif /* _SHADERS_CS_CONSTANT_HLSL */
