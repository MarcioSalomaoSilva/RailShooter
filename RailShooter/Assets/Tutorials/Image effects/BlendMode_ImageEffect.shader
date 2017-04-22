Shader "Hidden/BlendMode_ImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlendTex ("Blend Texture", 2D) = "white" {}
		_Opacity ("Blend Opacity", Range(0, 1)) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
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
			sampler2D _MainTex, _BlendTex;
			fixed _Opacity;
			fixed4 frag (v2f i) : SV_Target {
				// get colors from render texture and uvs from struct
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 blendTex = tex2D(_BlendTex, i.uv);
				//perform multiply blend
				fixed4 blenderMultiply = renderTex * blendTex;
				//adjust te amount of blend with lerp
				renderTex = lerp(renderTex, blenderMultiply, _Opacity);
				// just invert the colors
				//col = 1 - col;
				return renderTex;
			}
			ENDCG
		}
	}
}
