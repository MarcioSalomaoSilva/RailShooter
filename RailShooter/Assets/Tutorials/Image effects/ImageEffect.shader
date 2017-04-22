Shader "Hidden/ImageEffect"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LuminosityAmount ("GrayScale", Range(0,1)) = 1
		_BrightnessAmount ("Brightness", Range(0.0, 1)) = 1.0
		_SatAmount ("Saturation", Range(0, 1)) = 1.0
		_ConAmount ("Contrast", Range(0, 1)) = 1.0
		//_DepthPower ("Depth Power", Range(1,5)) = 1
		_BlendTex("Blend Texture", 2D) = "white" {}
		_Opacity("Blend Opacity", Range(0, 1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			sampler2D _MainTex, _BlendTex;
			fixed _Opacity;
			fixed _LuminosityAmount;
			fixed _BrightnessAmount;
			fixed _SatAmount;
			fixed _ConAmount;
			fixed _DepthPower;
			sampler2D _CameraDepthTexture;
			//
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
			float3 ContrastSaturationBrighhtness(float3 color, float brt, float sat, float con) {
				//Increase or Decrease these values to adjust rgb color channels separately
				float AvgLumR = 0.5;
				float AvgLumG = 0.5;
				float AvgLumB = 0.5;
				//Luminance coefficients for getting luminance from the image
				float3 LuminanceCoeff = float3(0.2125, 0.7154, 0.0721);
				//operation for brightness
				float3 AvgLumin = float3(AvgLumR, AvgLumG, AvgLumB);
				float3 brtColor = color * brt;
				float intensityf = dot(brtColor, LuminanceCoeff);
				float3 intensity = float3(intensityf, intensityf, intensityf);
				//Operation for saturation
				float3 satColor = lerp(intensityf, brtColor, sat);
				//operation for contrast
				float3 conColor = lerp(AvgLumin, satColor, con);
				return conColor;
			}
			fixed4 frag (v2f_img i) : COLOR
			{
				// get colors from render texture and uvs from struct
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 blendTex = tex2D(_BlendTex, i.uv);
				//perform multiply blend
				//fixed4 blendMultiply = renderTex * blendTex;
				//fixed4 blendAdd = renderTex + blendTex;
				fixed4 blendScreen = (1 - (1 - renderTex) * (1-blendTex));
				//adjust te amount of blend with lerp
				renderTex = lerp(renderTex, blendScreen, _Opacity);
				//depth
				float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv.xy));
				depth = pow(Linear01Depth(depth), _DepthPower);
				//
				renderTex.rgb = ContrastSaturationBrighhtness(renderTex.rgb, _BrightnessAmount, _SatAmount, _ConAmount);
				//
				return renderTex;
			}
			ENDCG
		}
	}
}
