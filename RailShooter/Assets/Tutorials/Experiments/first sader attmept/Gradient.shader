// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Gradient"
{
	Properties
	{
		_Transparency("Transparency (Obj)", Range(0, 1)) = 0
		[NoScaleOffset] _MainTex ("Texture (Additive)", 2D) = "white" {}
		[NoScaleOffset] _NormalTex("Normal Map (for shadows)", 2D) = "bump" {}
		_NormalDepth("Normal Depth (height of map)", Range(-2,2)) = 1
		_TopColor("Top Color (zero alpha for cutout)", Color) = (1,1,1,1)
		_BottomColor("Bottom Color (see above)", Color) = (1,1,1,1)
		_Intensity("Intensity", Range(0, 1)) = 0
		[NoScaleOffset] _RampTex("Ramp Texture (for Gradient)", 2D) = "white" {}
		[NoScaleOffset] _NoiseTex("Noise Texture (For Blend)", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		//Tags{ "RenderType" = "Opaque" }
		//ZWrite On
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			//Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			//
			struct appdata
			{
				float4 position :	POSITION;
				float2 uv :			TEXCOORD0;
				float4 texcoord :	TEXCOORD1;
				float3 normal :		NORMAL;
				float4 tangent :	TANGENT;
			};
			///
			struct v2f
			{
				float2 uv :				TEXCOORD0;
				float3 tbn[3] :			TEXCOORD1; // TEXCOORD2; TEXCOORD3;
				float3 normal :			NORMAL;
				float4 position :		SV_POSITION;
				//half4 color :			COLOR;
			};
			//
			sampler2D _MainTex;
			sampler2D _NormalTex;
			half4 _BottomColor;
			half4 _TopColor;
			float _Transparency;
			float _Intensity;
			//
			v2f vert(appdata input)
			{
				v2f output;
				//
				output.position = UnityObjectToClipPos(input.position);
				output.uv = input.uv;
				//
				float3 normal = UnityObjectToWorldNormal(input.normal);
				float3 tangent = UnityObjectToWorldNormal(input.tangent);
				float3 bitangent = cross(tangent, normal);
				//
				output.tbn[0] = tangent;
				output.tbn[1] = bitangent;
				output.tbn[2] = normal;
				//
				return output;
			}
			//
			sampler2D _RampTex;
			sampler2D _NoiseTex;
			float _NormalDepth;
			//
			float3 getNoise(float2 uv)
			{
				float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
				noise = mad(noise, 2.0, -0.5);
				//
				return noise / 255;
			}
			//
			fixed4 frag(v2f input) : SV_Target
			{
				//gradient using the ramp texture
				half4 ramp = tex2D(_RampTex, input.uv).a * _Intensity;
				half4 color = lerp(_BottomColor, _TopColor, ramp ) * _Transparency;
				color.rgb += getNoise(input.uv);
				//normal test
				float3 tangentNormal = tex2D(_NormalTex, input.uv) * 2 - 1;
				float3 surfaceNormal = input.tbn[2];
				float3 worldNormal = float3(input.tbn[0] * tangentNormal.r + input.tbn[1] * tangentNormal.g + input.tbn[2] * tangentNormal.b);
				//
				//c.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
				//half3 blend = tex2D(_BlendTex, IN.uv_MainTex).rgb;
				float3 normalDot = dot(worldNormal, _WorldSpaceLightPos0) * _NormalDepth;
				color.r = min(1.0, color.r + normalDot.r);
				color.g = min(1.0, color.g + normalDot.g);
				color.b = min(1.0, color.b + normalDot.b);
				//return dot(worldNormal, _WorldSpaceLightPos0);
				return color;
			}
			ENDCG
		}
		Pass
		{
			//Blend SrcAlpha OneMinusSrcAlpha
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" // for UnityObjectToWorldNormal
			#include "UnityLightingCommon.cginc" // for _LightColor0
			//
			struct appdata
			{
				float4 position :	POSITION;
				float2 uv :			TEXCOORD0;
				float3 normal :		NORMAL;
				float4 tangent :	TANGENT;
			};
			///
			struct v2f
			{
				float2 uv :				TEXCOORD0;
				float3 normal :			NORMAL;
				float4 position :		SV_POSITION;
				fixed4 diff :			COLOR0; // diffuse lighting color
			};
			//
			v2f vert(appdata input)
			{
				v2f output;
				//
				output.position = UnityObjectToClipPos(input.position);
				output.uv = input.uv;
				// get vertex normal in world space
				half3 worldNormal = UnityObjectToWorldNormal(input.normal);
				// dot product between normal and light direction for
				// standard diffuse (Lambert) lighting
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				// factor in the light color
				output.diff = nl * _LightColor0;
				//
				return output;
			}
			//
			sampler2D _NoiseTex;
			//
			float3 getNoise(float2 uv)
			{
				float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
				noise = mad(noise, 2.0, -0.5);
				//
				return noise / 255;
			}
			//
			fixed4 frag(v2f input) : SV_Target
			{
				// sample texture
                //fixed4 col = tex2D(_MainTex, input.uv);
				// multiply by lighting
				//col *= input.diff;
				return input.diff;
			}
			ENDCG
		}
	}
}
