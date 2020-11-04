﻿Shader "Custom/NewImageEffectShader"
{
	Properties
	{
		_Color_Main("Main Color", Color) = (1,1,1,1)
		_Color_Highlight("Highlight Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull front
		LOD 100
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


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 _Color_Main;
			fixed4 _Color_Highlight;

			fixed4 frag(v2f i) : SV_Target
			{
				if (i.uv.x < 0.10 || i.uv.x > 0.90) {
					return _Color_Highlight;
				}
				if (i.uv.y < 0.10 || i.uv.y > 0.90) {
					return _Color_Highlight;
				}
				return _Color_Main;
			}
			ENDCG
		}
	}
}
