
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
	float3 velocity;

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
    inline float3 LocalPosition(){return worldPos;}
    inline int BoundaryID(){return bid;}
    inline bool IsActive(){return bid != -1;}
};

StructuredBuffer<Boundary> _BoundaryBuffer;
int _BoundaryBufferCount;

Texture2D<float4> _BoundaryTexture;
float2 _BoundaryTextureSize;

RWStructuredBuffer<BoundaryParticle> _BoundaryParticleBuffer;
struct BoundaryCell
{
	uint2 index; //Particle start/end index
	inline uint2 Index(){return index;}
	inline void SetIndex(uint2 new_index){index = new_index;}
	inline void SetIndexX(uint new_index){index.x = new_index;}
	inline void SetIndexY(uint new_index){index.y = new_index;}
	
};
StructuredBuffer<BoundaryCell> _BoundaryGridBuffer;

float3 _BoundarySpaceMin;
float3 _BoundarySpaceMax;

int FindIndexByUUID(int uuid)
{
    if(uuid < 0) return -1;

    for(int bid = 0; bid < _BoundaryBufferCount; ++bid)
    {
        if(_BoundaryBuffer[bid].uuid == uuid) return bid;
    }    
    return -1;
}