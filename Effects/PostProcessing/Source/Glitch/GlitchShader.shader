/*
*   A Post Processing Effect that creates glitchy screen tearing
*   
*   Written by Fabian Kober
*   v1.0 01/2019
*   fabian-kober@gmx.net
*/
Shader "Hidden/Custom/Glitch"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        
        float _Size;
        float _Strength;
        float _StrengthRandomness;
        float _Amount;
        
        float rand(float pos) {
            return frac(sin(pos)*1000000.0);
        }

        float4 Frag(VaryingsDefault i) : SV_Target
        {               
            // calculate some kind of randomized Square Wave
            float square = sign(sin(i.texcoord.y*rand(_Time.z)*_Size*sin((_Time.x+i.texcoord.y)*10)));
            
            // get the texcoord that is transformed in x direction
            float2 texcoord = float2(i.texcoord.x+square*(1-rand(_Time.y)*_StrengthRandomness)*_Strength*0.01, i.texcoord.y);
            
            // only use the transformed coordinate sometimes
            if(rand(_Time.x) >= _Amount)
                texcoord = i.texcoord;
        
            // get color of pixel
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, texcoord);

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
