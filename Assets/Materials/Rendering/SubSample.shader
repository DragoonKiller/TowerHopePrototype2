Shader "Hidden/SubSample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _SubSamplePivot ("SubSample Pivot", vector) = (0.5, 0.5, 0, 0)
        _SubSampleRate ("SubSample Rate", float) = 1
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

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
            
            // 采样矩形的中心在原图的哪个位置.
            float2 _SubSamplePivot;
            
            // 最终只采样原图大小的多少倍.
            float _SubSampleRate;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // 做一个手动的狗牙消除...
                // 要求贴图的过滤方式不是 Point.
                float2 sampleCenter = (i.uv - _SubSamplePivot) * _SubSampleRate + _SubSamplePivot;
                float4 color = 0.25 * (
                    tex2D(_MainTex, sampleCenter + float2(0.5, 0.5) * _MainTex_TexelSize.xy)
                    + tex2D(_MainTex, sampleCenter + float2(-0.5, 0.5) * _MainTex_TexelSize.xy)
                    + tex2D(_MainTex, sampleCenter + float2(0.5, -0.5) * _MainTex_TexelSize.xy)
                    + tex2D(_MainTex, sampleCenter + float2(-0.5, -0.5) * _MainTex_TexelSize.xy)
                );
                
                return color;
            }
            ENDCG
        }
    }
}
