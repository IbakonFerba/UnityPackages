/*
*   This is a PBR Shader that shares the most important attributes with the Standard Unity Shader,
*   but adds a Layer of triplanar mapped dust on the upward facing faces of the Mesh (Taking the normal map into account)
*
*   Written by Fabian Kober
*   v1.0 11/2018
*   fabian-kober@gmx.net
*/
Shader "Custom/Triplanar Dust" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		
		[NormalMap][NoScaleOffset]
		_BumpMap("Normal Map (RGB)", 2D) = "bump" {}
		
		[Space]
		_Glossiness ("Glossiness", Range(0,1)) = 0.5
		[Gamma]
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
		[Space]
		[NoScaleOffset]
		_MetallicGlossMap("Metallic Smoothness Map (R-Metallic, A-Smoothness)", 2D) = "white" {}
		
		[Space]
		[NoScaleOffset]
		_OcclusionMap("Occlusion (R)", 2D) = "white" {}
		_OcclusionStrength("Occlusion Strength", Range(0,1) ) = 1
		
		[Space]
		[NoScaleOffset]
		_ParallaxMap("Height Map", 2D) = "white" {}
		_Parallax("Height Strength", Range(0.005, 0.08)) = 0.02
		
		[Space]
		_DustColor("Dust Color", Color) = (1,1,1,1)
		_DustMap("Dust Map (RGBA)", 2D) = "white" {}
		_DustStrength("Dust Strength", Range(0,1)) = 1
		_DustThreshold("Dust Threshold", Range(0,1)) = 0
		_DustSoftness("Dust Softness", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows
		#include "Assets/Shaders/Includes/Utility.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        // surface shader
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _MetallicGlossMap;
		sampler2D _OcclusionMap;
		sampler2D _ParallaxMap;
		
		// dust
		sampler2D _DustMap;
		float4 _DustMap_ST;

		struct Input {
			float2 uv_MainTex;
			float3 localPos;
			float3 localNormal;
			float3 viewDir;
			float3 worldNormal; INTERNAL_DATA
		};

        // surface shader values
        fixed4 _Color;
		half _Glossiness;
		half _Metallic;
		half _OcclusionStrength;
		float _Parallax;
		
		// dust values
		fixed4 _DustColor;
		half _DustStrength;
		half _DustThreshold;
		half _DustSoftness;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)
		
		void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            
            // get local position and normal for triplanar mapping
            data.localPos = v.vertex.xyz;
            data.localNormal = v.normal.xyz;
        }

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
		    // calculate parallaxed UVs using a Heightmap
		    float2 parallaxOffset = ParallaxOffset(tex2D(_ParallaxMap, IN.uv_MainTex).r, _Parallax, IN.viewDir);
		    float2 uv = IN.uv_MainTex+parallaxOffset;
		    
		    // get normal mapped normal
		    o.Normal = UnpackNormal(tex2D(_BumpMap, uv));
			float3 normal = WorldNormalVector (IN, o.Normal);
			
			// calculate the amount of dust on this fragment by checking the angle of the fragment in relation to the world up vector
			float3 worldUp = float3(0,1,0);
			fixed angle = (dot(worldUp, normal)+1)*0.5;
			
			// calculate the dust amount using the angle. First cap the amount to be between 0 and 1-_DustThreshold. With this we discard everything that is facing further away from up than _DustThreshold
			fixed dustAmount = saturate(angle-_DustThreshold);
			
			// remap the dust amount to be between 0 and 1
			dustAmount = remapFixed(dustAmount, 0, 1-_DustThreshold, 0, 1);
			
			// change the steepness of the dustAmount gradient and calculate the final dust strength for this fragment
			dustAmount = pow(dustAmount, _DustSoftness);
			half dustStrength = saturate(_DustStrength*dustAmount);
			
			// get map information
			fixed4 albedo = tex2D (_MainTex, uv) * _Color;
			fixed4 metallicGloss = tex2D(_MetallicGlossMap, uv);
			fixed4 dust = triplanarTex(_DustMap, _DustMap_ST, IN.localPos, IN.localNormal);
			
			// tint rgb of dust and take the dust alpha into account for the strength
			dust.rgb *= _DustColor.rgb;
			dustStrength *= dust.a;
			
			// do a normal alpha blend between albedo and the dust
			albedo.rgb = (1-dustStrength)*albedo.rgb + dustStrength * dust.rgb;
			
			// set output values
			o.Albedo = albedo.rgb;
			o.Metallic = saturate(metallicGloss.r*_Metallic-dustStrength);
			o.Smoothness = saturate(metallicGloss.a*_Glossiness-dustStrength);
			o.Occlusion = LerpOneTo(tex2D(_OcclusionMap, IN.uv_MainTex).r, _OcclusionStrength);
			o.Alpha = albedo.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
