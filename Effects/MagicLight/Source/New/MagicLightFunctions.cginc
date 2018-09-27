/*
*   This include file contains functions for the magic light shaders
*   
*   v1.0 09/2018
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
*   wobbleSpeed - Speed of the animation
*   wobbleStrength - Strength of the animation
*/
float animatedDistance(float distance, float angle, float4 time, float wobbleSpeed, float wobbleStrength) {
    return distance+cos(sin(angle+time.y*wobbleSpeed))*wobbleStrength;
}

/* clips the fragment if it is inside the cylinder of the magic light and returns the out of bounds value (negative if inside the circle, positive if outside)
*
*   worldPos - World Position of the fragment
*   lightDir - Foreward Vector of the Magic light
*   lightPos - World Position of the Magic light
*   lightRad - Radius of the Magic Light cylinder
*   wobbleSpeed - Speed of the animation
*   wobbleStrength - Strength of the animation
*   time - Pass through for unity's _Time
*   inverted - should the cutout effect be inverted?
*/
float clipOutsideLightCircle(float3 worldPos, float3 lightDir, float3 lightPos, float lightRad, float wobbleSpeed, float wobbleStrength, float4 time, int inverted) {
    // project the fragment onto the plane of the magic light origin
	float3 projected = worldPos - dot(lightDir, worldPos - lightPos) * lightDir;
	
	// calculate the distance between the projected fragment and the light origin
	float distance = length(projected - lightPos);
	
	// calculate the angle of the vector between the center of the circle and the fragment to the up vector of the circle
	float angle = acos(dot(normalize(projected - lightPos), float3(0.0,1.0,0.0)));
	
	// calculate the out of bounds value by testing if the fragment is inside the radius
	float animatedDist = animatedDistance(distance, angle, time, wobbleSpeed, wobbleStrength);
	float outOfBounds = inverted ? lightRad-animatedDist : animatedDist-lightRad;
	
	// clip if out of bounds is negative
	clip(outOfBounds);
	
	return outOfBounds;
}


/*  returns a color for the finalColor function that is a blend of the two border types and the base color
*
*   color - The base color to build on to
*   outOfBounds - The out of bounds value of the fragment
*   tex - texture for the border that is applied via triplanar mapping
*   tex_ST - Scale and Transform for the border texture
*   localPos - Local Position of the fragment
*   localNormal - Local Normal of the fragment
*   borderThreshold - Threshold for the gradient border
*   borderColor - Color of the gradient border
*   hardBorderThreshold - Threshold for the hard border
*   hardBorderColor - Color of the gard border
*   mode - the border modes. 
*       x > 0 = hard border is rendered here (else it is in emission)
*       y > 0 = soft border is rendered additive, else alpha blended
*       z > 0 = hard border is rendered additive, else alpha blended
*       w > 0 = hard border is rendered on top of soft border, else the other way around
*/
fixed4 drawBorder(fixed4 color, float outOfBounds, sampler2D tex, float4 tex_ST, float3 localPos, float3 localNormal, float borderThreshold, fixed4 borderColor, float hardBorderThreshold, fixed4 hardBorderColor, float4 mode) {
    // save the color and get the texture color
    fixed4 c = color;
    fixed4 texCol = triplanarTex(tex, tex_ST, tex,tex_ST, tex,tex_ST, localPos, localNormal);
	
	// calculate gradient color
	float gradientAlpha = saturate(borderColor.a-(outOfBounds/borderThreshold));
	fixed4 gradient = texCol*borderColor*gradientAlpha;
	
	// calculate hard border color
	fixed4 hardCol = hardBorderColor*texCol;
	
	// blend everything together
	if(mode.x > 0) {
	    if(mode.w > 0) {
	        // soft border
	        if(mode.y > 0)
	            c = c + gradient;
	        else
	            c = gradient + (1-gradientAlpha)*c;
	            
	        // hard border
	        if(outOfBounds <= hardBorderThreshold) {
	            if(mode.z > 0) 
	                c = c + hardCol;
	            else
	                c = hardCol.a*hardCol + (1-hardCol.a)*c;
	        }
	    } else {
	        // hard border
	        if(outOfBounds <= hardBorderThreshold) {
	            if(mode.z > 0) 
	                c = c + hardCol;
	            else
	                c = hardCol.a*hardCol + (1-hardCol.a)*c;
	            
	        }
	        
	        // soft border
	        if(mode.y > 0)
	            c = c + gradient;
	        else
	            c = gradient + (1-gradientAlpha)*c;
	    }
	} else {
	    // soft border
	    if(mode.y > 0)
	        c = c + gradient;
	    else
	        c = gradient + (1-gradientAlpha)*c;
	}
	
	// preserve alpha
	c.a = color.a;
	return c;
}

#endif