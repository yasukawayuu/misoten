Shader "Sprites/NeonOutLine"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_OutLineSpread("Outline Spread", Range(0, 0.1)) = 0.05
		_GlowColor("Glow Color", Color) = (1,0,0,1)
		_GlowIntensity("Glow Intensity", Range(0, 10)) = 5
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile DUMMY PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex	: SV_POSITION;
					float2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				sampler2D _MainTex;
				fixed4 _Color;
				half _OutLineSpread;
				fixed4 _GlowColor;
				half _GlowIntensity;

				v2f vert(appdata IN)
				{
					float2 texcoord = IN.texcoord;
					float scale = 1.0 + _OutLineSpread * 2.0;

					// アウトライン用のスケールされたUV
					texcoord = texcoord * scale - (_OutLineSpread);

					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = texcoord;
					OUT.color = IN.color;

					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					// メインテクスチャのサンプリング
					fixed4 baseColor = tex2D(_MainTex, IN.texcoord);

				// 発光アウトラインの処理
				fixed4 glowColor = 0;

				half2 offsets[4] = {
					half2(_OutLineSpread, 0),  // 右
					half2(-_OutLineSpread, 0), // 左
					half2(0, _OutLineSpread),  // 上
					half2(0, -_OutLineSpread)  // 下
				};

				for (int i = 0; i < 4; i++)
				{
					glowColor += tex2D(_MainTex, IN.texcoord + offsets[i]);
				}
				glowColor /= 4; // 平均化

				// 発光部分の色設定
				fixed4 outlineGlow = _GlowColor * _GlowIntensity * glowColor.a;

				// スプライト本体は非表示にし、アウトラインのみを描画
				return outlineGlow;
			}
		ENDCG
		}
		}
}