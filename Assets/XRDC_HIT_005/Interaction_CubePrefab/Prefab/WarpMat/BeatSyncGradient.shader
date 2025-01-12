Shader "Custom/BeatSyncGradient"
{
    Properties
    {
        _Amplitude ("Displacement Amplitude", Float) = 1.0
        _TimeMultiplier ("Time Multiplier", Float) = 1.0
        _ColorGradient ("Color Gradient", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _ColorGradient;
            float _Amplitude;
            float _TimeMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                float time = _Time.y * _TimeMultiplier;

                // Randomized displacement using noise
                float noise = frac(sin(dot(v.vertex.xy, float2(12.9898, 78.233))) * 43758.5453); // Random seed
                float displacement = noise * sin(time) * _Amplitude;

                // Apply displacement along vertex normal
                float3 offset = normalize(v.vertex.xyz) * displacement;
                o.pos = UnityObjectToClipPos(v.vertex + float4(offset, 0));

                // Use UV or vertex position for gradient mapping
                o.uv = v.uv;
                o.color = tex2D(_ColorGradient, float2(v.vertex.y, v.uv.x)); // Gradient based on y-position
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return i.color; // Output the gradient color
            }
            ENDCG
        }
    }
}