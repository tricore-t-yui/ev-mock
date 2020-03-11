Shader "GPUParticles/Debug/DrawCurve"
{
	Properties
	{
		_Skew ("Skew", float) = 1
		_Multiplier ("Multiplier", float) = 1
		_Invert ("Invert", float) = 1
		_Length ("Length", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ LINEAR

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			float _Skew;
			float _Multiplier;
			float _Invert;
			float _Length;//usually 0-1 (%)

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float Scale = 0;

				#ifdef LINEAR
					Scale = pow(abs(_Invert-i.uv.x),_Skew);
				#else
					float sizeProgress = pow(i.uv.x,_Skew);
					Scale = abs(_Invert-(-sin(sizeProgress * 6.283 + 1.57) + 1) * 0.5);
				#endif

				fixed4 col = clamp(distance(i.uv, float2(i.uv.x, Scale)) / .02, 0, 1);

				col *= clamp(((sin(i.uv.y * (62.83 * _Multiplier) - (1.775 * _Multiplier)) + 1) / 2)/ 0.01, 0, 1);
				col *= clamp(((sin(i.uv.x * (62.83 * _Length) - (1.775 * _Length)) + 1) / 2) / 0.01, 0, 1);
				
				if(i.uv.x < 0.005 || i.uv.x > 0.995 || i.uv.y < 0.005 || i.uv.y > 0.995)
					col = fixed4(1,0.5,0.5,0);
				
				return col;
			}
			ENDCG
		}
	}
}
