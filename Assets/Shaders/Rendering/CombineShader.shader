
Shader "Simulation/Combine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }


	CGINCLUDE
	#include "UnityCG.cginc"
	#include "../CS/UVTools.hlsl"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float4 _ST;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		//o.uv.y = 1 - v.uv.y;
		return o;
	}

	fixed4 fragComposite(v2f i) : SV_Target
	{
		float2 uvmin = _ST.xy;
		float2 uvmax = uvmin + _ST.zw;

		if(all(uvmin <= i.uv) && all(i.uv <= uvmax)) return tex2D(_MainTex, UV_GlobalToLocal(i.uv, _ST));

		discard;
		return 0;
	}

	fixed4 fragSeperate(v2f i) : SV_Target
	{
		return tex2D(_MainTex, UV_LocalToGlobal(i.uv, _ST));
	}
	ENDCG

    SubShader
    {
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragComposite			
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragSeperate			
			ENDCG
		}
    }
}
