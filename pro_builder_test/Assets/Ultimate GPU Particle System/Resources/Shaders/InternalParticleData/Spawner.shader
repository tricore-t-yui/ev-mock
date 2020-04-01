Shader "GPUParticles/Internal/Spawner"
{
	Properties
	{
		_StartID("Start ID", int) = 0
		_EndID("End ID", int) = 0
		_MapWidth("Map Width", int) = 0
		_MapHeight("Map Height", int) = 0
	}

	SubShader
	{
		Cull Off ZWrite Off ZTest LEqual
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ TRAILS


			#include "UnityCG.cginc"
			#include "../Includes/GPUParticles.cginc"

			int _StartID;
			int _EndID;
			int _MapWidth;
			int _MapHeight;

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
		
			float frag(v2f i) : SV_Target
			{
#if TRAILS
				int yCoord = i.uv.y * _MapHeight;
				int index = floor(yCoord);

				//Result 1 = Particle is new
				//Result 2 = Particle is not new
				float result1 = 0.0;
				float result2 = 1.0;

				float s1 = sign(_StartID - index);
				float s2 = sign(index - (_EndID - 1));

				float finalResult = lerp(result1, result2, s1);
				finalResult = lerp(finalResult, result2, s2);
				return finalResult;
#else
				int xCoord = i.uv.x * _MapWidth;
				int yCoord = i.uv.y * _MapHeight;

				int index = floor(yCoord) * _MapWidth + floor(xCoord);

				//Result 1 = Particle is new
				//Result 2 = Particle is not new
				float result1 = 0.0;
				float result2 = 1.0;

				float s1 = sign(_StartID - index);
				float s2 = sign(index - (_EndID - 1));

				float finalResult = lerp(result1, result2, s1);
				finalResult = lerp(finalResult, result2, s2);
				return finalResult;
#endif
			}
			ENDCG
		}
	}
}
