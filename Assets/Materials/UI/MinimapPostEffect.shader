Shader "Hidden/MinimapPostEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float4 frag (v2f t) : SV_Target
            {
                // 循环变量.
                int i;
                
                // 超采样抗锯齿.
                float2 uv = t.uv;
                float2 d = _MainTex_TexelSize.xy;
                float4 colors[5] = {
                    tex2D(_MainTex, uv),
                    tex2D(_MainTex, uv + float2(d.x, 0)),
                    tex2D(_MainTex, uv + float2(-d.x, 0)),
                    tex2D(_MainTex, uv + float2(0, d.y)),
                    tex2D(_MainTex, uv + float2(0, -d.y)),
                };
                
                // 取 alpha 最大的那个值作为它的颜色.
                // 如果有多个最大的, 则把颜色混合起来, 透明度不变.
                float maxAlpha = colors[0].a;
                for(i=1; i<5; i++) maxAlpha = max(maxAlpha, colors[i].a);
                float4 color = float4(0, 0, 0, 0);
                int colorCount = 0;
                for(i=0; i<5; i++) if(maxAlpha - colors[i].a <= 0.0001)
                {
                    colorCount += 1;
                    color += colors[i];
                }
                color /= colorCount;
                
                // 透明度加权平均.
                float alpha = colors[0].a * 0.5;
                for(i=1; i<5; i++) alpha += colors[i].a * 0.125;
                
                return float4(color.rgb, alpha);
            }
            ENDCG
        }
    }
}
