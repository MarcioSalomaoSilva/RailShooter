﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Experiments/PBS" {
	Properties{
		_Tint ("Tint", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		[Gamma]_Metallic("Metallic", Range(0, 1)) = 0
		_Smoothness("Smoothness", Range(0, 1)) = 0.5
	}
		SubShader{
			Pass {
				Tags{
					"LightMode" = "ForwardBase"
				}
				CGPROGRAM
				#pragma target 3.0
				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram
				#include "UnityCG.cginc"
				#include "UnityPBSLighting.cginc"
				float4		_Tint;
				sampler2D	_MainTex;
				float4		_MainTex_ST;
				float		_Metallic;
				float		_Smoothness;
			struct Interpolators {
				float4 position			: SV_POSITION;
				float2 uv				: TEXCOORD0;
				float3 normal			: TEXCOORD1;
				float3 worldPos			: TEXCOORD2;
			};
			struct VertexData {
				float4 position			: POSITION;
				float3 normal			: NORMAL;
				float2 uv				: TEXCOORD0;
			};
			Interpolators MyVertexProgram(VertexData v) {
				Interpolators i;
				//gets vertex positions
				i.position = UnityObjectToClipPos(v.position);
				//gets view direction
				i.worldPos = mul(unity_ObjectToWorld, v.position);
				//gets texture info
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//converts normals to world space and normalizes them
				i.normal = UnityObjectToWorldNormal(v.normal);
				return i;
			}
			float4 MyFragmentProgram (Interpolators i) : SV_TARGET{
				//normalizes normals
				i.normal = normalize(i.normal);
				//gets the light direction and color calculates the albedo(texture and tint) and multiplies it by the diffuse
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightColor = _LightColor0.rgb;
				float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
				float3 specularTint;
				float oneMinusReflectivity;
				albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);
				//specular
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				//light structure // source
				UnityLight light;
				light.color = lightColor;
				light.dir = lightDir;
				light.ndotl = DotClamped(i.normal, lightDir);
				//indirect light (ambient and spec)
				UnityIndirect indirectLight;
				indirectLight.diffuse = 0;
				indirectLight.specular = 0;
				//
				return UNITY_BRDF_PBS(albedo, specularTint,
					oneMinusReflectivity, _Smoothness,
					i.normal, viewDir, 
					light, indirectLight);
			}
			ENDCG
		}
	}
}
