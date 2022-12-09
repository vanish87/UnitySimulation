Shader "Hidden/FluidSim/AddImpulse"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	#include "../CS/Tools.hlsl"
	#include "../CS/UVTools.hlsl"
	#include "../CS/Boundary/BoundaryData.hlsl"
	#include "../CS/Boundary/Boundary.hlsl"
	// ======================================================================
	// 変数
	// ======================================================================
	sampler2D _Source;          // 入力場

	float4 _SimMin;
	float4 _SimMax;


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
		float3 uv = float3(i.uv.xy, 0);
		float3 pos = lerp(_SimMin, _SimMax, uv);
		float3 force = GetBoundaryVelocityForce(pos);

		float3 src = tex2D(_Source, i.uv.xy);
		return float4(src.xyz + force, 1.0);
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
