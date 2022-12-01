
float3 OnSDFBoundary(float3 pos, in Boundary b)
{
    float3 force = 0;
	switch(b.type)
	{
		case BT_SDF_SPHERE:
		{
			float r = GetScale(b.localToWorld).x;
			float3 c = GetPos(b.localToWorld);
			float dist = distance(pos,c);
			if(dist < r)
			{
				float4 p = b.parameter;
				force = normalize(pos-c) * pow(p.x, p.y+(r-dist));
			}
		}
		break;
        case BT_SDF_CUBE:
        {
			float3 p = pos;
            float3 bp = GetPos(b.localToWorld);
            float3 q = abs(p) - bp;
            float3 dir = normalize(p-bp);

            float sdf = length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);

            //not implemented
            // force =  sdf * dir;
        }
        break;
		case BT_SDF_FIELD:
		{
			float3 uv = WorldPosToUV(pos, b.worldToLocal);
			if(all(0 < uv) && all(uv < 1))
			{ 
				int2 tuv = UV_LocalToGlobal(uv.xy, b.parameter , _BoundaryTextureSize);
				float3 col = _BoundaryTexture[tuv].rgb;
				force = RGBToNormal(col);
			}
		}
		break;
		case BT_DISABLED: break;
		default: break;
	}
	return force;

}

float3 GetBoundaryForce(float3 pos)
{
    float3 total = 0;
	for(int i = 0; i < _BoundaryBufferCount; ++i)
	{
		Boundary b = _BoundaryBuffer[i];
		total += OnSDFBoundary(pos, b);
	}
	return total;

}