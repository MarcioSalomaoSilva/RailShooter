// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Experiments/PBS" {
	Properties{
		_Tint ("Tint", Color) = (1,1,1,1)
		_TintIntensity("Tint Strength", Range(0, 1)) = 0.5
		_MainTex ("Albedo", 2D) = "white" {}
		_MainTexIntensity("Texture Strength", Range(0, 1)) = 0.5
		[Gamma]_Metallic("Metallic", Range(0, 1)) = 0
		_Smoothness("Smoothness", Range(0, 1)) = 0.5
		[NoScaleOffset] _Ramp("Ramp Texture (for Gradient)", 2D) = "white" {}
		_TopColor("Opaque Color", Color) = (1,1,1,1)
		_BottomColor("Transparent Color", Color) = (1,1,1,1)
		[NoScaleOffset] _RampTex("Ramp Texture (for Gradient)", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex("Noise Texture (For Banding)", 2D) = "white" {}
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
				float		_TintIntensity;
				sampler2D	_MainTex, _RampTex, _NoiseTex, _Ramp;
				float4		_MainTex_ST;
				float		_MainTexIntensity;
				float4		_BottomColor;
				float4		_TopColor;
				//sampler2D	_RampTex;
				//sampler2D	_NoiseTex;
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
			float3 getNoise(float2 uv)
			{
				float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
				noise = mad(noise, 2.0, -0.5);
				return noise / 255;
			}
			float4 MyFragmentProgram (Interpolators i) : SV_TARGET{
				//normalizes normals
				i.normal = normalize(i.normal);
				//uses the ramp textures alpha channel to lerp between two colors and adds the info from the noise textue to avoid banding
				//user control banding and oter effects can be controlled with the ramp texture
				half4 ramp = tex2D(_RampTex, i.uv).a;
				half4 color = lerp(_BottomColor, _TopColor, ramp);
				color.rgb += getNoise(i.uv);
				//gets the light direction and color calculates the albedo(texture and tint) and multiplies it by the diffuse
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightColor = _LightColor0.rgb;
				float4 tint = lerp(float4(1, 1, 1, 1), _Tint, _TintIntensity);
				float4 mainTexture = lerp(float4(1, 1, 1, 1), tex2D(_MainTex, i.uv).rgba, _MainTexIntensity);
				float3 albedo = mainTexture.rgb * tint.rgb * color.rgb;
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
				//fixed NdotL = DotClamped(i.normal, lightDir);
				//light.ndotl = tex2D(_Ramp, NdotL).rgb;
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
