/*
*   This include file contains functions for the magic light shaders
*   
*   v2.0 09/2018
*   Written by Fabian Kober
*   fabian-kober@gmx.net
*/
#ifndef MAGIC_LIGHT_FUNCTIONS_CGINC
#define MAGIC_LIGHT_FUNCTIONS_CGINC

#include "Assets/Shaders/Includes/Utility.cginc"

/*  returns a modified distance based on the angle of the point (animated over time)
*
*   distance - The distance of the fragment to the center of the circle
*   angle - The angle of the vector between the center of the circle and the fragment to the up vector of the circle
*   time - Pass through for unity's _Time
*   wobbleParams - parameters for the wobble
*       x - Speed
*       y - Strength
*/
float animatedDistance(float distance, float angle, float4 time, float2 wobbleParams) {
    return distance+cos(sin(angle+time.y*wobbleParams.x))*wobbleParams.y;
}

/*  clips the fragment if it is inside the cylinder of the magic light and returns the out of bounds value (negative if inside the circle, positive if outside)
*
*   worldPos - World Position of the fragment
*   lightDir - Foreward Vector of the Magic light
*   lightPos - World Position of the Magic light
*   lightRad - Radius of the Magic Light cylinder
*   wobbleParams - parameters for the wobble
*       x - Speed
*       y - Strength
*   time - Pass through for unity's _Time
*   inverted - should the cutout effect be inverted?
*/
float clipCircle(float3 worldPos, float3 lightDir, float3 lightPos, float lightRad, float2 wobbleParams, float4 time, int inverted) {
    // project the fragment onto the plane of the magic light origin
	float3 projected = worldPos - dot(lightDir, worldPos - lightPos) * lightDir;
	
	// calculate the distance between the projected fragment and the light origin
	float distance = length(projected - lightPos);
	
	// calculate the angle of the vector between the center of the circle and the fragment to the up vector of the circle
	float angle = acos(dot(normalize(projected - lightPos), float3(0.0,1.0,0.0)));
	
	// calculate the out of bounds value by testing if the fragment is inside the radius
	float animatedDist = animatedDistance(distance, angle, time, wobbleParams);
	float outOfBounds = inverted ? lightRad-animatedDist : animatedDist-lightRad;
	
	// clip if out of bounds is negative
	clip(outOfBounds);
	
	return outOfBounds;
}



/*  returns an animated offset for the uv coordinates when sampling the stencil
*
*   coord - The uv coordinate of the fragment for the stencil
*   time - Pass through for unity's _Time
*   wobbleParams - parameters for the wobble
*       x - Speed
*       y - Strength
*/
float2 animatedOffset(float2 coord, float4 time, float2 wobbleParams) {
    return float2(cos(time.y*wobbleParams.x+coord.x)*wobbleParams.y*0.1, sin(time.y*wobbleParams.x+coord.y)*wobbleParams.y*0.1);
}

/*  clips the fragment based onthe red channel of a stencil map and returns the rgb value of the stencil map for the fragment
*
*   stencilMap - The stencil map to use
*   stencilMap_ST - Scale and Translate of the stencilMap
*   worldPos - World Position of the fragment
*   lightDir - Foreward Vector of the Magic light
*   lightPos - World Position of the Magic light
*   wobbleParams - parameters for the wobble
*       x - Speed
*       y - Strength
*   time - Pass through for unity's _Time
*/
float3 clipStencil(sampler2D stencilMap, float4 stencilMap_ST, float3 worldPos, float3 lightDir, float3 lightPos, float2 wobbleParams, float4 time) {    
    // project the fragment onto the plane of the magic light origin
	float3 projected = worldPos - dot(lightDir, worldPos - lightPos) * lightDir;
	
	// calculate the distance between the projected fragment and the light origin
	float2 diff = projected - lightPos;
	float distance = length(diff);
	
	// get an offset
	float2 offset = animatedOffset(diff, time, wobbleParams);
	
	// sample the stencil texture centered around the lightPos
	fixed4 stencil = tex2D(stencilMap, (diff+offset)*stencilMap_ST.xy+stencilMap_ST.zw+float2(0.5,0.5));

	// clip if red channel of the stencil is bigger than 0.5
	clip(0.5-stencil.r);
	
	return stencil.rgb;
}


