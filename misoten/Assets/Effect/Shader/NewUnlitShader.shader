Shader "Custom/TransparentUnlit"
{
	Properties
	{
		_MainTex("Base Texture", 2D) = "white" { }
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }

			Pass
			{
			// 透明度のブレンド
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma surface surf unlit
			#include "UnityCG.cginc"

			struct Input
			{
				float2 uv_MainTex;
			};

			sampler2D _MainTex;
			float4 _Color;

			void surf(Input IN, inout SurfaceOutput o)
			{
				// テクスチャと色を適用
				float4 texColor = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = texColor.rgb * _Color.rgb;
				o.Alpha = texColor.a * _Color.a;
			}
			ENDCG
		}
		}
			FallBack "Unlit/Color"
}
