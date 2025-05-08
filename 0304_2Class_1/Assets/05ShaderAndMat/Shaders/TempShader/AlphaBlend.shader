Shader "Unlit/AlphaBlend"
{
   Properties // ���׸��� ���� �� ����
    {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _Alpha ("Float", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" } // transparent : ������ �Ϳ� �׸���. | opaque : ������

        Pass // ���� ������ ���ִ� ��
        {
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // ���⿡ �� �������

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float _Alpha;
            CBUFFER_END


            struct Attributes
            {
                float4 positionOS   : POSITION;  //��ġ(������Ʈ�����̽�)
                float2 texcoord     : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;  //ȣ�����Ͼ Ŭ�������̽�
                float4 positionOS   : TEXCOORD1;  //��ġ
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
