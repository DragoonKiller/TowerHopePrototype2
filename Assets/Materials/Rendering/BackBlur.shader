Shader "Hidden/BackBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag(v2f t) : SV_Target
            {
                float dimKernel[6] = { 16, 13, 10, 6, 4, 2 };
                
                float sum = 0;
                for(int i = -5; i <= 5; i++) for(int j = -5; j <= 5; j++) sum += dimKernel[abs(i)] * dimKernel[abs(j)];
                
                float4 color = float4(0, 0, 0, 0);
                for(int dx = -5; dx <= 5; dx++) for(int dy = -5; dy <= 5; dy++)
                {
                    float weight = dimKernel[abs(dx)] * dimKernel[abs(dy)];
                    float2 samplePoint = t.uv + float2(dx * 2 - sign(dx) * 0.5, dy * 2 - sign(dy) * 0.5) * _MainTex_TexelSize.xy;
                    color += weight * tex2D(_MainTex, samplePoint);
                }
                color /= sum;
                
                return color;
            }
            ENDCG
        }
    }
}
