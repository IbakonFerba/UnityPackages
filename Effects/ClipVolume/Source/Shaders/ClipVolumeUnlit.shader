/*
* This Shader is an unlit shader that is only visible inside a Clip Volume and Fades using Dithering towards the Border of the Clip Volume.
* Backface Culling is turned of so that the object looks solid (from most angles)
*
* v2.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Clip Volume/Unlit"
{
	Properties
	{
	    _Color ("Color", Color) = (1,1,1,1)
	    _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ClipVolumeMin("Min XYZ", Vector) = (0,0,0)	
		_ClipVolumeMax("Max XYZ", Vector) = (1,1,1)
		_InsideColor("Inside Color", Color) = (1,1,1,1)
		
		[Toggle(FADE_BORDER)]
		_FadeBorder("Fade Border", Float) = 0
		
		_BorderGradientHardness("Border Hardness", Float) = 50
		[NoScaleOffset]
		_NoiseMap("Noise Map (R)", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 1
		_NoiseThreshold("Threshold", Range(0,1)) = 0.5
	}
    CustomEditor "ClipVolumeUnlitEditor"
	SubShader
	{
	    UsePass "Clip Volume/Standard/RenderBack"
		Tags { "RenderType"="Opaque" }
		LOD 100

        Cull back
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile _ FADE_BORDER
			
			#include "UnityCG.cginc"
			#include "Assets/Shaders/Includes/ClipVolume.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				#if FADE_BORDER
				float4 normal : NORMAL;
				#endif
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				#if FADE_BORDER
				float3 localPos : TEXCOORD2;
				float3 localNormal : TEXCOORD3;
				#endif
			};
					
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			// backface rendering
		    fixed4 _InsideColor;	
        
            // clip volume definition
		    float4 _ClipVolumeMin;
		    float4 _ClipVolumeMax;	
		    float4x4 _ClipVolumeWorldToLocal;
		    float3 _ClipVolumeWorldPos;
		    
		    #if FADE_BORDER
		    // border fading
		    sampler2D _NoiseMap;
		    float _BorderGradientHardness;
            float _NoiseThreshold;
            float _NoiseScale;
            #endif
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#if FADE_BORDER
				o.localPos = v.vertex.xyz;
                o.localNormal = v.normal.xyz;
                #endif
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    // use clip volume
                float outOfBounds = useClipVolume(i.worldPos.xyz, _ClipVolumeWorldToLocal, _ClipVolumeWorldPos.xyz, _ClipVolumeMin.xyz, _ClipVolumeMax.xyz);
                
                #if FADE_BORDER
                // fade border
		        fadeBorder(outOfBounds, _NoiseMap, _NoiseScale, _NoiseThreshold, _BorderGradientHardness, i.localPos, i.localNormal);
		        #endif
                
				fixed4 col = tex2D(_MainTex, i.uv)*_Color;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
