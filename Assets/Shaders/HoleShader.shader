Shader "Custom/HoleShader"
{
    Properties
    {
        _Color("Color Tint", Color) = (1,1,1,1)
        _Center("Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _Radius("Radius", Float) = 0.25
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanvasOverlay"="True" // UI에서 사용 시
        }
        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
            };

            float4 _Color;
            float4 _Center;   // xy좌표로 사용
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 현재 픽셀의 UV(0~1)
                float2 uv = i.uv;

                // 중심점(Center)와의 거리 계산
                float dist = distance(uv, _Center.xy);

                // dist < _Radius 이면 구멍(투명), 아니면 원래 color
                float alpha = dist > _Radius ? _Color.a : 0.0;

                return float4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}