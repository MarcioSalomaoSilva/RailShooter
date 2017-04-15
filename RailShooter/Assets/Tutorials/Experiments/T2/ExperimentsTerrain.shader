// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Experiments/Terrain" {
	Properties{
		_Tint("Tint", Color) = (1,1,1,1)
		_TintIntensity("Tint Strength", Range(0, 1)) = 0.5
		_MainTex ("SplatMap", 2D) = "white" {}
		[NoScaleOffset] _RampTex("Ramp Texture (for Gradient)", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex("Noise Texture (For Blending)", 2D) = "white" {}
		//
		_TexIntensity1("Albedo 1 Strength", Range(0, 1)) = 0.5
		_TopColor1("Opaque Color 1", Color) = (1,1,1,1)
		_BottomColor1("Transparent Color 1", Color) = (1,1,1,1)
		[NoScaleOffset] _Texture1("Albedo 1", 2D) = "white" {}
		//
		_TexIntensity2("Albedo 2 Strength", Range(0, 1)) = 0.5
		_TopColor2("Opaque Color", Color) = (1,1,1,1)
		_BottomColor2("Transparent Color", Color) = (1,1,1,1)
		[NoScaleOffset] _Texture2("Albedo 2", 2D) = "white" {}
		//
		_TexIntensity3("Albedo 3 Strength", Range(0, 1)) = 0.5
		_TopColor3("Opaque Color", Color) = (1,1,1,1)
		_BottomColor3("Transparent Color", Color) = (1,1,1,1)
		[NoScaleOffset] _Texture3("Albedo 3", 2D) = "white" {}
		//
		_TexIntensity4("Albedo 4 Strength", Range(0, 1)) = 0.5
		_TopColor4("Opaque Color", Color) = (1,1,1,1)
		_BottomColor4("Transparent Color", Color) = (1,1,1,1)
		[NoScaleOffset] _Texture4("Albedo 4", 2D) = "white" {}
		//
		[NoScaleOffset] _NoiseTex("Noise Texture (For Blending)", 2D) = "white" {}
	}
		SubShader{
			Pass {
				CGPROGRAM
				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram
				#include "UnityCG.cginc"
				float4 _Tint;
				float  _TintIntensity;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _Texture1, _Texture2, _Texture3, _Texture4;
				float4 _BottomColor1; 
				float4 _BottomColor2; 
				float4 _BottomColor3; 
				float4 _BottomColor4;
				float4 _TopColor1; 
				float4 _TopColor2; 
				float4 _TopColor3; 
				float4 _TopColor4;
				float  _TexIntensity1; 
				float  _TexIntensity2; 
				float  _TexIntensity3; 
				float  _TexIntensity4;
				sampler2D _RampTex;
				sampler2D _NoiseTex;
			struct Interpolators {
				float4 position			: SV_POSITION;
				float2 uv				: TEXCOORD0;
				float2 uvSplat			: TEXCOORD1;
			};
			struct VertexData {
				float4 position			: POSITION;
				float2 uv				: TEXCOORD0;
			};
			Interpolators MyVertexProgram(VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);;
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				i.uvSplat = v.uv;
				return i;
			}
			float3 getNoise(float2 uv)
			{
				float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
				noise = mad(noise, 2.0, -0.5);
				return noise / 255;
			}
			float4 MyFragmentProgram (Interpolators i) : SV_TARGET{
				//
				half4 ramp = tex2D(_RampTex, i.uv).a;
				//
				half4 color1 = lerp(_BottomColor1, _TopColor1, ramp);
				color1.rgb += getNoise(i.uv);
				half4 color2 = lerp(_BottomColor2, _TopColor2, ramp);
				color2.rgb += getNoise(i.uv);
				half4 color3 = lerp(_BottomColor3, _TopColor3, ramp);
				color3.rgb += getNoise(i.uv);
				half4 color4 = lerp(_BottomColor4, _TopColor4, ramp);
				color4.rgb += getNoise(i.uv);
				//uses the tint intensity property to change the tint strength. Lerps between white (invsible) to the tint color
				float4 tint = lerp(float4(1,1,1,1), _Tint, _TintIntensity);
				//uses the tint intensity property to change the tint strength. Lerps between white (invsible) to the tint color
				float4 texture1 = lerp(float4(1, 1, 1, 1), tex2D(_Texture1, i.uv).rgba, _TexIntensity1) * color1;
				float4 texture2 = lerp(float4(1, 1, 1, 1), tex2D(_Texture2, i.uv).rgba, _TexIntensity2) * color2;
				float4 texture3 = lerp(float4(1, 1, 1, 1), tex2D(_Texture3, i.uv).rgba, _TexIntensity3) * color3;
				float4 texture4 = lerp(float4(1, 1, 1, 1), tex2D(_Texture4, i.uv).rgba, _TexIntensity4) * color4;
				//
				float4 splat = tex2D(_MainTex, i.uvSplat);
				return
					(texture1 * splat.r +
					texture2 * splat.g +
					texture3 * splat.b +
					texture4 * (1 - splat.r - splat.g - splat.b)) * tint;
			}
			ENDCG
		}
	}
}
