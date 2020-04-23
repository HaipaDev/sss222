// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/QuickBlur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BlendTex("Blend", 2D) = "white" {}
		_Intensity("Intensity", float) = 0
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
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _BlendTex;
			float _Intensity;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					v.uv.y = 1 - v.uv.y;
				#endif

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 blend = tex2D(_BlendTex, i.uv);
				fixed4 col = tex2D(_MainTex, i.uv) + blend * _Intensity;

				return col;
			}
			ENDCG
		}
	}
}
