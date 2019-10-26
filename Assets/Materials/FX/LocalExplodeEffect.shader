Shader "Hidden/LocalExplodeEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Color ("Color", color) = (1, 1, 1, 1)
        _CameraViewport ("Camera Vieport", vector) = (0, 0, 0, 0)
        _Center ("Center", vector) = (0, 0, 0, 0)
        _TimeOffset ("Time Offset", float) = 0
        _MaxRadius ("Max Radius", float) = 1
        _Deform ("Deform", float) = 1
        _SpaceFrequency ("Space Frequency", float) = 1
        _TimeFrequency ("Time Frequency", float) = 1
        _BiasThreshold ("Bias Threshold", float) = 0
        _Process ("Process", float) = 0
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
            float4 _MainTex_TexelSize;
            
            float4 _Color;
            float2 _CameraViewport;
            float2 _Center;
            float _TimeOffset;
            float _MaxRadius;
            float _Deform;
            float _SpaceFrequency;
            float _TimeFrequency;
            float _Process;
            float _BiasThreshold;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float2 worldPos = UVToWorld(_CameraViewport, i.uv);
                
                float2 bias = float2(
                    perlinNoise(float3(worldPos * _SpaceFrequency, _TimeOffset + pow(_Process, 0.5) * _TimeFrequency), 1), 
                    perlinNoise(float3(worldPos * _SpaceFrequency, _TimeOffset + pow(_Process, 0.5) * _TimeFrequency + 117.2323), 1)
                );
                bias = xmap(bias, 0, 1, -1, 1);
                bias = xmap(sign(bias) * max(0, abs(bias) - float2(_BiasThreshold, _BiasThreshold)), 0, 1 - _BiasThreshold, 0, 1);
                bias *= _Deform * _MainTex_TexelSize.xy;
                float dist = length(worldPos - _Center);
                float radiusRate = clamp(dist / _MaxRadius, 0, 1);
                bias *= pow(radiusRate, 2);
                bias *= 1 - clamp(sign(dist - _MaxRadius), 0, 1);
                
                float4 color = tex2D(_MainTex, i.uv + bias * pow(1 - _Process, 2));
                if(bias.x != 0 || bias.y != 0) color = blendNormal(float4(_Color.rgb, _Color.a * pow(1 - _Process, 2) * (1 - radiusRate)), color);
                return color;
            }
            
            ENDCG
        }
    }
}
