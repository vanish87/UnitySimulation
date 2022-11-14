// #pragma kernel BitonicSort
// #pragma kernel MatrixTranspose

#define BITONIC_BLOCK_SIZE 1024
#define TRANSPOSE_BLOCK_SIZE 32

cbuffer cb
{
	uint _Level;
	uint _LevelMask;
	uint _Width;
	uint _Height;
};

StructuredBuffer  <DataType> Input : register(t0);
RWStructuredBuffer<DataType> Data  : register(u0);

bool Compare(DataType left, DataType right) {
	return COMPARE(left, right);
}

groupshared DataType shared_data[BITONIC_BLOCK_SIZE];
[numthreads(BITONIC_BLOCK_SIZE, 1, 1)]
void BitonicSort(uint3 Gid  : SV_GroupID, uint3 DTid : SV_DispatchThreadID, uint3 GTid : SV_GroupThreadID, uint  GI : SV_GroupIndex) {
	// Load shared data
	shared_data[GI] = Data[DTid.x];
	GroupMemoryBarrierWithGroupSync();

	// Sort the shared data
	for (uint j = _Level >> 1; j > 0; j >>= 1) {
		DataType result = (Compare(shared_data[GI & ~j], shared_data[GI | j]) == (bool)(_LevelMask & DTid.x)) ? shared_data[GI ^ j] : shared_data[GI];
		GroupMemoryBarrierWithGroupSync();
		shared_data[GI] = result;
		GroupMemoryBarrierWithGroupSync();
	}

	// Store shared data
	Data[DTid.x] = shared_data[GI];
}


groupshared DataType transpose_shared_data[TRANSPOSE_BLOCK_SIZE * TRANSPOSE_BLOCK_SIZE];
[numthreads(TRANSPOSE_BLOCK_SIZE, TRANSPOSE_BLOCK_SIZE, 1)]
void MatrixTranspose(uint3 Gid  : SV_GroupID, uint3 DTid : SV_DispatchThreadID, uint3 GTid : SV_GroupThreadID, uint  GI : SV_GroupIndex) {
	transpose_shared_data[GI] = Input[DTid.y * _Width + DTid.x];
	GroupMemoryBarrierWithGroupSync();
	uint2 XY = DTid.yx - GTid.yx + GTid.xy;
	Data[XY.y * _Height + XY.x] = transpose_shared_data[GTid.x * TRANSPOSE_BLOCK_SIZE + GTid.y];
}