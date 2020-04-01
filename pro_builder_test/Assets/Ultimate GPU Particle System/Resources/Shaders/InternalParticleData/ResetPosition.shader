Shader "GPUParticles/Internal/ResetPositionBuffer"
{
	Properties
	{
		_RandomValues ("Random Values", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature_local __ TRAILS

			#include "UnityCG.cginc"
			#include "../Includes/GPUParticles.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _RandomValues;

			float4 frag (v2f i) : SV_Target
			{
				#ifdef TRAILS
					i.uv.x = 0;
					float randomValue = tex2D(_RandomValues, i.uv).r;
				#else
					float randomValue = tex2D(_RandomValues, i.uv).r;
				#endif
				
				return float4(0,0,0,randomValue);
			}
			ENDCG
		}
	}
}
