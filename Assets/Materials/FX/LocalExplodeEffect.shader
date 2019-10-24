Shader "Hidden/LocalExplodeEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _CameraViewport ("Camera Vieport", vector) = (0, 0, 0, 0)
        _Center ("Center", vector) = (0, 0, 0, 0)
        _MaxRadius ("Max Radius", float) = 1
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
            #include "../Utils/Utils.cginc"

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
            float2 _CameraViewport;
            float2 _Center;
            float _MaxRadius;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);
                if(length(UVToWorld(_CameraViewport, i.uv) - _Center) <= _MaxRadius) return float4(float3(1, 1, 1) - color.rgb, color.a);
                return color;
            }
            ENDCG
        }
    }
}
