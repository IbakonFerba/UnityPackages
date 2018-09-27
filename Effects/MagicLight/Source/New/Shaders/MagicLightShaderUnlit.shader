
/*
* This Unlit Shader is cut away inside the cylinder of the MagicLightsource.
* Backfaces are rendered unlit and aorund the cutout a hard and a faded border can be rendered. The cutout can also animate
* It can also fade to an unlit secondary color.
*
* v1.0 09/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Magic Light/Unlit" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
		
		[Space]
		_InsideColor("Inside Color", Color) = (1, 1, 1, 1)
		
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
			
		#include "UnityCG.cginc"
		#include "Assets/Shaders/Includes/MagicLightFunctions.cginc"	

        struct appdata
		{
		    float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float4 normal : NORMAL;
		};

		struct v2f
		{
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
			float3 localPos : TEXCOORD2;
			float3 localNormal : TEXCOORD3;
		};
		
		// Textures and Maps
		sampler2D _MainTex;
		float4 _MainTex_ST;
		
		sampler2D _BorderTex;
		float4 _BorderTex_ST;

        // color
		fixed4 _Color;
        
        // Border
        float4 _BorderMode;
        fixed4 _BorderColor;
		fixed4 _BorderColorHard;
		float _BorderThresholdHard;
		float _BorderThreshold;
		
		// animation
		float _WobbleSpeed;
		float _WobbleStrength;
		
        // Magic Light values
        int _Inverted;
        float _UseMagicLight;
		float4 _MagicLightPos;
		float4 _MagicLightDir;
		float _MagicLightRad;
		float _RadiusScaleFactor;
		
		// the out of bounds value of the fragment
		float outOfBounds;
		
		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.localPos = v.vertex.xyz;
            o.localNormal = v.normal.xyz;
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}
			
		fixed4 frag (v2f i) : SV_Target
		{
		    fixed4 col = tex2D(_MainTex, i.uv)*_Color;
		    
		    // clip and draw border
			outOfBounds=0;
			if (_UseMagicLight) {
				outOfBounds = clipOutsideLightCircle(i.worldPos.xyz, _MagicLightDir.xyz, _MagicLightPos.xyz, _MagicLightRad*_RadiusScaleFactor, _WobbleSpeed, _WobbleStrength, _Time, _Inverted);
				col = drawBorder(col, outOfBounds, _BorderTex, _BorderTex_ST, i.localPos, i.localNormal, _BorderThreshold, _BorderColor, _BorderThresholdHard, _BorderColorHard, _BorderMode);
			}
                
		    

			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
		}
		ENDCG
		}		
	}
	FallBack "Diffuse"
}
