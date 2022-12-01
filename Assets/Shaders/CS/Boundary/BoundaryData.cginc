
const static int BT_DISABLED = 0;
const static int BT_SDF_SPHERE = 1;
const static int BT_SDF_CUBE = 2;
const static int BT_SDF_FIELD = 3;
const static int BT_PARTICLE = 4;
// const static int BT_PARTICLE_SPHERE = 4;
// const static int BT_PARTICLE_MESH = 5;

struct Boundary
{
    int uuid;
	int type;
	float4x4 localToWorld;
	float4x4 worldToLocal;
	float4 parameter;

	inline bool IsActive()
	{
		return type != BT_DISABLED;
	}
};

struct BoundaryParticle
{
    int bid;
    float3 localPos;
    float3 worldPos;

    inline float3 Position(){return worldPos;}
    inline bool IsActive(){return bid != -1;}
};



StructuredBuffer<Boundary> _BoundaryBuffer;
int _BoundaryBufferCount;

Texture2D<float4> _BoundaryTexture;
float2 _BoundaryTextureSize;

RWStructuredBuffer<BoundaryParticle> _BoundaryParticleBuffer;
StructuredBuffer<uint2> _BoundaryGridBuffer;

int FindIndexByUUID(int uuid)
{
    if(uuid < 0) return -1;

    for(int bid = 0; bid < _BoundaryBufferCount; ++bid)
    {
        if(_BoundaryBuffer[bid].uuid == uuid) return bid;
    }    
    return -1;
}