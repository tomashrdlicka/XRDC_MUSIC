Shader "Custom/RandomMorph"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MorphAmount ("Morph Amount", Range(0, 1)) = 0.5
        _TimeSpeed ("Time Speed", Range(0, 5)) = 1.0
        _Transparency ("Transparency", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _MorphAmount;
            float _TimeSpeed;
            float _Transparency;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float random(float3 seed, float freq)
            {
                return frac(sin(dot(seed, float3(12.9898, 78.233, 45.164))) * 43758.5453 * freq);
            }

            v2f vert (appdata v)
            {
                v2f o;
                float3 pos = v.vertex.xyz;

                // Generate random offsets based on the vertex position and time
                float time = _TimeSpeed * _Time.y;
                float offsetX = random(pos + time, 1.0) * _MorphAmount;
                float offsetY = random(pos + time * 2.0, 1.0) * _MorphAmount;
                float offsetZ = random(pos + time * 3.0, 1.0) * _MorphAmount;

                // Apply the morphing to the vertex position
                pos += float3(offsetX, offsetY, offsetZ);

                o.pos = UnityObjectToClipPos(float4(pos, 1.0));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Apply transparency
                col.a *= _Transparency;

                return col;
            }
            ENDCG
        }
    }
}