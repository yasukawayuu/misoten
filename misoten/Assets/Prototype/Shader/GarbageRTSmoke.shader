Shader "Metaball/GarbageRTSmoke" {
Properties
{
    _MainTex ("MainTex", 2D) = "white" {}
}

SubShader
{
    Tags
    {
        "Queue"="Transparent"
        "IgnoreProjector"="True"
        "RenderType"="Transparent"
        // "PreviewType"="Plane"
    }

    Cull Off//裏も描画する
    // Lighting Off
    ZWrite Off //transparentで使用する部分
    Blend One SrcAlpha //Blend One OneMinusSrcAlpha

    Pass
    {
    CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fog

        #include "UnityCG.cginc"

        struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
        };


        sampler2D _MainTex;

        v2f vert(appdata_t IN)
        {
            v2f OUT;
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.texcoord = IN.texcoord;
            OUT.color = IN.color;
            return OUT;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 color = tex2D(_MainTex, i.texcoord);

            // 最終的な色
            return color;
        }


    ENDCG
    }
}
}
