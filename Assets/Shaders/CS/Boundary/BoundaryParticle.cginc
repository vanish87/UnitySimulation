
struct BoundaryParticle
{
    uint bid;
    float3 localPos;
    float3 worldPos;

    inline float3 Position(){return worldPos;}
};
