
//separate
float2 UV_LocalToGlobal(float2 uv, float4 st)
{
	return saturate(st.xy + uv * st.zw);
}
//composite
float2 UV_GlobalToLocal(float2 uv, float4 st)
{
	return saturate((uv - st.xy)/ st.zw);
}

int2 UV_LocalToGlobal(float2 uv, float4 st, int2 texSize)
{
	return clamp(UV_LocalToGlobal(uv, st) * texSize, 0, texSize-1);
	// return lerp(int2(0,0), texSize-1, UV_LocalToGlobal(uv, st));
}
int2 UV_GlobalToLocal(float2 uv, float4 st, int2 texSize)
{
	return clamp(UV_GlobalToLocal(uv, st) * texSize, 0, texSize-1);
	// return lerp(int2(0,0), texSize-1, UV_GlobalToLocal(uv, st));
}

float3 RGBToNormal(float3 rgbNormal)
{
	return normalize(rgbNormal * 2.0f - 1.0f);
}
