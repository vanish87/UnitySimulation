Shader "Hidden/FluidSim/Divergence"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	
	// ======================================================================
	// 変数 (Variables)
	// ======================================================================
	sampler2D _Source;		// 入力場
	float2    _InverseSize; // テクセルサイズ

	// ======================================================================
	// 関数 (Functions)
	// ======================================================================
	// Pass 0 : 発散を計算 (Compute Divergence)
	float4 frag(v2f_img i) : SV_Target
	{
		float2 vN = tex2D(_Source, i.uv.xy + _InverseSize.xy * float2( 0,  1)).xy;
		float2 vS = tex2D(_Source, i.uv.xy + _InverseSize.xy * float2( 0, -1)).xy;
		float2 vE = tex2D(_Source, i.uv.xy + _InverseSize.xy * float2( 1,  0)).xy;
		float2 vW = tex2D(_Source, i.uv.xy + _InverseSize.xy * float2(-1,  0)).xy;
	
		float result = 0.5 * ((vE.x - vW.x) + (vN.y - vS.y));

		return float4(result, 0.0, 0.0, 1.0);
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		
		// Pass 0 : 発散 (Compute Divergence)
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
