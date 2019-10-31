Shader "Hidden/UVRescale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _UVScale ("UV Scale", vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        
        Tags { "Queue" = "Transparent" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "./Utils.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR0;
            };
            
            sampler2D _MainTex;
            float2 _UVScale;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 scaledSize = float2(1, 1) / _UVScale;
                float2 uv = float2(
                    (i.uv.x - 0.5) / _UVScale.x + 0.5,
                    xmap(i.uv.y, 0, 1, 1 - _UVScale.y, 1)
                );
                float4 color = tex2D(_MainTex, uv) * i.color;
                return color;
            }
            ENDCG
        }
    }
}
