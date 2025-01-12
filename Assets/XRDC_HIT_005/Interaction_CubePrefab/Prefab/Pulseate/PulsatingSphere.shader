Shader "Custom/PulsatingScaleFade"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1) // Base color
        _BPM("BPM", Float) = 120 // Beats per minute
        _ScaleIntensity("Scale Intensity", Float) = 1 // Maximum scale intensity
        _FadeMultiplier("Fade Multiplier", Float) = 1 // Fade intensity
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency
            ZWrite Off // Disable depth writing for transparent objects

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            float4 _BaseColor;
            float _BPM;
            float _ScaleIntensity;
            float _FadeMultiplier;

            // Vertex input/output structure
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // Homogeneous clip space position
                float3 positionWS : TEXCOORD0;   // World position
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);

                // Pulsation scale factor (0 to 1 based on sine wave)
                float timePerBeat = 60.0 / _BPM;
                float pulsation = sin(_Time.y * 3.14159 * 2.0 / timePerBeat) * 0.5 + 0.5;

                // Scale the object
                float scale = lerp(0, _ScaleIntensity, pulsation);
                float4 scaledPos = float4(input.positionOS.xyz * scale, 1);

                output.positionHCS = TransformObjectToHClip(scaledPos);
                return output;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                // Pulsation based on sine wave
                float timePerBeat = 60.0 / _BPM;
                float pulsation = sin(_Time.y * 3.14159 * 2.0 / timePerBeat) * 0.5 + 0.5;

                // Fade out as scale increases
                float fade = saturate(1.0 - pulsation * _FadeMultiplier);

                // Return the final color with fading alpha
                return float4(_BaseColor.rgb, _BaseColor.a * fade);
            }
            ENDHLSL
        }
    }
}