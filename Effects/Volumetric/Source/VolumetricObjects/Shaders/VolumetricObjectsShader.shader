/*
* This shader renders volumetric Objects in Screenspace using Raymarching
* It can render the following shapes:
* Box
* Sphere
* Capsule
* 
* Created with this Tutorial on Raymarching: http://flafla2.github.io/2016/10/01/raymarching.html
*   
* v1.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Hidden/VolumetricObjectsShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
            #pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Assets/Shaders/Includes/SignedDistanceFunctions.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 ray : TEXCOORD1;
			};
			
						
			// ############################# UNIFORMS PROVIDED BY VOLUMTERIC OBJECT RENDERER ############################# //
			// screen textures
			uniform sampler2D _MainTex;
            uniform float4 _MainTex_TexelSize;
            uniform sampler2D _CameraDepthTexture;
            
			// camera values
            uniform float4x4 _FrustumCornersES;
            uniform float4x4 _CameraInvViewMatrix;
            uniform float3 _CameraWS;
            
            // model values
            uniform float4x4 _InvModel;
            uniform fixed4 _Color;
            uniform float _Density;
            uniform float3 _BoxDimensions;
            uniform float _SphereRad;
            uniform float4x4 _CapsuleBounds;
            uniform int _Type;
            
            // raymarching values
            uniform int _StepCount;
            uniform int _StepCountInside;
            uniform float _StepSize;
            uniform float _DrawDist;
           
            // maps a point to the correct sd function       
            float map(float3 p) {
                // transform point 
                float3 tp = mul(_InvModel, float4(p, 1)).xyz;
                
                // calculate distance
                if(_Type == 0) {
                    return sdBox(tp, _BoxDimensions);
                } else if(_Type == 1) {           
                    return sdSphere(tp, _SphereRad);
                }else if(_Type == 2) {
                    return sdCapsule(tp, _CapsuleBounds[0].xyz, _CapsuleBounds[1].xyz, _SphereRad);
                }
                
                return 0;
            }
            
            // does the raymarching and returns a color
            fixed4 raymarch(float3 rayOrigin, float3 rayDirection, float depthBuffer) {
                // return color
                fixed4 ret = _Color;
                
                // the number of steps we took inside the Object
                float stepsInside = 0;
                
                // the distance we traveled
                float traveled = 0;
                
                // do raymarching
                for(int i = 0; i < _StepCount; ++i) {
                    // If we run past the depth buffer or the max Draw distance, stop and return nothing (transparent pixel)
                    // this way raymarched objects and traditional meshes can coexist.
                    if (traveled >= depthBuffer || traveled > _DrawDist) {
                        break;
                    }
        
                    float3 p = rayOrigin + rayDirection * traveled; // World space position of sample
                    float dist = map(p);       // Sample of distance field (see map())

                    // If the sample <= 0, we have hit something.
                     if (dist < 0.0001) {
                        ++stepsInside;
                        for(int i = 0; i < _StepCountInside; ++i) {
                            // If we run past the depth buffer or the max Draw distance, stop and return nothing (transparent pixel)
                            // this way raymarched objects and traditional meshes can coexist.
                            if (traveled >= depthBuffer || traveled > _DrawDist) {
                                break;
                            }
        
                            float3 p = rayOrigin + rayDirection * traveled; // World space position of sample
                            float dist = map(p);      // Sample of distance field

                            // If the sample <= 0, we are still inside the object
                            if (dist < 0.001) {
                                ++stepsInside;
                            } else {
                                break;
                            }

                            // march forward by step size
                            traveled += _StepSize;
                        }
                        break;
                     }

                    // If the sample > 0, we haven't hit anything yet so we should march forward
                    // We step forward by distance d, because d is the minimum distance possible to intersect
                    // an object (see map()).
                    traveled += dist;
                }
                
                // set the alpha of the Color to a value dependend from the density of the Object and how long we where inside it
                ret.a = saturate((stepsInside*_Density)/_StepCountInside);
                return ret;
            }
            

			v2f vert (appdata v)
			{
				v2f o;
				
				// Index passed via custom blit function in VolumetricObjectsRenderer.cs
                half index = v.vertex.z;
                v.vertex.z = 0.1;
    
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				
			    #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    o.uv.y = 1 - o.uv.y;
                #endif
    
                // Get the eyespace view ray (normalized)
                o.ray = _FrustumCornersES[(int)index].xyz;

                // Dividing by z "normalizes" it in the z axis
                // Therefore multiplying the ray by some number i gives the viewspace position
                // of the point on the ray with [viewspace z]=i
                o.ray /= abs(o.ray.z);

                // Transform the ray from eyespace to worldspace
                o.ray = mul(_CameraInvViewMatrix, o.ray);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// ray direction
                float3 rayDir = normalize(i.ray.xyz);
                // ray origin (camera position)
                float3 rayOrigin = _CameraWS;
                
                float2 duv = i.uv;
                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    duv.y = 1 - duv.y;
                #endif

                // Convert from depth buffer (eye space) to true distance from camera
                // This is done by multiplying the eyespace depth by the length of the "z-normalized"
                // ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
                // and the camera is proportional to the absolute distance.
                float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, duv).r);
                depth *= length(i.ray.xyz);

                // sample texture and create color
                fixed3 col = tex2D(_MainTex,i.uv);
                fixed4 add = raymarch(rayOrigin, rayDir, depth);

                // Returns final color using alpha blending
                return fixed4(col*(1.0 - add.w) + add.xyz * add.w,1.0);
			}
			ENDCG
		}
	}
}
