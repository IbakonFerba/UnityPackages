/*
* This Shader is an unlit shader that is only visible inside a Clip Volume and Fades using Dithering towards the Border of the Clip Volume.
* Backface Culling is turned of so that the object looks solid (from most angles)
*
* v1.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Clip Volume/Fade Border Unlit"
{
	Properties
	{
	    _Color ("Color", Color) = (1,1,1,1)
		_ClipVolumeMin("Min XYZ", Vector) = (0,0,0)	
		_ClipVolumeMax("Max XYZ", Vector) = (1,1,1)
		_BorderGradientHardness("Border Hardness", Float) = 50
		[NoScaleOffset]
		_NoiseMap("Noise Map (R)", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 1
		_NoiseThreshold("Threshold", Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

        Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Assets/Shaders/Includes/ClipVolume.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float3 localPos : TEXCOORD1;
				float3 localNormal : TEXCOORD2;
			};
			
			sampler2D _NoiseMap;
					
			fixed4 _Color;
			
			// backface rendering
		    fixed4 _InsideColor;	
        
            // clip volume definition
		    float4 _ClipVolumeMin;
		    float4 _ClipVolumeMax;	
		    float4x4 _ClipVolumeWorldToLocal;
		    float3 _ClipVolumeWorldPos;
		    
		    // border fading
		    float _BorderGradientHardness;
            float _NoiseThreshold;
            float _NoiseScale;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.localPos = v.vertex.xyz;
                o.localNormal = v.normal.xyz;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    // use clip volume
                float outOfBounds = useClipVolume(i.worldPos.xyz, _ClipVolumeWorldToLocal, _ClipVolumeWorldPos.xyz, _ClipVolumeMin.xyz, _ClipVolumeMax.xyz);
                
                // fade border
		        fadeBorder(outOfBounds, _NoiseMap, _NoiseScale, _NoiseThreshold, _BorderGradientHardness, i.localPos, i.localNormal);
                
				fixed4 col = _Color;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
