// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Experiments/TheShader"{
	Properties{
		[Header(Tint Color)]_Tint("Tint", Color) = (1,1,1,1)
		_TintIntensity("Tint Strength", Range(0, 1)) = 0.5
		[Header(Texture)]_MainTex("Diffuse", 2D) = "white" {}
		_MainTexIntensity("Texture Strength", Range(0, 1)) = 0.5
		[Header(Color Gradient)] _TopColor("Opaque Color", Color) = (1,1,1,1)
		_BottomColor("Transparent Color", Color) = (1,1,1,1)
		[NoScaleOffset] _RampTex("Ramp Texture (for Gradient)", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex("Noise Texture (For Blending)", 2D) = "white" {}
		_GradientIntensity("Gradient Strength", Range(0, 1)) = 0.5
		[Header(Shadow Ramp)] _Ramp("Ramp Texture (for shadows)", 2D) = "white" {}
		_RampIntensity("shadow Strength", Range(0.02, 0.95)) = 0.5
	}
	SubShader{
		Pass{
			Tags{
			"LightMode" = "ForwardBase"
			"RenderType" = "Opaque"
			}
			ZWrite On
			ZTest LEqual
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#include "LightingRampPass.cginc"
			ENDCG
			//
			}
			Pass{
			Tags{
			"LightMode" = "ForwardAdd"
			"RenderType" = "Opaque"
			}
			Blend One One
			ZWrite off//turn off of writing to the depth buffer
			ZTest LEqual
			CGPROGRAM
			#pragma target 3.0
			//#pragma multi_compile DIRECTIONAL DIRECTIONAL_COOKIE POINT SPOT
			#pragma multi_compile_fwdadd //replaces above
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#include "LightingAlbedoPass.cginc"
			ENDCG
		}
	}
	FallBack "Diffuse" //note: required for passes: ForwardBase, ShadowCaster, ShadowCollector and Depth
}
