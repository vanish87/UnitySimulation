// #pragma kernel ObjectToGridIndex
// #pragma kernel ClearGridIndex
// #pragma kernel BuildGridIndex
// #pragma kernel BuildSortedObject

#include "GridHelper.hlsl"
#include "DispatcherHelper.hlsl"

StructuredBuffer<DataType> _ObjectBufferRead;
int	_ObjectBufferReadCount;
RWStructuredBuffer<DataType> _ObjectBufferSorted;
RWStructuredBuffer<uint2> _ObjectGridIndexBuffer;
// RWStructuredBuffer<uint2> _GridBuffer;

[numthreads(SIMULATION_BLOCK_SIZE, 1, 1)]
void ObjectToGridIndex(uint3 DTid : SV_DispatchThreadID) 
{
	RETURN_IF_INVALID(DTid);

	const unsigned int P_ID = DTid.x;	// Particle ID to operate on

	float3 position = _ObjectBufferRead[P_ID].Position();
	_ObjectGridIndexBuffer[P_ID] = PosToGridIndexPair(P_ID, position, _GridMin, _GridMax, _GridSize, _GridSpacing);
}

[numthreads(GRID_BLOCK_SIZE, GRID_BLOCK_SIZE, GRID_BLOCK_SIZE)]
void ClearGridIndex(uint3 DTid : SV_DispatchThreadID) 
{
	RETURN_IF_INVALID(DTid);

	uint C_ID = CellIndexToCellID(DTid, _GridSize);
	_GridBuffer[C_ID] = uint2(0, 0);
}

[numthreads(SIMULATION_BLOCK_SIZE, 1, 1)]
void BuildGridIndex(uint3 DTid : SV_DispatchThreadID) 
{
	RETURN_IF_INVALID(DTid);

	const unsigned int G_ID = DTid.x;	// Grid Data ID to operate on
	unsigned int       G_ID_PREV = (G_ID == 0) ? (uint)_ObjectBufferReadCount : G_ID; G_ID_PREV--;
	unsigned int       G_ID_NEXT = G_ID + 1; if (G_ID_NEXT == (uint)_ObjectBufferReadCount) { G_ID_NEXT = 0; }

	unsigned int cell = _ObjectGridIndexBuffer[G_ID].x;
	unsigned int cell_prev = _ObjectGridIndexBuffer[G_ID_PREV].x;
	unsigned int cell_next = _ObjectGridIndexBuffer[G_ID_NEXT].x;

	if (cell != cell_prev) {
		// I'm the start of a cell
		_GridBuffer[cell].x = G_ID;
	}

	if (cell != cell_next) {
		// I'm the end of a cell
		_GridBuffer[cell].y = G_ID + 1;
	}
}

[numthreads(SIMULATION_BLOCK_SIZE, 1, 1)]
void BuildSortedObject(uint3 DTid : SV_DispatchThreadID)
{
	RETURN_IF_INVALID(DTid);
	
	const unsigned int ID = DTid.x; // Grid Data ID to operate on
	const unsigned int O_ID = _ObjectGridIndexBuffer[ID].y;
	_ObjectBufferSorted[ID] = _ObjectBufferRead[O_ID];
}
