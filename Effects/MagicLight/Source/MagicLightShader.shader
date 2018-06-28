
/*
* This Shader is basically a Standard Surface Shader, but with the added effect that it is only visible inside the Light cone of the MagicLightsource.
* Backface Culling is turned of and the inside is Colored in a Flat, unlit color along with the edges where it is cut of by the lightcone, so it looks like a solid object.
* It can also fade to an unlit secondary color.
*
* v1.0 06/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "FK/Effects/Magic Light" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Secondary Color", Color) = (1,1,1,1)
		_Color2Strength("Secondary Color Strength", Range(0, 1)) = 0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_BorderThreshold("Border Threshold", Float) = 0.01
		_BorderColor("Border Color", Color) = (1, 1, 1, 1)
		_UseMagicLight("Use Magic Ligth", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }
		CULL Off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Custom fullforwardshadows finalcolor:secondColor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "UnityPBSLighting.cginc"

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _Color2;

		float _Color2Strength;

		float _BorderThreshold;
		fixed4 _BorderColor;

		float4 _MagicLightPos;
		float4 _MagicLightDir;
		float _MagicLightAngle;

		float _UseMagicLight;

		int cutoff;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			if (_UseMagicLight == 1) {
				// get the vector from the Light to the fragement in world space
				float3 fromLight = normalize(IN.worldPos.xyz - _MagicLightPos.xyz);
				// calculate the angle between the light direction and the direction from the light to the frag
				float angle = acos(dot(normalize(_MagicLightDir.xyz), fromLight.xyz));
				clip(_MagicLightAngle*0.5 - angle); // discard frag if the angle is bigger than half the light angle of the Light

													// if the angle is within a certain threshold, mark the fragment so its color can be set later (this creates a border where the objects starts to dissolve)
				if (_MagicLightAngle*0.5 - angle < _BorderThreshold || dot(o.Normal, IN.viewDir) < 0) {
					cutoff = 1;
				}
			}

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}

	    inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
		{
			if (!cutoff)
				return LightingStandard(s, lightDir, gi); // Unity5 PBR
#ifdef UNITY_PASS_FORWARDBASE
			return _BorderColor; // Unlit
#endif
			return float4(0, 0, 0, 0);
		}

		inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		void secondColor(Input IN, SurfaceOutputStandard o, inout fixed4 color) {
#ifdef UNITY_PASS_FORWARDBASE
			if(!cutoff)
			color = _Color2Strength * _Color2 + (1 - _Color2Strength) * color;
#endif
		}
		ENDCG
	}
	FallBack "Diffuse"
}
