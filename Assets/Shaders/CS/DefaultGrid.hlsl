
struct Cell
{
	uint2 index; //Particle start/end index
	inline uint2 Index(){return index;}
	inline void SetIndex(uint2 new_index){index = new_index;}
	inline void SetIndexX(uint new_index){index.x = new_index;}
	inline void SetIndexY(uint new_index){index.y = new_index;}
	
};

RWStructuredBuffer<Cell> _GridBuffer;