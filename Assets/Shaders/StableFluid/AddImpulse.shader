Shader "Hidden/FluidSim/AddImpulse"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	// ======================================================================
	// 変数
	// ======================================================================
	sampler2D _Source;          // 入力場

	float4 _ImpulseParams;	    // point.x, point.y, radius, power
	float2 _ImpulseDirection;	// direction.x, direction.y

	// ======================================================================
	// 関数
	// ======================================================================
	float gaussian(float d2, float radius)
	{
		return exp(-d2 / radius);
	}

	// Pass 0 : 速度場に力を加える
	float4 frag(v2f_img i) : SV_Target
	{
		float3 velocityDir = float3(_ImpulseDirection.xy, 0.0);
		velocityDir = -1.0 * max(-1.0, min(1.0, velocityDir));
		float2 diff    = _ImpulseParams.xy - i.uv.xy;
		float3 impulse = min(0.8, velocityDir.xyz * gaussian(dot(diff, diff), _ImpulseParams.z));

		float3 src = tex2D(_Source, i.uv.xy);
		return float4(src.xyz + _ImpulseParams.w * impulse.xyz, 1.0);
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// Pass 0 : 速度場に力を加える
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
