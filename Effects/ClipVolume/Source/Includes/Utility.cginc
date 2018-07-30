/*
*   This include file contains useful functions that might be used in several shaders
*   
*   v1.0 07/2018
*   Written by Fabian Kober
*   fabian-kober@gmx.net
*/
#ifndef UTILITY_CGINC
#define UTILITY_CGINC

/*  returns 0 if the fragment is not on a backface and 1 if it is
*   
*   normal - normal vector of the fragment
*   viewDir - view Direction of the camera
*/
int backface(float3 normal, float3 viewDir) {
   return dot(normal, viewDir) < 0;
}

/*  returns a color that is the result of a triplanar mapping
*
*   texX - The texture that is used for the plane perpendicular to the X axis
*   texY - The texture that is used for the plane perpendicular to the Y axis
*   texZ - The texture that is used for the plane perpendicular to the Z axis
*   worldPos - World Position of the fragment
*   worldNormal - World Normal of the fragment
*   scale - scaling factor for the maps
*/
fixed4 triplanarTex(sampler2D texX, sampler2D texY, sampler2D texZ, float3 worldPos, float3 worldNormal, float scale) {
    // Blending factor of triplanar mapping
    half3 bf = normalize(abs(worldNormal));
    bf /= dot(bf, (float3)1);
    
    // Triplanar mapping
    half2 tx = worldPos.yz * scale;
    half2 ty = worldPos.zx * scale;
    half2 tz = worldPos.xy * scale;  
     
    // Base colors
    half4 cx = tex2D(texX, tx) * bf.x;
    half4 cy = tex2D(texY, ty) * bf.y;
    half4 cz = tex2D(texZ, tz) * bf.z;
    
    return cx+cy+cz;
}

#endif
