
#define SetBit(target, shift)   target = target | (1<<shift)
#define ClearBit(target, shift) target = target & (~(1<<shift))

#define SetValue(target, val, mask,  shift) target = (target & ~mask) | ((val << shift) & mask);

#define ParticleDataType float3

//try to align data with 16-bytes stride
//portential perfomance improvement

//2^20 = 1024x1024k
//2^12 = 4096

//2^4 = 16
static const uint ParticleTypeMask   		= 0x00000007;
static const uint ParticleTypeMaskShift 	= 0;

static const uint ParticleActiveMask 		= 0x00000008;
static const uint ParticleActiveMaskShift 	= 3;

static const uint ParticleUUIDMask 			= 0x000fffff;
static const uint ParticleUUIDShift 		= 0;

static const uint BoundaryMask 				= 0x00fffff0;
static const uint BoundaryMaskShift 		= 4;

static const uint MaxParticleNum 			= 0x000fffff;
static const uint MaxParticleType 			= 0x00000004;

static const uint PT_NONE 		= 0;
static const uint PT_FLUID 		= 1;
static const uint PT_ELASTIC    = 2;
static const uint PT_SNOW    	= 3;

struct Particle 
{
	/*
	|0000 0000 0000 |0000 0000 0000	0000 0000	|
	|12-bits  		|20-bits					|
	|not used		|uuid id					|
	|0xfff00000		|0x000fffff					|
	*/
	uint uuid; //20-bits uuid
	/*
	|0000 0000	|0000 0000 0000 0000 0000	|0      	|000		|
	|8-bits   	|20-bits					|1-bit  	|3-bits		|
	|not used 	|boundary id				|is active 	|type		|
	|0xff000000	|0x00fffff0					|0x00000008	|0x00000007	|
	*/
	uint type;  // 20 bits boundary id + 1 bit active + 3 bits type  

	float mass;
	float volume;
	ParticleDataType position;
	ParticleDataType velocity;

	float3x3 C;
    float3x3 Fe;
    float Jp;
	// #if define(USE_2D_KERNEL)
	// uint3 padding; //pading to 16-bytes for 2D
	// #endif

	inline bool IsActive()
	{
		return (type & ParticleActiveMask) >> ParticleActiveMaskShift;
	}
	inline void SetActive(bool v)
	{
		if(v) SetBit(type, ParticleActiveMaskShift);
		else ClearBit(type, ParticleActiveMaskShift);
	}
	inline uint Type()
	{
		return (type & ParticleTypeMask) >> ParticleTypeMaskShift;
	}
	inline void SetType(uint t)
	{
		type = type | ((t & ParticleTypeMask)<<ParticleTypeMaskShift);
	}
	inline bool IsFluid()
	{
		return Type() == PT_FLUID;
	}
	inline bool IsBoundary()
	{
		return false;
	}
	inline uint UUID()
	{
		return (uuid & ParticleUUIDMask) >> ParticleUUIDShift;
	}
	inline void SetUUID(uint uuid_new)
	{
		uuid = (uuid & ~ParticleUUIDMask) | ((uuid_new << ParticleUUIDShift) & ParticleUUIDMask);
	}
	inline uint BID()
	{
		return (type & BoundaryMask) >> BoundaryMaskShift;
	}
	inline void SetBID(uint bid_new)
	{
		type = (type & ~BoundaryMask) | ((bid_new << BoundaryMaskShift) & BoundaryMask);
	}
	inline ParticleDataType Position()
	{
		return position;
	}

	// inline Reset(int uuid = -1, float4x4 localToWorld = 0)
	// {

	// }

	
};