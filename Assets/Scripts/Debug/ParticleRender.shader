// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/SimpleParticleRender"
{
	CGINCLUDE
	#include "UnityCG.cginc"

	struct ParticleData
	{
		float3 velocity;
		float3 position;
		float  age;
	};
	
	struct v2g
	{
		float3 position : TEXCOORD0;
		float4 color    : COLOR;
	};
	
	struct g2f
	{
		float4 position : POSITION;
		float2 texcoord : TEXCOORD0;
		float4 color    : COLOR;
	};

	
	StructuredBuffer<ParticleData> _ParticleDataBuffer;
	
	sampler2D _MainTex;
	float4    _MainTex_ST;
	
	float     _ParticleSize;
	float4    _Color;
	
	float4x4  _InvViewMatrix;
	
	static const float3 g_positions[4] =
	{
		float3(-1, 1, 0),
		float3(1, 1, 0),
		float3(-1,-1, 0),
		float3(1,-1, 0),
	};
	
	static const float2 g_texcoords[4] =
	{
		float2(0, 0),
		float2(1, 0),
		float2(0, 1),
		float2(1, 1),
	};

	// --------------------------------------------------------------------
	// Vertex Shader
	// --------------------------------------------------------------------
	v2g vert(uint id : SV_VertexID)
	{
		v2g o = (v2g)0;
		
		o.position = UnityObjectToViewPos(_ParticleDataBuffer[id].position);
		//o.color = float4(0.5 + 0.5 * normalize(_ParticleDataBuffer[id].velocity), 1.0);
		o.color = _Color;
		return o;
	}

	// --------------------------------------------------------------------
	// Geometry Shader
	// --------------------------------------------------------------------
	[maxvertexcount(4)]
	void geom(point v2g In[1], inout TriangleStream<g2f> SpriteStream)
	{
		g2f o = (g2f)0;
		[unroll]
		for (int i = 0; i < 4; i++)
		{
			float3 position = g_positions[i] * _ParticleSize + In[0].position;
			o.position =  mul(UNITY_MATRIX_P, float4(position,1));

			o.color = In[0].color;
			o.texcoord = g_texcoords[i];
			
			SpriteStream.Append(o);
		}
		
		SpriteStream.RestartStrip();
	}

	// --------------------------------------------------------------------
	// Fragment Shader
	// --------------------------------------------------------------------
	fixed4 frag(g2f i) : SV_Target
	{
		return tex2D(_MainTex, i.texcoord.xy) * i.color;
	}
		ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 100

			ZWrite Off
			Blend One One

			Pass
		{
			CGPROGRAM
#pragma target   5.0
#pragma vertex   vert
#pragma geometry geom
#pragma fragment frag
			ENDCG
		}
	}
}