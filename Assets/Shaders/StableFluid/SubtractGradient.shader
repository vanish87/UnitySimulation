Shader "Hidden/FluidSim/SubtractGradient"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	
	// ======================================================================
	// 変数 (Variables)
	// ======================================================================
	sampler2D _Pressure;    // 圧力場
	sampler2D _Velocity;    // 速度場

	float2    _InverseSize; // テクセルサイズ

	// ======================================================================
	// 関数 (Functions)
	// ======================================================================
	// Pass 0 : 速度場から圧力場の勾配を引く (Subtract Gradient)
	float4 frag(v2f_img i) : SV_Target
	{
		float pN = tex2D(_Pressure, i.uv.xy + _InverseSize.xy * float2( 0,  1)).x;
		float pS = tex2D(_Pressure, i.uv.xy + _InverseSize.xy * float2( 0, -1)).x;
		float pE = tex2D(_Pressure, i.uv.xy + _InverseSize.xy * float2( 1,  0)).x;
		float pW = tex2D(_Pressure, i.uv.xy + _InverseSize.xy * float2(-1,  0)).x;
	
		float2 w = tex2D(_Velocity, i.uv.xy).xy;
		float2 grad = 0.5 * float2(pE - pW, pN - pS);
		float2 u = w - grad;

		return float4(u, 0.0, 1.0);
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// Pass 0 : 速度場から圧力場の勾配を引く (Subtract Gradient)
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
