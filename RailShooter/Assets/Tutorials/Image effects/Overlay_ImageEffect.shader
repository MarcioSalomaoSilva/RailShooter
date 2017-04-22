Shader "Hidden/Overlay_ImageEffect"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DeltaX("Delta X", Float) = 0.01
		_DeltaY("Delta Y", Float) = 0.01
		_LuminosityAmount("GrayScale", Range(0,1)) = 1
		_BrightnessAmount("Brightness", Range(0.0, 1)) = 1.0
		_SatAmount("Saturation", Range(0, 1)) = 1.0
		_ConAmount("Contrast", Range(0, 1)) = 1.0
		_DepthPower ("Depth Power", Range(1,5)) = 1
		_OverlayTex("Overlay Texture", 2D) = "white" {}
		_OverlayOpacity("Overlay Opacity", Range(0, 1)) = 1
		_BlendTex("Blend Texture", 2D) = "white" {}
		_BlendOpacity("Blend Opacity", Range(0, 1)) = 1
	}
		SubShader
		{
			Pass
			{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			sampler2D _MainTex, _BlendTex, _OverlayTex;
			float _DeltaX;
			float _DeltaY;
			fixed _OverlayOpacity;
			fixed _BlendOpacity;
			fixed _LuminosityAmount;
			fixed _BrightnessAmount;
			fixed _SatAmount;
			fixed _ConAmount;
			fixed _DepthPower;
			sampler2D _CameraDepthTexture;
			sampler2D _CameraDepthNormalsTexture;
			//
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};
			float sobel(sampler2D tex, float2 uv) {
				float2 delta = float2(_DeltaX, _DeltaY);
				//
				float4 hr = float4(0, 0, 0, 0);
				float4 vt = float4(0, 0, 0, 0);
				//
				hr += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
				hr += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) * -1.0;
				hr += tex2D(tex, (uv + float2(-1.0, 0.0) * delta)) *  2.0;
				hr += tex2D(tex, (uv + float2(0.0, 0.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, 0.0) * delta)) * -2.0;
				hr += tex2D(tex, (uv + float2(-1.0, 1.0) * delta)) *  1.0;
				hr += tex2D(tex, (uv + float2(0.0, 1.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, 1.0) * delta)) * -1.0;
				//
				vt += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
				vt += tex2D(tex, (uv + float2(0.0, -1.0) * delta)) *  2.0;
				vt += tex2D(tex, (uv + float2(1.0, -1.0) * delta)) *  1.0;
				vt += tex2D(tex, (uv + float2(-1.0, 0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(0.0, 0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(1.0, 0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(-1.0, 1.0) * delta)) * -1.0;
				vt += tex2D(tex, (uv + float2(0.0, 1.0) * delta)) * -2.0;
				vt += tex2D(tex, (uv + float2(1.0, 1.0) * delta)) * -1.0;
				//
				return sqrt(hr * hr + vt * vt);
			}
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
			fixed OverlayBlendMode(fixed basePixel, fixed blendPixel) {
				if (basePixel) {
					return (2 * basePixel*blendPixel);
				}
				else {
					return (1 - 2 * (1 - basePixel)*(1 - blendPixel));
				}
			}
			fixed4 frag(v2f_img i) : COLOR {
				// get colors from render texture and uvs from struct
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 overlayTex = tex2D(_OverlayTex, i.uv);
				fixed4 blendTex = tex2D(_BlendTex, i.uv);
				//perform multiply blend
				//fixed4 blendMultiply = renderTex * blendTex;
				//fixed4 blendAdd =lerp(renderTex, renderTex + blendTex, _BlendOpacity);
				//fixed4 blendScreen = (1 - (1 - renderTex) * (1 - blendTex));
				//
				fixed4 overlayImage = renderTex;
				overlayImage.r = OverlayBlendMode(renderTex.r, overlayTex.r);
				overlayImage.g = OverlayBlendMode(renderTex.g, overlayTex.g);
				overlayImage.b = OverlayBlendMode(renderTex.b, overlayTex.b);
				renderTex = lerp(renderTex, overlayImage, _OverlayOpacity);
				//
				renderTex = lerp(renderTex, renderTex + blendTex, _BlendOpacity);
				//depth
				//float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv.xy));
				//depth = pow(Linear01Depth(depth), _DepthPower);
				//depth & normals
				float3 normalValues;
				float depthValue;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv.xy), depthValue, normalValues);
				depthValue = pow(Linear01Depth(depthValue), _DepthPower);
				float4 normalColor = float4(normalValues, 1);
				fixed4 color;
				//color.r = (normalColor.r + depthValue) * 0.5;
				//color.g = (normalColor.g + depthValue) * 0.5;
				//color.b = (normalColor.b + depthValue) * 0.5;
				color.r = normalColor.x;
				color.g = normalColor.y;
				color.b = depthValue;
				color.a = 1;
				//
				renderTex.rgb = ContrastSaturationBrighhtness(renderTex.rgb, _BrightnessAmount, _SatAmount, _ConAmount);
				//
				float s = sobel(_CameraDepthNormalsTexture, i.uv);
				//
				return color;//float4(normalValues,1);//float4(s, s, s, 1);//renderTex;
			}
			ENDCG
		}
	}
}