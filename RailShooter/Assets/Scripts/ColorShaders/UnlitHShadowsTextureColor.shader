// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Personal/UnlitHShadowsTextureColor"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_UnlitColor ("Shadow", Color) = (0,0,0,1)
		_DiffuseThreshold ("Shadow Position", Range(-1.1,1)) = 0.1
		_SpecColor ("Specular Color", Color) = (1,1,1,1)
		_Shininess ("Shininess", Range(0.5,1)) = 1
		_OutlineThickness ("Outline Thickness", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  }
//		Blend SrcAlpha OneMinusSrcAlpha
//		cull on
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAl;
			};

			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float3 normalDir : TEXCOORD1;
				float4 lightDir : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
				float4 pos :  SV_POSITION;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _UnlitColor;
			float _DiffuseThreshold;
			float4 _SpecColor;
			float _Shininess;
			float _OutlineThickness;
			
			v2f vert (appdata IN)
			{
				v2f OUT;

				//normal direction
				OUT.normalDir = normalize (mul(float4(IN.normal, 0.0), unity_WorldToObject).xyz);

				//world position
				float4 posWorld = mul(unity_ObjectToWorld, IN.vertex);

				//view direction
				OUT.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);

				//light direction
				float3 fragmentToLightSource = (_WorldSpaceCameraPos.xyz - posWorld.xyz);
				OUT.lightDir = float4 ( 
					normalize(lerp(_WorldSpaceCameraPos.xyz, fragmentToLightSource, _WorldSpaceLightPos0.w)),
					lerp(1.0 , 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w));

				//fragment input output
				OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
//				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				//
				float nDotL = saturate (dot(IN.normalDir, IN.lightDir.xyz));
				//diffuse threshold calc
				float diffuseCutOff = saturate((max(_DiffuseThreshold, nDotL) - _DiffuseThreshold ) * 1000);
				//specular threshold calc
				float specularCutOff = saturate(max(_Shininess, dot(reflect(-IN.lightDir.xyz, IN.normalDir), IN.viewDir))-_Shininess) * 1000;
				//calc outlines
				float outlineStrength = saturate((dot(IN.normalDir, IN.viewDir) - _OutlineThickness) * 1000);
				//
				float3 ambientLight = (1-diffuseCutOff) * _UnlitColor.xyz;
				float3 diffuseReflection = (1-specularCutOff) * _Color.xyz * diffuseCutOff;
				float3 specularReflection = _SpecColor.xyz * specularCutOff;
				//
			 	float3 combinedLight = (ambientLight );
				// sample the texture
				fixed4 tex = tex2D(_MainTex, IN.texcoord);
				fixed4 col = _Color;
				return float4 (combinedLight, 1.0) * _Color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
