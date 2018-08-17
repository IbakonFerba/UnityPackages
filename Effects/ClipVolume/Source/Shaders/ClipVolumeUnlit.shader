/*
* This Shader is an unlit shader that is only visible inside a Clip Volume.
* Backface Culling is turned of so that the object looks solid (from most angles)
*
* v1.0 08/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Clip Volume/Unlit"
{
	Properties
	{
	    _Color ("Color", Color) = (1,1,1,1)
		_ClipVolumeMin("Min XYZ", Vector) = (0,0,0)	
		_ClipVolumeMax("Max XYZ", Vector) = (1,1,1)
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
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};
			
			fixed4 _Color;
			
			// backface rendering
		    fixed4 _InsideColor;	
        
            // clip volume definition
		    float4 _ClipVolumeMin;
		    float4 _ClipVolumeMax;	
		    float4x4 _ClipVolumeWorldToLocal;
		    float3 _ClipVolumeWorldPos;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    // use clip volume
                useClipVolume(i.worldPos.xyz, _ClipVolumeWorldToLocal, _ClipVolumeWorldPos.xyz, _ClipVolumeMin.xyz, _ClipVolumeMax.xyz);
                
				fixed4 col = _Color;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
