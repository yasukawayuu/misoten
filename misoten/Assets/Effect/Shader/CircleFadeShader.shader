Shader "Custom/CircleFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}      // テクスチャ
        _MaskProgress ("Mask Progress", Range(0, 1)) = 0.5    // 円形マスクの大きさを制御
        _FadeColor ("Fade Color", Color) = (0, 0, 0, 1)      // 背景色
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;    // 頂点座標
                float2 uv : TEXCOORD0;      // UV座標
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;      // UV座標
                float4 vertex : SV_POSITION; // クリップ空間の頂点座標
            };

            sampler2D _MainTex;            // メインテクスチャ
            float4 _MainTex_ST;           // テクスチャのUV変換用データ
            float _MaskProgress;          // マスクの進行度
            float4 _FadeColor;            // フェードアウトの背景色

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // オブジェクト空間からクリップ空間に変換
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; // UV座標を変換
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);  // 円の中心位置
                float dist = distance(i.uv, center);  // 現在のピクセルから円の中心までの距離を計算

                // 距離がマスクの進行度より小さい場合は透明
                if (dist < _MaskProgress)
                {
                    // 円形内部領域を透明に設定
                    return float4(0, 0, 0, 0);  // 完全に透明
                }
                else
                {
                    // 円形外部領域に背景色を表示
                    return _FadeColor;
                }
            }
            ENDCG
        }
    }
}
