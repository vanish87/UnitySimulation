float3 _GridMin;
float3 _GridMax;
float3 _GridSize;
float3 _GridSpacing;

static const int3 _GridRange = int3(1,1,1);
RWStructuredBuffer<Cell> _GridBuffer;

#define FOR_EACH_NEIGHBOR_START(POS, NID, GRID) {\
int3 cid = PosToCellIndex(POS, _GridMin, _GridMax, _GridSpacing); \
for(int i = max(cid.x - _GridRange.x, 0); i <= min(cid.x + _GridRange.x, _GridSize.x-1); ++i)\
for(int j = max(cid.y - _GridRange.y, 0); j <= min(cid.y + _GridRange.y, _GridSize.y-1); ++j)\
for(int k = max(cid.z - _GridRange.z, 0); k <= min(cid.z + _GridRange.z, _GridSize.z-1); ++k){\
	uint2 startEnd = GRID[CellIndexToCellID(int3(i,j,k), _GridSize)].Index(); \
	for(uint NID = startEnd.x; NID < startEnd.y; ++NID){\

#define FOR_EACH_NEIGHBOR_END }}}


#define FOR_EACH_PARTICLE_IN_CELL_START(CELL, PID, GRID) {\
uint2 startEnd = GRID[CellIndexToCellID(int3(CELL.x,CELL.y,CELL.z), _GridSize)].Index(); \
for(uint PID = startEnd.x; PID < startEnd.y; ++PID){\

#define FOR_EACH_PARTICLE_IN_CELL_END }}