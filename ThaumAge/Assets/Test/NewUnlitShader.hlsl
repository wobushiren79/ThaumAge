#pragma shader_feature _ CULL_OFF
#pragma shader_feature _ CULL_FRONT
#pragma shader_feature _ CULL_BACK

float4 Cull(float3 viewDir, float3 normal, float4 cullMode)
{
    if (cullMode.x > 0) // CULL_OFF
        return 1;

    float dotValue = dot(viewDir, normal);

    if ((cullMode.y > 0 && dotValue < 0) || (cullMode.z > 0 && dotValue > 0))
        return 0;

    return 1;
}