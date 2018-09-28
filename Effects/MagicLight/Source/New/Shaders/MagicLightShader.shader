
/*
* This Shader is basically a Standard Surface Shader, but with the added effect that it is cut away inside the cylinder of the MagicLightsource or a specified stencil.
* Backfaces are rendered unlit and around the cutout two borders can be rendered. The cutout can also animate
* It can also fade to an unlit secondary color.
*
* v3.0 09/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Magic Light/Standard" {
	Properties {
	    // surface shader properties
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
		
		// secondary color
		[Space]
		_Color2("Secondary Color", Color) = (1,1,1,1)
		_Color2Strength("Secondary Color Strength", Range(0, 1)) = 0
		
		// backfaces
		[Space]
		_InsideColor("Inside Color", Color) = (1, 1, 1, 1)
		
		// stencil options
		[Space]
		[Toggle]
		_UseStencil("Use Stencil", Float) = 0
		_StencilMap("_StencilMap", 2D) = "white" {}
		
		// Border
		[Space]
		_BorderTex("Border Texture (RGBA)", 2D) = "white" {}
		_BorderMode("Border Mode", Vector) = (0,0,0)
		_UseBorderTexture("Use Border Texture", Vector) = (1,1,0)
		_BorderThresholds("Border Thresholds", Vector) = (0.01, 0.01, 0)
		_Border1Color("Border 1 Color", Color) = (1, 1, 1, 1)
		_Border2Color("Border 2 Color", Color) = (1, 1, 1, 1)

        // cutout manipulation
		[Space]
		_RadiusScaleFactor("Radius Scale Factor", Float) = 1
		_WobbleParams("Wobble Parameters (Speed, Strength)", Vector) = (1,1,0)
		
		// additional options
		[Space]
		[Toggle(USE_MAGICLIGHT)]
		_UseMagicLight("Use Magic Light", float) = 1
		[Toggle]
		_Inverted("Inverted", float) = 0
	}
	CustomEditor "MagicLightStandardEditor"
	SubShader {
	    // render backfaces
		Tags { "RenderType"="Opaque" "Queue"="AlphaTest"}
	    LOD 100
	    
	    Cull front
	    Name "RenderBack"
	    Pass {
	    CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#pragma multi_compile _ USE_MAGICLIGHT
			
			#include "UnityCG.cginc"
			#include "Assets/Shaders/Includes/MagicLightFunctions.cginc"
			
			
			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				#if USE_MAGICLIGHT
				float3 worldPos : TEXCOORD0;
				#endif
			};
			
		    fixed4 _InsideColor;


            #if USE_MAGICLIGHT
            // Magic light values
            int _Inverted;
            float _UseMagicLight;
			float4 _MagicLightPos;
			float4 _MagicLightDir;
			float _MagicLightRad;
			
		    float _RadiusScaleFactor;
				
			// stencil values		
			float _UseStencil;
			sampler2D _StencilMap;
			float4 _StencilMap_ST;
			
			// animation
			float2 _WobbleParams;
            #endif
	
            		
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				#if USE_MAGICLIGHT
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    #if USE_MAGICLIGHT
			    // clip everything inside the parameters of the magic light
				if(_UseStencil)
				    clipStencil(_StencilMap, _StencilMap_ST, i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _WobbleParams, _Time);
				else
				    clipCircle(i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _MagicLightRad*_RadiusScaleFactor, _WobbleParams, _Time, _Inverted);
                #endif
                
                // set the color
			    fixed4 col = _InsideColor;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
	    }
		
		// render the object
		Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }
		CULL BACK
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows finalcolor:renderBorderAndSecondColor
		
		#pragma multi_compile _ USE_MAGICLIGHT

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		#include "Assets/Shaders/Includes/MagicLightFunctions.cginc"
		
		// Textures and Maps
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SmoothnessMap;
		sampler2D _EmissionMap;
		sampler2D _OcclusionMap;
		
		#if USE_MAGICLIGHT
		sampler2D _BorderTex;
		float4 _BorderTex_ST;
		
		sampler2D _StencilMap;
		float4 _StencilMap_ST;
		#endif

		struct Input {
			float2 uv_MainTex;
			#if USE_MAGICLIGHT
			float3 worldPos;
			float3 localPos;
			float3 localNormal;	
			#endif
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
		half _Color2Strength;
        
        # if USE_MAGICLIGHT
        // Border values
        half4 _BorderMode;
        half2 _UseBorderTexture;
        fixed4 _Border1Color;
		fixed4 _Border2Color;
		float2 _BorderThresholds;

        // Magic Light values
        int _Inverted;
        float _UseMagicLight;
		float4 _MagicLightPos;
		float4 _MagicLightDir;
		float _MagicLightRad;
		
		float _RadiusScaleFactor;
		
		// stencil
		float _UseStencil;
		
		// animation
		float2 _WobbleParams;

        // the out of bounds value of the fragment
		float3 outOfBounds;
		#endif

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            
            #if USE_MAGICLIGHT
            data.localPos = v.vertex.xyz;
            data.localNormal = v.normal.xyz;
            #endif
        }
        
		void surf (Input IN, inout SurfaceOutputStandard o) {
		    #if USE_MAGICLIGHT
		    // clip inside the magic light parameters and get the out of bounds value
		    if(_UseStencil) {
			    outOfBounds = clipStencil(_StencilMap, _StencilMap_ST, IN.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _WobbleParams, _Time);
			    
			    // if the hard border mode is set to Emission, set an emission color
			    if(_BorderMode.x == 0) {
				    o.Emission = _Border2Color*outOfBounds.b;
			    }
		    } else {
				outOfBounds.x = clipCircle(IN.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _MagicLightRad*_RadiusScaleFactor, _WobbleParams, _Time,_Inverted);
				
				// if the hard border mode is set to Emission, set an emission color
			    if(outOfBounds.x <= _BorderThresholds.y && _BorderMode.x == 0) {
				    o.Emission = _Border2Color;
			    }
			}		
            #endif



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
            #if USE_MAGICLIGHT
            // draw the border
            if(_UseStencil)
                color = drawBorder(color, outOfBounds, _BorderTex, _BorderTex_ST, IN.localPos, IN.localNormal, _Border1Color, _Border2Color, _UseBorderTexture, _BorderMode);
            else
			    color = drawBorder(color, outOfBounds.x, _BorderTex, _BorderTex_ST, IN.localPos, IN.localNormal, _BorderThresholds, _Border1Color, _Border2Color, _UseBorderTexture, _BorderMode);
			#endif
			
			// apply the second color	
			color = _Color2Strength * _Color2 + (1 - _Color2Strength) * color;
            #endif
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
        
        #pragma multi_compile _ USE_MAGICLIGHT

        #include "UnityCG.cginc"
        #include "Assets/Shaders/Includes/MagicLightFunctions.cginc"

        struct v2f
        {
            V2F_SHADOW_CASTER;
            
            #if USE_MAGICLIGHT
            float3 worldPos : TEXCOORD0;
            #endif
        };

        v2f vert(appdata_base v)
        {
            v2f o;
            
            #if USE_MAGICLIGHT
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            #endif
            
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            return o;
        }
		
		#if USE_MAGICLIGHT
		// magic light values
		int _Inverted;
	    float _UseMagicLight;
		float4 _MagicLightPos;
		float4 _MagicLightDir;
		float _MagicLightRad;
		
		float _RadiusScaleFactor;
		
		// stencil
		float _UseStencil;
		sampler2D _StencilMap;
		float4 _StencilMap_ST;
		
		// animation
		float2 _WobbleParams;
        #endif

        float4 frag(v2f i) : COLOR
        {
            #if USE_MAGICLIGHT
            // clip inside magic light parameters
            if(_UseStencil)
                clipStencil(_StencilMap, _StencilMap_ST, i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _WobbleParams, _Time);
            else
                clipCircle(i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _MagicLightRad*_RadiusScaleFactor, _WobbleParams, _Time, _Inverted);
            #endif    
            
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
        }
	}
	FallBack "Diffuse"
}
