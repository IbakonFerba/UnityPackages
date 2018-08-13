/*
* This shader renders a Volumetric Box that can be viewed from the outside, not the inside
*
* v1.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Custom/Volumetric Box" {
	Properties {	
		 _Color("Color (RGB)", Color) = (1,1,1,1)
	    _Density("Density", Float) = 1
	    _Dimensions("Dimensions (XYZ)", Vector) = (1,1,1,0)
	    
	    _Steps("Steps", Float) = 64
	    _StepSize("Step Size", Float) = 0.01
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching"="True" }
		LOD 200

        ZWrite Off
        ZTest Always
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Unlit alpha
        #include "UnityPBSLighting.cginc"
        
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float3 worldPos;
			float3 origin;
		};
		
		fixed4 _Color;
		
	    float _Density;
		float3 _Dimensions;
			
		float _Steps;
		float _StepSize;

        // had to use a surface shader because of problems getting the world space position of a fragment, so I need to do a custom lighting model here
        half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
        {
         return half4(s.Albedo, s.Alpha);
        }
    
        // returns true if a given point is inside the defined cube
		bool insideCube(float3 origin, float3 pos) {
			    float3 dim = _Dimensions.xyz*0.5;
			    
			    if(pos.x > origin.x-dim.x && 
			    pos.x < origin.x+dim.x &&
			    pos.y > origin.y-dim.y &&
			    pos.y < origin.y+dim.y &&
			    pos.z > origin.z-dim.z &&
			    pos.z < origin.z+dim.z) {
			        return true;
			    }
			    
			    return false;
			}
			
			// returns a value between 0 and 1 depending on how far it is from the entrance point to either the exit or the end of the ray (if the end of the ray is reached it just returns 1)
			float distToExit(float3 worldPos, float3 origin, float3 viewDir) {
			    float hits = 0;
			    for (int i = 0; i < _Steps; i++)
                {
                    if ( insideCube(origin, worldPos) )
                        ++hits;
 
                    worldPos += viewDir * _StepSize;
                }
			    return hits*_Density/_Steps;
			}
        
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
		    o.Albedo = _Color;
			o.Alpha = distToExit(IN.worldPos, mul(unity_ObjectToWorld, float4(0.0,0.0,0.0,1.0) ), normalize(IN.worldPos - _WorldSpaceCameraPos));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
