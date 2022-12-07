Shader "Hidden/FluidSim/Boundary"
{
	Properties
	{
	}
	CGINCLUDE
	#include "UnityCG.cginc"

	// ======================================================================
	// 変数 (Variables)
	// ======================================================================
	sampler2D _Source;      // 入力場
	float2    _InverseSize; // テクセルサイズ

	// ======================================================================
	// 関数 (Functions)
	// ======================================================================
	// Pass 0 : 境界条件 (Boundary Conditions)
	float4 frag(v2f_img i) : SV_Target
	{
		bool isBoundary =
			i.uv.x < _InverseSize.x || i.uv.x > 1.0 - _InverseSize.x ||
			i.uv.y < _InverseSize.y || i.uv.y > 1.0 - _InverseSize.y;
		
		float2 src = tex2D(_Source, i.uv.xy).xy;

		src.xy *= (isBoundary ? 0.0 : 1.0);
		
		return float4(src.xy, 0.0, 1.0);
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		
		// Pass 0 : 境界条件 (Boundary Conditions)
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
