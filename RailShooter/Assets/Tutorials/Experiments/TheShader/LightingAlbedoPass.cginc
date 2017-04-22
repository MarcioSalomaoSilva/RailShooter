#if !defined(LIGHTING_ALBEDO_PASS)
#define LIGHTING_ALBEDO_PASS	
	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
	#include "UnityPBSLighting.cginc"
	#include "UnityStandardBRDF.cginc"
	#include "UnityStandardUtils.cginc"
#endif
	float4 _Tint;
	float  _TintIntensity;
	sampler2D _MainTex, _Ramp, _RampTex, _NoiseTex;
	float4 _MainTex_ST;
	float  _MainTexIntensity;
	float  _GradientIntensity;
	float  _RampIntensity;
	float4 _BottomColor;
	float4 _TopColor;
	//
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
		//converts normals to world space and normalizes them
		i.uv = TRANSFORM_TEX(v.uv, _MainTex);
		//converts normals to world space and normalizes them
		i.normal = UnityObjectToWorldNormal(v.normal);
		return i;
	}
	float3 GetNoise(float2 uv)
	{
		float3 noise = tex2D(_NoiseTex, uv * 100 + _Time * 50);
		noise = mad(noise, 2.0, -0.5);
		return noise / 255;
	}
	float3 Light(Interpolators i) : SV_TARGET {
		//gets the light direction and color calculates the albedo(texture and tint) and multiplies it by the diffuse
		//float3 lightDir = _WorldSpaceLightPos0.xyz;
		float3 lightDir;
		#if defined(POINT) || defined(SPOT) || defined(POINT_COOKIE)
			lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
		#else
			lightDir = _WorldSpaceLightPos0.xyz;
		#endif
		//
		UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
		//
		float3 lightColor = _LightColor0.rgb * attenuation;
		float3 light = lerp(float4(1, 1, 1, 1), DotClamped(i.normal, lightDir) * lightColor, _RampIntensity);
		return light;
	}
	float4 MyFragmentProgram(Interpolators i) : SV_TARGET{
		//normalizes normals
		i.normal = normalize(i.normal);
		//uses the tint intensity property to change the tint strength. Lerps between white (invsible) to the tint color
		float4 tint = lerp(float4(1,1, 1, 1), _Tint, _TintIntensity);
		//uses the ramp textures alpha channel to lerp between two colors and adds the info from the noise textue to avoid banding
		//user control banding and oter effects can be controlled with the ramp texture
		half4 ramp = lerp(float4(1,1,1,0),tex2D(_RampTex, i.uv).a, _GradientIntensity);
		half4 color = lerp(_BottomColor, _TopColor, ramp);
		color.rgb += GetNoise(i.uv);
		//
		float3 albedo = lerp(float4(1, 1, 1, 1), tex2D(_MainTex, i.uv), _MainTexIntensity).rgb * color * tint + GetNoise(i.uv);
		//
		float3 light = Light(i);
		float3 diffuse = albedo * light;
		//
		return float4(diffuse, 1);
	}
