Shader "Unlit/MyParticleShader"
{
	CGINCLUDE
	#include "UnityCG.cginc"
	#include "CS/DefaultParticle.hlsl"

	struct v2g
	{
		float3 position : TEXCOORD0;
		float4 col : TEXCOORD1;
		float  size : TEXCOORD2;
	};
	struct g2f
	{
		float4 position : POSITION;
		float2 texcoord : TEXCOORD0;
		float4 col : TEXCOORD1;
	};

	struct gin
	{
		float3 pos;
		float size;
		float4 col;
	};

	#define VertexIn gin
	#define VertexOut g2f
	void UpdateVertex(in VertexIn vin, inout VertexOut o)
	{
		o.position =  mul(UNITY_MATRIX_P, o.position);
		// o.position =  UnityObjectToClipPos(o.position);
		o.col = vin.col;
	}

	#include "GeometryQuad.cginc"

	StructuredBuffer<Particle> _ParticleBuffer;
	sampler2D _ParticleTex;

	// --------------------------------------------------------------------
	// Vertex Shader
	// --------------------------------------------------------------------
	v2g vert(uint id : SV_VertexID) // SV_VertexID:
	{

		v2g o = (v2g)0;
		Particle p = _ParticleBuffer[id];
		// bool shouldRender = (p.type != PT_INACTIVE);
		bool shouldRender = p.IsActive();

		o.position = UnityObjectToViewPos(p.pos);
		o.col = shouldRender?saturate(p.col):float4(0.5,0,0,1);
		o.size =  shouldRender?0.1:0.05;
		o.size *= shouldRender;
		return o;
	}

	// --------------------------------------------------------------------
	// Geometry Shader
	// --------------------------------------------------------------------
	[maxvertexcount(QuadVertex)]
	void geom(point v2g p[1], inout TriangleStream<g2f> outStream)
	{
		float size = p[0].size;
		if(size > 0)
		{
			float3 pos = p[0].position;
			float4 col = p[0].col;
			VertexIn vin;
			vin.pos = pos;
			vin.col = col;
			vin.size = size;
			AddQuad(vin, outStream);
		}
	}

	// --------------------------------------------------------------------
	// Fragment Shader
	// --------------------------------------------------------------------
	float4 frag(g2f i) : SV_Target
	{
		return i.col;
	}
	ENDCG

	SubShader
	{
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
