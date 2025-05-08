Shader "Unlit/AlphaBlend"
{
   Properties // 메테리얼에 들어가는 값 세팅
    {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _Alpha ("Float", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" } // transparent : 투명한 것에 그린다. | opaque : 불투명

        Pass // 실제 수행을 해주는 것
        {
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // 여기에 다 들어있음

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float _Alpha;
            CBUFFER_END


            struct Attributes
            {
                float4 positionOS   : POSITION;  //위치(오브젝트스페이스)
                float2 texcoord     : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;  //호모지니어스 클립스페이스
                float4 positionOS   : TEXCOORD1;  //위치
                float2 uv           : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionOS = IN.positionOS;
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _BaseMap);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {                
                half4 texel = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgba;
                
                //return float4(texel, _Alpha);
                return texel;
            }
            ENDHLSL
        }
       
    }

}
