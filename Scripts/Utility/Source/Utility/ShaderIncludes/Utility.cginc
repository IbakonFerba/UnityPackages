/*
*   This include file contains useful functions that might be used in several shaders
*   
*   v1.2 08/2018
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

/*  remaps the provided value to the new range
*
*   value - value to remap
*   lowOrig - lowest value of original range
*   highOrig - highest value of original range
*   lowNew - lowest value of new range
*   highNes - highest value of new range
*/
float remapFloat(float value, float lowOrig, float highOrig, float lowNew, float highNew) {
    return lowNew + (value-lowOrig)*(highNew-lowNew)/(highOrig-lowOrig);
}

/*  remaps the provided value to the new range
*
*   value - value to remap
*   lowOrig - lowest value of original range
*   highOrig - highest value of original range
*   lowNew - lowest value of new range
*   highNes - highest value of new range
*/
half remapHalf(half value, half lowOrig, half highOrig, half lowNew, half highNew) {
    return lowNew + (value-lowOrig)*(highNew-lowNew)/(highOrig-lowOrig);
}

/*  remaps the provided value to the new range
*
*   value - value to remap
*   lowOrig - lowest value of original range
*   highOrig - highest value of original range
*   lowNew - lowest value of new range
*   highNes - highest value of new range
*/
half remapFixed(fixed value, fixed lowOrig, fixed highOrig, fixed lowNew, fixed highNew) {
    return lowNew + (value-lowOrig)*(highNew-lowNew)/(highOrig-lowOrig);
}

/*  returns a color that is the result of a triplanar mapping
*
*   texX - The texture that is used for the plane perpendicular to the X axis
*   texY - The texture that is used for the plane perpendicular to the Y axis
*   texZ - The texture that is used for the plane perpendicular to the Z axis
*   pos - position of the fragment
*   normal - normal of the fragment
*   scale - scaling factor for the maps
*/
fixed4 triplanarTex(sampler2D texX, sampler2D texY, sampler2D texZ, float3 pos, float3 normal, float scale) {
    // Blending factor of triplanar mapping
    half3 bf = normalize(abs(normal));
    bf /= dot(bf, (float3)1);
    
    // Triplanar mapping
    half2 tx = pos.yz * scale;
    half2 ty = pos.zx * scale;
    half2 tz = pos.xy * scale;  
     
    // Base colors
    half4 cx = tex2D(texX, tx) * bf.x;
    half4 cy = tex2D(texY, ty) * bf.y;
    half4 cz = tex2D(texZ, tz) * bf.z;
    
    return cx+cy+cz;
}

/*  returns a color that is the result of a triplanar mapping
*
*   texX - The texture that is used for the plane perpendicular to the X axis
*   texX_ST - Scale and Translate of texX
*   texY - The texture that is used for the plane perpendicular to the Y axis
*   texY_ST - Scale and Translate of texY
*   texZ - The texture that is used for the plane perpendicular to the Z axis
*   texZ_ST - Scale and Translate of texZ
*   pos - Position of the fragment
*   normal - Normal of the fragment
*/
fixed4 triplanarTex(sampler2D texX, float4 texX_ST, sampler2D texY, float4 texY_ST, sampler2D texZ,float4 texZ_ST, float3 pos, float3 normal) {
    // Blending factor of triplanar mapping
    half3 bf = normalize(abs(normal));
    bf /= dot(bf, (float3)1);
    
    // Triplanar mapping
    half2 tx = pos.yz * texX_ST.xy + texX_ST.zw;
    half2 ty = pos.zx * texY_ST.xy + texY_ST.zw;
    half2 tz = pos.xy * texZ_ST.xy + texZ_ST.zw;  
     
    // Base colors
    half4 cx = tex2D(texX, tx) * bf.x;
    half4 cy = tex2D(texY, ty) * bf.y;
    half4 cz = tex2D(texZ, tz) * bf.z;
    
    return cx+cy+cz;
}

/*  returns a color that is the result of a triplanar mapping
*
*   texX - The texture that is used for all planes
*   tex_ST - Scale and Translate of tex
*   pos - Position of the fragment
*   normal - Normal of the fragment
*/
fixed4 triplanarTex(sampler2D tex, float4 tex_ST,  float3 pos, float3 normal) {
    return triplanarTex(tex, tex_ST, tex, tex_ST, tex, tex_ST, pos, normal);
}

#endif