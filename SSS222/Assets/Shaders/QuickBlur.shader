// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/QuickBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Size ("Blur Size", float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 halfTexel : HALFTEXEL;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Size;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.halfTexel = _MainTex_TexelSize.xy * 0.5 * _Size;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col =
					tex2D(_MainTex, i.uv + float2(i.halfTexel.x, i.halfTexel.y)) * 0.25 +
					tex2D(_MainTex, i.uv + float2(-i.halfTexel.x, i.halfTexel.y)) * 0.25 +
					tex2D(_MainTex, i.uv + float2(i.halfTexel.x, -i.halfTexel.y)) * 0.25 +
					tex2D(_MainTex, i.uv + float2(-i.halfTexel.x, -i.halfTexel.y)) * 0.25;

				return col;
			}
			ENDCG
		}
	}
}
