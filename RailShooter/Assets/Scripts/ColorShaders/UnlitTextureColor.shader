// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Personal/UnlitTextureColor"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
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
			};

			struct v2f
			{
				float2 texcoord : TEXCOORD0;
//				float4 vertex : SV_POSITION;
				float4 pos :  SV_POSITION;
			};

			sampler2D _MainTex;
//			float4 _MainTex_ST;
			fixed4 _Color;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.vertex);
//				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				// sample the texture
				fixed4 tex = tex2D(_MainTex, IN.texcoord);
				fixed4 col = _Color;
				return tex * col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
