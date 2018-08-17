/*
* This Shader is basically a Standard Surface Shader, but with the added effect that it is only visible inside a Clip Volume and Fades using Dithering towards the Border of the Clip Volume.
* Backface Culling is turned of and the inside is Colored in a Flat, unlit color so that the object looks solid (from most angles)
*
* v2.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Clip Volume/Fade Border Transparent" {
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
        _OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		[HDR]
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[NoScaleOffset]
		_EmissionMap("Emission (RGB)", 2D) = "white" {}
		_ClipVolumeMin("Min XYZ", Vector) = (0,0,0)	
		_ClipVolumeMax("Max XYZ", Vector) = (1,1,1)
		_BorderGradientHardness("Border Hardness", Float) = 50
		[NoScaleOffset]
		_NoiseMap("Noise Map (R)", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 1
		_NoiseThreshold("Threshold", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		LOD 200

        ZWrite Off
		CGPROGRAM
		// Custom lighting model based on the Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows alpha:premul
		
		// includes
        #include "UnityPBSLighting.cginc"
        #include "Assets/Shaders/Includes/ClipVolume.cginc"
        
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        // textures and maps
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SmoothnessMap;
		sampler2D _EmissionMap;
		sampler2D _NoiseMap;
		sampler2D _OcclusionMap;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
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
		
		// Clip volume definition
		float4 _ClipVolumeMin;
		float4 _ClipVolumeMax;	
		float4x4 _ClipVolumeWorldToLocal;
		float3 _ClipVolumeWorldPos;  
		
		// border fading
		float _BorderGradientHardness;
        float _NoiseThreshold;
        float _NoiseScale;
		      
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
        
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
		    // use clip volume and get the out of bounds value back
            float outOfBounds = useClipVolume(IN.worldPos.xyz, _ClipVolumeWorldToLocal, _ClipVolumeWorldPos.xyz, _ClipVolumeMin.xyz, _ClipVolumeMax.xyz); 
			
			// fade border
		    fadeBorder(outOfBounds, _NoiseMap, _NoiseScale, _NoiseThreshold, _BorderGradientHardness, IN.localPos, IN.localNormal);
		    
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			
			// Metallic and smoothness
			fixed4 smoothMetallic = tex2D(_SmoothnessMap, IN.uv_MainTex);
			o.Metallic = smoothMetallic.g*_Metallic;
			o.Smoothness = smoothMetallic.r*_Glossiness;
			o.Alpha = c.a;
			
			// normal mapping
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			
			//Emission
			o.Emission = tex2D(_EmissionMap, IN.uv_MainTex) * _EmissionColor;
			
			//Occlusion
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_MainTex)*_OcclusionStrength;
		}       
		ENDCG
		
			
		// shadow caster pass	
		Tags{ "LightMode" = "ShadowCaster"  }
        Name "ShadowCaster"
        Pass
        {
        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_shadowcaster
        #pragma fragmentoption ARB_precision_hint_fastest

        #include "UnityCG.cginc"
        #include "Assets/Shaders/Includes/ClipVolume.cginc"

        struct v2f
        {
            V2F_SHADOW_CASTER;
            float3 worldPos : TEXCOORD0;
        };

        v2f vert(appdata_base v)
        {
            v2f o;
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            return o;
        }
    
       // clip volume definition
	    float4 _ClipVolumeMin;
	    float4 _ClipVolumeMax;	
	    float4x4 _ClipVolumeWorldToLocal;
	    float3 _ClipVolumeWorldPos;

        float4 frag(v2f i) : COLOR
        {
            useClipVolume(i.worldPos.xyz, _ClipVolumeWorldToLocal, _ClipVolumeWorldPos.xyz, _ClipVolumeMin.xyz, _ClipVolumeMax.xyz);
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
        }
	}
	FallBack "Diffuse"
}
