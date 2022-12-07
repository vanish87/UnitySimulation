Shader "Hidden/FluidSim/Jacobi"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"

	// ======================================================================
	// 変数 (Variables)
	// ======================================================================
	sampler2D _Source;		   // 入力場
	sampler2D _ConstantVector; // 定数ベクトル場
	
	float  _Alpha;			   // α
	float  _InverseBeta;       // β 逆数

	float2 _InverseSize;       // テクセルサイズ

	// ======================================================================
	// 関数 (Functions)
	// ======================================================================
	// Pass 0 : ヤコビ反復法 (Jacobi Interations)
	float4 frag(v2f_img i) : SV_Target
	{
		float2 xN = tex2D(_Source, i.uv + _InverseSize.xy * float2( 0,  1)).xy;
		float2 xS = tex2D(_Source, i.uv + _InverseSize.xy * float2( 0, -1)).xy;
		float2 xE = tex2D(_Source, i.uv + _InverseSize.xy * float2( 1,  0)).xy;
		float2 xW = tex2D(_Source, i.uv + _InverseSize.xy * float2(-1,  0)).xy;
		
		float2 bC = tex2D(_ConstantVector, i.uv.xy).xy;

		float2 result = (xW + xE + xS + xN + _Alpha * bC) * _InverseBeta;

		return float4(result, 0.0, 1.0);
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		// Pass 0 : ヤコビ反復法 (Jacobi Interations)
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
