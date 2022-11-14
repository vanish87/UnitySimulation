
uint _DispatchedX;
uint _DispatchedY;
uint _DispatchedZ;

#define RETURN_IF_INVALID(TID) if(any(TID >= uint3(_DispatchedX, _DispatchedY,_DispatchedZ))) return;
// #define RETURN_IF_INVALID(TID) if(TID.x >= _DispatchedX) return;