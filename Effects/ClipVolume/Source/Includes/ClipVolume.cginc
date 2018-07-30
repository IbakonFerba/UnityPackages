/*
*   This Include File contains Functions that are used for all Clip Volume shaders
*   
*   v1.0 07/2018
*   Written by Fabian Kober
*   fabian-kober@gmx.net
*/
#ifndef CLIP_VOLUME_CGINC
#define CLIP_VOLUME_CGINC

#include "Assets/Shaders/Includes/Utility.cginc"

/*  Discards all fragments outside the defined clip volume
*   
*   worldPos - World Position of the fragment
*   clipVolumeWorldToLocal - matrix from world space to the clip volume local space
*   clipVolumeWorldPos - world position of the center of the clip volume
*   clipVolumeMin - min coordinates of the clip volume relative to its center
*   clipVolumeMax - max coordinates of the clip volume relative to its center
*/
float useClipVolume(float3 worldPos, float4x4 clipVolumeWorldToLocal, float3 clipVolumeWorldPos, float3 clipVolumeMin, float3 clipVolumeMax) {
    // calculate whether this fragment is inside the volume or outside the volume
    float3 volumeSpacePosition = mul(clipVolumeWorldToLocal, worldPos);
    float3 clipVolumeLocalPos = mul(clipVolumeWorldToLocal, clipVolumeWorldPos);
    float3 offset = (volumeSpacePosition) - (clipVolumeLocalPos+clipVolumeMax);
    float outOfBounds = max(offset.x, offset.y);
    outOfBounds = max(outOfBounds, offset.z);
    offset = (clipVolumeLocalPos+clipVolumeMin) - (volumeSpacePosition);
    outOfBounds = max(outOfBounds, max(offset.x, offset.y));
    outOfBounds = max(outOfBounds, offset.z);
    clip(-outOfBounds);
    return outOfBounds;
}

/*  Fades towards the border of the Clip Volume using dithering via a triplanar mapped noise map
*   
*   outOfBounds - the out of bounds value of the fragment from the useClipVolume Function
*   noiseMap - noise map to use for dithering
*   noiseScale - scaling factor for the noise map
*   threshold - threshold for determining which pixels to discard at which color of the noise map
*   gradientHardness - How fast should we fade?
*   worldPos - world Position of the fragment
*   worldNormal - world Normal of the fragment
*/
void fadeBorder(float outOfBounds, sampler2D noiseMap, float noiseScale, float threshold, float gradientHardness, float3 worldPos, float3 worldNormal) {
    // fade the border using a triplanar mapped noise map for dithering
    float border = 1-saturate(-outOfBounds*gradientHardness);
    half noise = triplanarTex(noiseMap, noiseMap, noiseMap, worldPos, worldNormal, noiseScale)*border;
    clip(threshold-noise.r);
}

#endif
