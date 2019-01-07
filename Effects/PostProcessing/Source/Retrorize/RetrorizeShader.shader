/*
* A Image Effect shader that Quantizes the color of the Rendertexture and reduces its resoultion
*
* v1.0 06/2018
* Written by Fabian Kober
* fabian-kober@gmx.net
*/
Shader "Hidden/Custom/Retrorize"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Bits;
        float2 _Resolution;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            // reduce resolution if resolution has a valid value
            float2 texcoord = i.texcoord;
            if(_Resolution.x > 0 && _Resolution.y > 0)
                texcoord = float2(floor(i.texcoord.x*_Resolution.x)/_Resolution.x, floor(i.texcoord.y*_Resolution.y)/_Resolution.y);
                
            // get color of pixel
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, texcoord);
            
            // quantize color if the bits have a valid value
            if(_Bits > 0) {
                float val = _Bits/4;
                color.r = round(val * color.r)/val;
                color.g = round(val * color.g)/val;
                color.b = round(val * color.b)/val;
                color.a = round(val * color.a)/val;
            }
            return color;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
