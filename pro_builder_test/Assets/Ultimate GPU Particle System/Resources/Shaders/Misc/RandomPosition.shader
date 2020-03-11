Shader "Hidden/SkinnedMeshPosition"
{
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Zwrite on ZTest Always Cull back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float4 color : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.color = mul(unity_ObjectToWorld, v.vertex);
				//o.color = v.vertex;
				float4 pos = float4( v.uv2.x * 2 - 1, -v.uv2.y * 2 + 1,0, 1);
				pos.y *= -_ProjectionParams.x;//Flips uv if necessary
				o.vertex = pos;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = i.color;
				col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
