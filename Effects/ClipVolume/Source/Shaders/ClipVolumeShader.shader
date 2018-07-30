﻿/*
* This Shader is basically a Standard Surface Shader, but with the added effect that it is only visible inside a Clip Volume.
* Backface Culling is turned of and the inside is Colored in a Flat, unlit color so that the object looks solid (from most angles)
*
* v1.1 07/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Clip Volume/Clip Volume" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset]
		_SmoothnessMap("Smoothness (R), Metallic (G)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		[Normal] [NoScaleOffset]
		_NormalMap("Normal Map (RGB)", 2D) = "bump" {}
		[HDR]
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[NoScaleOffset]
		_EmissionMap("Emission (RGB)", 2D) = "white" {}
		_ClipVolumeMin("Min XYZ", Vector) = (0,0,0)	
		_ClipVolumeMax("Max XYZ", Vector) = (1,1,1)
		_InsideColor("Inside Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200

        Cull Off
		CGPROGRAM
		// Custom lighting model based on the Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Custom fullforwardshadows

        // includes
        #include "UnityPBSLighting.cginc"
        #include "Assets/Shaders/Includes/ClipVolume.cginc"
        
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        // textures and maps
		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _SmoothnessMap;
		sampler2D _EmissionMap;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};
        
        // surface shader values
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _EmissionColor;
		
		// backface rendering
		fixed4 _InsideColor;	
        int back = 0;
        
        // clip volume definition
		float4 _ClipVolumeMin;
		float4 _ClipVolumeMax;	
		float4x4 _ClipVolumeWorldToLocal;
		float3 _ClipVolumeWorldPos;
        
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)
        
        // custom lighting function that uses the Standard PBR Lighting when rendering a fronface and just returns a color when rendering a backface
        half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
        {
	        if (!back)
		        return LightingStandard(s, lightDir, gi); // Unity5 PBR
	        return _InsideColor; // Unlit
        }

        // custom GI function needed when using custom lighting model
        void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
        {
	        LightingStandard_GI(s, data, gi);		
        }
        
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
		    // use clip volume
            useClipVolume(IN.worldPos.xyz, _ClipVolumeWorldToLocal, _ClipVolumeWorldPos.xyz, _ClipVolumeMin.xyz, _ClipVolumeMax.xyz); 
		    
		    // determine whether this fragment is on a backface or on a front face	
		    back = backface(o.Normal.xyz, IN.viewDir.xyz);
		
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			
			// Metallic and smoothness
			fixed4 smoothMetallic = tex2D(_SmoothnessMap, IN.uv_MainTex);
			o.Metallic = smoothMetallic.g*_Metallic;
			o.Smoothness = smoothMetallic.r*_Glossiness;
			o.Alpha = c.a;
			
			// normal mapping
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			
			//Emission
			o.Emission = tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor;
		}       
		ENDCG
	}
	FallBack "Diffuse"
}
