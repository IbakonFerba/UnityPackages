
/*
* This Unlit Shader is cut away inside the cylinder of the MagicLightsource or a specified stencil.
* Backfaces are rendered unlit and around the cutout two borders can be rendered. The cutout can also animate
* It can also fade to an unlit secondary color.
*
* v2.0 09/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Magic Light/Unlit" {
	Properties {
		// surface shader properties
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		
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
	CustomEditor "MagicLightUnlitEditor"
	SubShader {
	    UsePass "Magic Light/Standard/RenderBack"
	    
		Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }
		CULL BACK
		LOD 200
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
			float2 uv : TEXCOORD0;
			
			#if USE_MAGICLIGHT
			float4 normal : NORMAL;
			#endif
		};

		struct v2f
		{
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			
			#if USE_MAGICLIGHT
			float3 worldPos : TEXCOORD1;
			float3 localPos : TEXCOORD2;
			float3 localNormal : TEXCOORD3;
			#endif
		};
		
		// Textures and Maps
		sampler2D _MainTex;
		float4 _MainTex_ST;
		
		// color
		fixed4 _Color;
		
		#if USE_MAGICLIGHT 
		// Textures and Maps
		sampler2D _BorderTex;
		float4 _BorderTex_ST;  
		
		sampler2D _StencilMap;
		float4 _StencilMap_ST;
		    
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
		
		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			
			#if USE_MAGICLIGHT
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.localPos = v.vertex.xyz;
            o.localNormal = v.normal.xyz;
            #endif
            
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}
			
		fixed4 frag (v2f i) : SV_Target
		{
		    fixed4 col = tex2D(_MainTex, i.uv)*_Color;
		    
		    #if USE_MAGICLIGHT
		    // clip and draw border
			outOfBounds=0;
			if (_UseMagicLight) {
			    if(_UseStencil) {
			        outOfBounds =  clipStencil(_StencilMap, _StencilMap_ST, i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _WobbleParams, _Time);
				    col = drawBorder(col, outOfBounds, _BorderTex, _BorderTex_ST, i.localPos, i.localNormal, _Border1Color, _Border2Color, _UseBorderTexture, _BorderMode);
			    } else {
				    outOfBounds = clipCircle(i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _MagicLightRad*_RadiusScaleFactor, _WobbleParams, _Time, _Inverted);
				    col = drawBorder(col, outOfBounds.x, _BorderTex, _BorderTex_ST, i.localPos, i.localNormal, _BorderThresholds, _Border1Color, _Border2Color, _UseBorderTexture, _BorderMode);
			    }
			}
            #endif  
		    

			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
		}
		ENDCG
		}		
	}
	FallBack "Diffuse"
}
