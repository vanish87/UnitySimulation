
static const float2 _ArrowTileSize       = float2(0.05, 0.05);
static const float  _ArrowShaftThickness = 0.0015;
static const float  _ArrowHeadLength     = 0.0075;
static const float  _ArrowHeadAngle      = 0.75;
static const float4 _ArrowColor          = float4(1.0, 1.0, 1.0, 1.0);

float2 ArrowTileCenterCoord(float2 uv)
{
    return (floor(uv / _ArrowTileSize) + 0.5) * _ArrowTileSize.xy;
}

float Arrow(float2 pos, float2 dir)
{
    float2 p = pos;
    float2 v = dir;

    p -= ArrowTileCenterCoord(p);

    float magV = length(dir);
    float magP = length(p);

    if (magV > 0.0)
    {
        float2 dirP = p / magP;
        float2 dirV = v / magV;

        magV = clamp(magV, 0.001, _ArrowTileSize * 0.5);

        v = dirV * magV;

        float dist = 0.0;
        dist = max
        (
            // Shaft
            _ArrowShaftThickness * 0.25 -
            max(abs(dot(p, float2(dirV.y, -dirV.x))), // Width
                abs(dot(p, dirV)) - magV + _ArrowHeadLength * 0.5), // Length

            // Arrow head
            min(0.0, dot(v - p, dirV) - cos(_ArrowHeadAngle * 0.5) * length(v - p)) * 2.0 + // Front sides
            min(0.0, dot(p, dirV) + _ArrowHeadLength - magV) //Back
        );

        return clamp(1.0 + dist * 512.0, 0.0, 1.0);
    }
    else
    {
        return max(0.0, 1.2 - magP);
    }
    return 0;
}