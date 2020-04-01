Shader "GPUParticles/Internal/Position"
{
	Properties
	{
		_EmitterPosition("Emitter Position", vector) = (0,0,0,1)

		_Meta("Meta", 2D) = "white" {}
		_Velocity ("Velocity", 2D) = "white" {}
		_Position("Position", 2D) = "white" {}
		
		_NewParticle("New Particle Mask", 2D) = "white" {}
		_MeshEmitterPositions("Mesh Emitter Positions", 2D) = "white" {}

		_StartLifeTime("Start Life Time", float) = 1
		
		_EmitterParam("Emitter Parameters", vector) = (1,1,0,0)
		_EmitterLength("Emitter Length", float) = 1

		_Turbulence("Turbulence", 2D) = "white" {}

		_Frequency("Frequency", vector) = (0,0,0,0)
		_Amplitude("Amplitude", vector) = (0,0,0,0)
		_Offset("Offset", vector) = (0,0,0,0)

		_FollowSegment("Follow Segment", float) = 1
		_FollowSpeed("Follow Speed", float) = 0.1
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma shader_feature_local POINT EDGE CIRCLE BOX HEMISPHERE SPHERE CONE MESH TEXTUREEMITTER
			#pragma shader_feature_local __ LOCALSIM
			#pragma shader_feature_local __ EMITFROMSHELL
			#pragma shader_feature_local __ EMITFROMBASE
			#pragma shader_feature_local __ TRAILS

			#include "UnityCG.cginc"
			#include "../Includes/GPUParticles.cginc"

			sampler2D _Position;
			sampler2D _Velocity;
			sampler2D _Meta;
			sampler2D _MeshEmitterPositions;
			sampler2D _NewParticle;

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

			float4 _EmitterPosition;
			float4 _Amplitude;
			float4 _Frequency;
			float4 _Offset;
			float4 _EmitterParam;
			float _StartLifeTime;
			float4x4 _EmitterMatrix;
			float _CustomTime;
			float _CustomDeltaTime;

			float _TexelWidth;
			float _FollowSegment;
			float _FollowSpeed;

			float4 frag (v2f i) : SV_Target
			{
				fixed isNew = tex2D(_NewParticle, i.uv).r;
				float4 meta = tex2D(_Meta, i.uv);
				float4 vel = tex2D(_Velocity, i.uv);
				float4 pos = tex2D(_Position, i.uv);

				float4 result0 = float4(10000,0,0,pos.a);				//Particle is dead; Reset position 
				float4 result1 = pos;									//Particle is alive; Set original position for further calculations

				
				#if TRAILS	
					float2 ouv = i.uv;
					i.uv.x = _FollowSegment;
				#endif

				#ifdef LOCALSIM
					//Local Simulation
					#ifdef CIRCLE
						#ifdef EMITFROMSHELL
							result0.xyz = GetRandomPointOnDiscEdgeLocal(i.uv, _EmitterParam.x).xyz;
						#else
							result0.xyz = GetRandomPointOnDiscSurfaceLocal(i.uv, _EmitterParam.x).xyz;
						#endif
					#endif
						
					#ifdef EDGE
						result0.xyz = GetRandomPointOnEdgeLocal(i.uv, _EmitterParam.x).xyz;
					#endif
						
					#ifdef CONE
						#ifdef EMITFROMBASE
							#ifdef EMITFROMSHELL
								result0.xyz = GetRandomPointConeLocalBaseEdge(i.uv, _EmitterParam.x, _EmitterParam.z).xyz;
							#else
								result0.xyz = GetRandomPointConeLocalBase(i.uv, _EmitterParam.x, _EmitterParam.z).xyz;
							#endif
						#else
							#ifdef EMITFROMSHELL
								result0.xyz = GetRandomPointConeLocalEdge(i.uv, _EmitterParam.x, _EmitterParam.w, _EmitterParam.z).xyz;
							#else
								result0.xyz = GetRandomPointConeLocal(i.uv, _EmitterParam.x, _EmitterParam.w, _EmitterParam.z).xyz;
							#endif
						#endif	
					#endif
						
					#ifdef BOX
						result0.xyz = GetRandomPointBoxLocal(i.uv, _EmitterParam.xyz).xyz;
					#endif
						
					#ifdef HEMISPHERE
						#ifdef EMITFROMSHELL
							result0.xyz = GetRandomPointHemiSphereSurfaceLocal(i.uv, _EmitterParam.x);
						#else
							result0.xyz = GetRandomPointHemiSphereVolumeLocal(i.uv, _EmitterParam.x);
						#endif
							
					#endif

					#ifdef MESH
						result0.xyz = GetPointOnMeshLocal(_MeshEmitterPositions, i.uv);
					#endif

					//Local and World
					#ifdef SPHERE
						result0.xyz = _EmitterPosition.xyz + GetRandomPositionSphere(i.uv, _EmitterParam.x).xyz;
					#endif

					#ifdef POINT
						result0.xyz = _EmitterPosition.xyz;
					#endif
				#else
					//World Simulation
					#ifdef CIRCLE
						#ifdef EMITFROMSHELL
							result0.xyz = GetRandomPointOnDiscEdgeWorld(i.uv, _EmitterParam.x, _EmitterMatrix).xyz;
						#else
							result0.xyz = GetRandomPointOnDiscSurfaceWorld(i.uv, _EmitterParam.x, _EmitterMatrix).xyz;
						#endif
					#endif
						
					#ifdef EDGE
						result0.xyz = GetRandomPointOnEdgeWorld(i.uv, _EmitterParam.x, _EmitterMatrix).xyz;
					#endif
						
					#ifdef CONE
						#ifdef EMITFROMBASE
							#ifdef EMITFROMSHELL
								result0.xyz = GetRandomPointConeWorldBaseEdge(i.uv, _EmitterParam.x, _EmitterMatrix).xyz;
							#else
								result0.xyz = GetRandomPointConeWorldBase(i.uv, _EmitterParam.x, _EmitterMatrix).xyz;
							#endif
						#else
							#ifdef EMITFROMSHELL
								result0.xyz = GetRandomPointConeWorldEdge(i.uv, _EmitterParam.x, _EmitterParam.w, _EmitterParam.z, _EmitterMatrix).xyz;
							#else
								result0.xyz = GetRandomPointConeWorld(i.uv, _EmitterParam.x, _EmitterParam.w, _EmitterParam.z, _EmitterMatrix).xyz;
							#endif
						#endif	
					#endif
						
					#ifdef BOX
						result0.xyz = GetRandomPointBoxWorldVolume(i.uv, _EmitterParam.xyz, _EmitterMatrix).xyz;
					#endif
						
					#ifdef HEMISPHERE
						#ifdef EMITFROMSHELL
							result0.xyz = GetRandomPointHemiSphereSurfaceWorld(i.uv, _EmitterParam.x, _EmitterMatrix);
						#else
							result0.xyz = GetRandomPointHemiSphereVolumeWorld(i.uv, _EmitterParam.x, _EmitterMatrix);
						#endif
					#endif

					#ifdef MESH
						result0.xyz = GetPointOnMeshWorld(_MeshEmitterPositions, i.uv, _EmitterMatrix);
					#endif

					#ifdef TEXTUREEMITTER
						result0 = GetPointOnPlaneWorld(i.uv, _EmitterParam, _EmitterMatrix);
					#endif

					//Local and World
					#ifdef SPHERE
						#ifdef EMITFROMSHELL
							result0.xyz = _EmitterPosition.xyz + GetRandomPositionSphere(i.uv, _EmitterParam.x).xyz;
						#else
							result0.xyz = _EmitterPosition.xyz + GetRandomPositionSphereVolume(i.uv, _EmitterParam.x).xyz;
						#endif
					#endif

					#ifdef POINT
						result0.xyz = _EmitterPosition.xyz;
					#endif
				#endif
					
				#if TRAILS	
						
					if (ouv.x > _FollowSegment)
					{
						result1 = lerp(pos, tex2D(_Position, ouv - float2(_FollowSegment, 0.0)), clamp(_FollowSpeed * _CustomDeltaTime, 0.01, 1));
					}
					else {
						result1.xyz += vel.xyz * _CustomDeltaTime;
					}
				#else
					result1.xyz += vel.xyz * _CustomDeltaTime;
				#endif

				int isAlive = (sign(meta.g-_CustomTime) + 1.0) / 2.0;

				float4 finalResult = lerp(result0, result1, isAlive);
				finalResult = lerp(result0, finalResult, isNew);
				return finalResult;
			}
			ENDCG
		}
	}
}
