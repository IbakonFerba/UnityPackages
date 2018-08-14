/*
* This include file contains signed distance functions from http://iquilezles.org/www/articles/distfunctions/distfunctions.htm used for raymarching
*   
* v1.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
#ifndef SDF_CGING
#define SDF_CGING

float sdBox(float3 p, float3 bounds)
{
    float3 d = abs(p) - bounds;
    return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

float sdSphere(float3 p, float rad )
{
    return length(p)-rad;
}

float sdCapsule( float3 p, float3 a, float3 b, float r )
{
    float3 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

#endif
