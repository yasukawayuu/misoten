Shader "Custom/2DGradation" {
    Properties {
        _BeforeColor("Before Color", Color) = (0, 0, 1, 1)
        _AfterColor("After Color", Color) = (1, 0, 0, 1)
        _BeforeColorAmount("Before Color Amount", Range(-1, 1)) = -1
    }

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _BeforeColor;
            fixed4 _AfterColor;
            fixed _BeforeColorAmount;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // グラデーションの量を計算
                fixed amount = clamp(i.uv.y + _BeforeColorAmount, 0, 1.0);
                // グラデーションの色を補間
                fixed4 color = lerp(_AfterColor, _BeforeColor, amount);
                return color;
            }
            ENDCG
        }
    }
}
