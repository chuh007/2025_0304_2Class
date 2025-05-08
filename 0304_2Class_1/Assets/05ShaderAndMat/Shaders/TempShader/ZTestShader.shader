Shader "Unlit/ZTestShader"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ZTest Less

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
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
                OUT.uv = IN.texcoord;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {                
                
                return _BaseColor;
            }
            ENDHLSL
        }
       
    }

}
