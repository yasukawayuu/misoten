Shader "Unlit/2DDepthOutline"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineThickness("Outline Thickness", Range(1, 10)) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
			LOD 100

			Pass
			{
				Tags { "LightMode" = "ForwardBase" }
				ZWrite Off
				ZTest Always
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _OutlineColor;
				float _OutlineThickness;

				// カメラの深度テクスチャ
				sampler2D _CameraDepthTexture;

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 pos : SV_POSITION;
					float4 screenPos : TEXCOORD1;
				};

				v2f vert(appdata_t v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.screenPos = ComputeScreenPos(o.pos);
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					// メインテクスチャの色を取得
					float4 mainColor = tex2D(_MainTex, i.uv);

					// 深度テクスチャの座標
					float2 screenUV = i.screenPos.xy / i.screenPos.w;

					// 深度値を取得
					float depth = tex2D(_CameraDepthTexture, screenUV).r;

					// アウトラインの幅を調整
					float outline = 0.0;
					for (int x = -1; x <= 1; x++)
					{
						for (int y = -1; y <= 1; y++)
						{
							float2 offset = float2(x, y) * _OutlineThickness / _ScreenParams.xy;
							float neighborDepth = tex2D(_CameraDepthTexture, screenUV + offset).r;

							// 隣接ピクセルと深度の差を比較
							if (abs(depth - neighborDepth) > 0.01)
							{
								outline = 1.0;
							}
						}
					}

					// アウトラインの描画
					if (outline > 0.0)
					{
						return _OutlineColor;
					}

					// 通常のテクスチャを描画
					return mainColor;
				}
				ENDCG
			}
		}
			FallBack "Diffuse"
}
