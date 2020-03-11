Shader "GPUParticles/Internal/Meta"
{
	Properties
	{
		//Particle Data
		_NewParticle("New Particle", 2D) = "white" {}
		_Meta("Meta", 2D) = "white" {}
		_Position ("Position", 2D) = "white" {}

		//To be visible in editor:
		_StartSizeRot ("_StartSizeRot", vector) = (0,0,0,0)
		_StartLifeTimeSpeed ("_StartLifeTimeSpeed", vector) = (0,0,0,0)
		_CustomTime("_CustomTime", float) = 0
	}

	SubShader
	{
		Cull Off ZWrite Off ZTest LEqual
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "../Includes/GPUParticles.cginc"

			sampler2D _NewParticle;
			sampler2D _Position;
			sampler2D _Meta;

			float4 _StartSizeRot;
			float4 _StartLifeTimeSpeed;
			float _CustomTime;

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
		
			float4 frag(v2f i) : SV_Target
			{
				float rand = tex2D(_Position, i.uv).a;
				float4 meta = tex2D(_Meta, i.uv);
				float particle = tex2D(_NewParticle, i.uv).r;

				float startSize = lerp(_StartSizeRot.x, _StartSizeRot.y, rand);
				float startRot = lerp(_StartSizeRot.z, _StartSizeRot.w, rand);
				float startLifeTime = lerp(_StartLifeTimeSpeed.x, _StartLifeTimeSpeed.y, rand);

				float4 result1 = float4(_CustomTime, _CustomTime + startLifeTime, startSize, startRot);
				float4 result2 = meta;

				//result1 = New particle with new Start Time, Life time end, Start Size and Start Rotation values
				//result2 = original value, no changes
				//If the particle is new, write new values to buffer. Otherwise write original value.
				float4 FinalResult = lerp(result1, result2, particle);
				return FinalResult;
			}
			ENDCG
		}
	}
}
