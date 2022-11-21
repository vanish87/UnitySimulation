
struct BoundaryParticle
{
    uint bid;
    float3 localPos;

    inline float3 Position(){return localPos;}
};
