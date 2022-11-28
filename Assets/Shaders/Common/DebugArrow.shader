Shader "Debug/Arrow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "Arrow.cginc"
	#include "../CS/UVTools.cginc"

	sampler2D _MainTex;

	fixed4 frag(v2f_img i) : SV_Target
	{
		float4 mainColor = tex2D(_MainTex, i.uv.xy);
		float3 dir = tex2D(_MainTex, ArrowTileCenterCoord(i.uv.xy));
		dir = RGBToNormal(dir);
		float arrowStrength = Arrow(i.uv.xy, dir);
		return lerp(mainColor * 0.5f, _ArrowColor, arrowStrength);
	}
	ENDCG

	SubShader
	{
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
