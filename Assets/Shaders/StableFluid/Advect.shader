Shader "Hidden/FluidSim/Advect"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	
	// ======================================================================
	// 変数 (Variables)
	// ======================================================================
	sampler2D _Source;	 // 入力場
	sampler2D _Velocity; // 速度場

	float  _Dissipation; // 減衰率
	float  _TimeStep;	 // タイムステップ
	float2 _InverseSize; // テクセルサイズ

	// ======================================================================
	// 関数 (Functions)
	// ======================================================================
	// Pass 0 : 移流 (Advection)
	float4 frag(v2f_img i) : SV_Target
	{
		float2 u = tex2D(_Velocity, i.uv.xy).xy;
		
		float2 backtracedCoord = float2(i.uv.xy - (u.xy * _InverseSize.xy * _TimeStep));
		return float4(_Dissipation * tex2D(_Source, backtracedCoord).xy, 0.0, 1.0);
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// Pass 0 : 
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