/*  returns a color for the finalColor function that is a blend of the two border types and the base color
*
*   color - The base color to build on to
*   outOfBounds - The out of bounds value of the fragment
*   tex - texture for the border that is applied via triplanar mapping
*   tex_ST - Scale and Transform for the border texture
*   localPos - Local Position of the fragment
*   localNormal - Local Normal of the fragment
*   borderThresholds - Thresholds for teh borders
*       x - Threshold for border 1
*       y - Threshold for border 2
*   border1Color - Color for border 1
*   border2Color - Color for border 2
*   useTexture - Defines which borders should use the texture
*       x > 0 - border 1 uses the texture, else it does not
*       y > 0 - border 2 uses the texture, else it does not
*   mode - the border modes. 
*       x > 0 - hard border is rendered here (else it is in emission)
*       y > 0 - soft border is rendered additive, else alpha blended
*       z > 0 - hard border is rendered additive, else alpha blended
*       w > 0 - hard border is rendered on top of soft border, else the other way around
*/
fixed4 drawBorder(fixed4 color, float outOfBounds, sampler2D tex, float4 tex_ST, float3 localPos, float3 localNormal, float2 borderThresholds, fixed4 border1Color, fixed4 border2Color, half2 useTexture, half4 mode) {
    // save the color and get the texture color
    fixed4 c = color;
    fixed4 texCol = triplanarTex(tex, tex_ST, tex,tex_ST, tex,tex_ST, localPos, localNormal);
	
	// calculate border1 color
	float border1Alpha = saturate(border1Color.a-(outOfBounds/borderThresholds.x));
	fixed4 border1Col = useTexture.x ? texCol*border1Color : border1Color;
	
	// calculate border2 color
	fixed4 border2Col = useTexture.y ? border2Color*texCol : border2Color;
	
	// blend everything together
	if(mode.x) {
	    if(mode.w) {
	        // border1
	        if(mode.y)
	            c = c + border1Alpha*border1Col;
	        else
	            c = border1Alpha*border1Col + (1-border1Alpha)*c;
	            
	        // border2
	        if(outOfBounds <= borderThresholds.y) {
	            if(mode.z) 
	                c = c + border2Col.a*border2Col;
	            else
	                c = border2Col.a*border2Col + (1-border2Col.a)*c;
	        }
	    } else {
	        // border2
	        if(outOfBounds <= borderThresholds.y) {
	            if(mode.z) 
	                c = c + border2Col.a*border2Col;
	            else
	                c = border2Col.a*border2Col + (1-border2Col.a)*c;
	            
	        }
	        
	        // border1
	        if(mode.y)
	            c = c + border1Alpha*border1Col;
	        else
	            c = border1Alpha*border1Col + (1-border1Alpha)*c;
	    }
	} else {
	    // border1
	    if(mode.y)
	        c = c + border1Alpha*border1Col;
	    else
	        c = border1Alpha*border1Col + (1-border1Alpha)*c;
	}
	
	// preserve alpha
	c.a = color.a;
	return c;
}

/*  returns a color for the finalColor function that is a blend of the two border types and the base color
*
*   color - The base color to build on to
*   stencil - The stencil values for the fragment
*       r - cutout, not used here
*       g - border1 factor
*       b - border2 factor
*   tex - texture for the border that is applied via triplanar mapping
*   tex_ST - Scale and Transform for the border texture
*   localPos - Local Position of the fragment
*   localNormal - Local Normal of the fragment
*   border1Color - Color for border 1
*   border2Color - Color for border 2
*   useTexture - Defines which borders should use the texture
*       x > 0 - border 1 uses the texture, else it does not
*       y > 0 - border 2 uses the texture, else it does not
*   mode - the border modes. 
*       x > 0 - hard border is rendered here (else it is in emission)
*       y > 0 - soft border is rendered additive, else alpha blended
*       z > 0 - hard border is rendered additive, else alpha blended
*       w > 0 - hard border is rendered on top of soft border, else the other way around
*/
fixed4 drawBorder(fixed4 color, float3 stencil, sampler2D tex, float4 tex_ST, float3 localPos, float3 localNormal, fixed4 border1Color, fixed4 border2Color, half2 useTexture, half4 mode) {
    // save the color and get the texture color
    fixed4 c = color;
    fixed4 texCol = triplanarTex(tex, tex_ST, tex,tex_ST, tex,tex_ST, localPos, localNormal);
	
	// calculate gradient color
	float border1Alpha = border1Color.a*stencil.g;
	fixed4 border1Col = useTexture.x ? texCol*border1Color : border1Color;
	
	// calculate hard border color
	float border2Alpha = stencil.b*border2Color.a;
	fixed4 border2Col = useTexture.y ? border2Color*texCol : border2Color;
	
	// blend everything together
	if(mode.x) {
	    if(mode.w) {
	        // soft border
	        if(mode.y)
	            c = c + border1Alpha*border1Col;
	        else
	            c = border1Alpha*border1Col + (1-border1Alpha)*c;
	            
	        // hard border
	        if(mode.z) 
	            c = c + border2Alpha*border2Col;
	        else
	            c = border2Alpha*border2Col + (1-border2Alpha)*c;
	    } else {
	        // hard border
	        if(mode.z) 
	            c = c + border2Alpha*border2Col;
	        else
	            c = border2Alpha*border2Col + (1-border2Alpha)*c;
	            
	        
	        // soft border
	        if(mode.y)
	            c = c + border1Alpha*border1Col;
	        else
	            c = border1Alpha*border1Col + (1-border1Alpha)*c;
	    }
	} else {
	    // soft border
	    if(mode.y)
	        c = c + border1Alpha*border1Col;
	    else
	        c = border1Alpha*border1Col + (1-border1Alpha)*c;
	}
	
	// preserve alpha
	c.a = color.a;
	return c;
}

#endif