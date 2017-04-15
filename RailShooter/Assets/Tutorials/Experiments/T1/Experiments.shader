// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Experiments/unlit"{
	Properties{
		_Tint("Tint", Color) = (1,1,1,1)
		_TintIntensity("Tint Strength", Range(0, 1)) = 0.5
		_MainTex("Texture", 2D) = "white" {}
		_MainTexIntensity("Texture Strength", Range(0, 1)) = 0.5
		_TopColor("Opaque Color", Color) = (1,1,1,1)
		_BottomColor("Transparent Color", Color) = (1,1,1,1)
		[NoScaleOffset] _RampTex("Ramp Texture (for Gradient)", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex("Noise Texture (For Blending)", 2D) = "white" {}
	}
	SubShader{
		Pass{
			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#include "UnityCG.cginc"
			float4 _Tint;
			float  _TintIntensity;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float  _MainTexIntensity;
			float4 _BottomColor;
			float4 _TopColor;
			sampler2D _RampTex;
			sampler2D _NoiseTex;
			//
			struct Interpolators {
				float4 position			: SV_POSITION;
				float2 uv				: TEXCOORD0;
			};
			struct VertexData {
				float4 position			: POSITION;
				float2 uv				: TEXCOORD0;
			};
			Interpolators MyVertexProgram(VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}
			float3 getNoise(float2 uv)
			{
				float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
				noise = mad(noise, 2.0, -0.5);
				return noise / 255;
			}
			float4 MyFragmentProgram(Interpolators i) : SV_TARGET{
				//uses the ramp textures alpha channel to lerp between two colors and adds the info from the noise textue to avoid banding
				//user control banding and oter effects can be controlled with the ramp texture
				half4 ramp = tex2D(_RampTex, i.uv).a;
				half4 color = lerp(_BottomColor, _TopColor, ramp);
				color.rgb += getNoise(i.uv);
				//uses the tint intensity property to change the tint strength. Lerps between white (invsible) to the tint color
				float4 tint = lerp(float4(1,1,1,1), _Tint, _TintIntensity);
				//uses the tint intensity property to change the tint strength. Lerps between white (invsible) to the tint color
				float4 mainTexture = lerp(float4(1, 1, 1, 1), tex2D(_MainTex, i.uv).rgba, _MainTexIntensity);
				//uses the main texture multiplies it by the ramped gradient information and then multiplies it with the tint
				float4 tintedTexture = mainTexture * color * tint;

				return tintedTexture;
			}
			ENDCG
		}
	}
}
