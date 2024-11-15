Shader "Custom/PlayerGradation"{
    Properties {
        _Scale("Scale", Range(0, 0.05)) = 0.01
        _Cutoff("Cutoff", Range(0, 0.5)) = 0.01
        _BeforeColor("Before Color", Color) = (0, 0, 1, 1)
        _AfterColor("After Color", Color) = (1, 0, 0, 1)
        _BeforeColorAmount("Before Color Amount", Range(-1, 1)) = -1
    }

    SubShader {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed _Scale;
            fixed _Cutoff;
            fixed4 _BeforeColor;
            fixed4 _AfterColor;
            fixed _BeforeColorAmount;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // メタボールエフェクト
                fixed2 uv = i.texcoord - 0.5;
                fixed a = 1 / (uv.x * uv.x + uv.y * uv.y);
                a *= _Scale;

                // グラデーションの量を計算
                fixed amount = clamp(i.texcoord.y + _BeforeColorAmount, 0, 1.0);
                
                // グラデーションの色を補間
                fixed4 gradColor = lerp(_AfterColor, _BeforeColor, amount);

                // メタボールエフェクトのアルファ適用
                gradColor.a = a;
                clip(gradColor.a - _Cutoff);

                return gradColor;
            }
            ENDCG
        }
    }
}
