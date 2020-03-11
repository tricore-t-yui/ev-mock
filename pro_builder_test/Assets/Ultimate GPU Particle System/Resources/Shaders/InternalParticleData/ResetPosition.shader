Shader "GPUParticles/Internal/ResetPositionBuffer"
{
	Properties
	{
		//_Position ("Position", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
			
			//sampler2D _Position;

			float4 frag (v2f i) : SV_Target
			{
				//float4 col = tex2D(_Position, i.uv);
				//return float4(0,0,0,col.a);
				return float4(0,0,0,Random(i.uv));
			}
			ENDCG
		}
	}
}
