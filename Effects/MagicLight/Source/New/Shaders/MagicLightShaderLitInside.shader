
/*
* This Shader is basically a Standard Surface Shader, but with the added effect that it is cut away inside the cylinder of the MagicLightsource.
* Backfaces are rendered lit and aorund the cutout a hard and a faded border can be rendered. The cutout can also animate
* It can also fade to an unlit secondary color.
*
* v1.0 09/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Magic Light/Lit Inside" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		[NoScaleOffset]
		_SmoothnessMap("Smoothness (R), Metallic (G)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[Gamma]
		_Metallic ("Metallic", Range(0,1)) = 0.0
		[Normal] [NoScaleOffset]
		_BumpMap("Normal Map (RGB)", 2D) = "bump" {}
		[NoScaleOffset]
        _OcclusionMap("Occlusion (R)", 2D) = "white" {}
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		[HDR]
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[NoScaleOffset]
		_EmissionMap("Emission (RGB)", 2D) = "white" {}
		
		[Space]
		_Color2("Secondary Color", Color) = (1,1,1,1)
		_Color2Strength("Secondary Color Strength", Range(0, 1)) = 0
		
		[Space]
		_BorderMode("Border Mode", Vector) = (0,0,0)
		_BorderTex("Border Texture (RGBA)", 2D) = "white" {}
		_BorderColorHard("Hard Border Color", Color) = (1, 1, 1, 1)
		_BorderThresholdHard("Hard Border Threshold", Float) = 0.01
		_BorderColor("Border Color", Color) = (1, 1, 1, 1)
		_BorderThreshold("Border Threshold", Float) = 0.01
		
		[Space]
		_RadiusScaleFactor("Radius Scale Factor", Float) = 1
		_WobbleSpeed("Wobble Speed", float) = 1
		_WobbleStrength("Wobble Strength", float) = 1
		
		[Space]
		[Toggle]
		_UseMagicLight("Use Magic Light", float) = 1
		_Inverted("Inverted", float) = 0
	}
	CustomEditor "MagicLightNoInsideEditor"
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }
		CULL off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows finalcolor:renderBorderAndSecondColor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		#include "Assets/Shaders/Includes/MagicLightFunctions.cginc"
		
		// Textures and Maps
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SmoothnessMap;
		sampler2D _EmissionMap;
		sampler2D _OcclusionMap;
		
		sampler2D _BorderTex;
		float4 _BorderTex_ST;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 localPos;
			float3 localNormal;	
			INTERNAL_DATA
		};

        // surface shader values
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _EmissionColor;
		float _OcclusionStrength;
		
        // secondary color
		fixed4 _Color2;
		float _Color2Strength;
        
        // Border values
        float4 _BorderMode;
        fixed4 _BorderColor;
		fixed4 _BorderColorHard;
		float _BorderThresholdHard;
		float _BorderThreshold;

        // amgic Light values
        int _Inverted;
        float _UseMagicLight;
		float4 _MagicLightPos;
		float4 _MagicLightDir;
		float _MagicLightRad;
		float _RadiusScaleFactor;
		
		// animation
		float _WobbleSpeed;
		float _WobbleStrength;

        // the out of bounds value of the fragment
		float outOfBounds;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            data.localPos = v.vertex.xyz;
            data.localNormal = v.normal.xyz;
        }
        
		void surf (Input IN, inout SurfaceOutputStandard o) {
		    // clip inside magic light or get out of bounds value
			outOfBounds=0;
			if (_UseMagicLight) {
				outOfBounds = clipOutsideLightCircle(IN.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _MagicLightRad*_RadiusScaleFactor, _WobbleSpeed, _WobbleStrength, _Time, _Inverted);
				
				// if the hard border mode is set to Emission, set an emission color
				if(outOfBounds <= _BorderThresholdHard && _BorderMode.x == 0) {
					o.Emission = _BorderColorHard;
				}
			}


			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			
			// Metallic and smoothness
			fixed4 smoothMetallic = tex2D(_SmoothnessMap, IN.uv_MainTex);
			o.Metallic = smoothMetallic.g*_Metallic;
			o.Smoothness = smoothMetallic.r*_Glossiness;
			
			// normal mapping
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			
			//Emission
			o.Emission += tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor;
			
			//Occlusion
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_MainTex).r*_OcclusionStrength;
		}

        // this function gets passed the final color and can manipulate it
		void renderBorderAndSecondColor(Input IN, SurfaceOutputStandard o, inout fixed4 color) {
		    // only work on the first pass
#ifdef UNITY_PASS_FORWARDBASE
            // draw the border
			if(_UseMagicLight)
				color = drawBorder(color, outOfBounds, _BorderTex, _BorderTex_ST, IN.localPos, IN.localNormal, _BorderThreshold, _BorderColor, _BorderThresholdHard, _BorderColorHard, _BorderMode);
			
			// apply the second color	
			color = _Color2Strength * _Color2 + (1 - _Color2Strength) * color;
#endif
		}
		ENDCG
		
		UsePass "Magic Light/Standard/ShadowCaster"
	}
	FallBack "Diffuse"
}