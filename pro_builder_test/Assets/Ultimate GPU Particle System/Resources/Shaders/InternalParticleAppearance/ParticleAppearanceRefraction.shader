Shader "GPUParticles/GPUParticlesRefraction"
{
	Properties
	{
		//Particle appearance
		_MainTex ("Main Tex", 2D) = "white" {}

		//Particle Data
		_Meta("Meta", 2D) = "white" {}
		_Velocity ("Velocity", 2D) = "white" {}
		_Position ("Positions", 2D) = "white" {}

		//Values over lifetime
		_ColorOverLifetime ("Color over Lifetime", 2D) = "white" {}
		
		//Skew
		_SizeOverLifetimeSkew("Size Skew", float) = 1
		_SizeMultiplier("Size Multiplier",float) = 1
		_RotationOverLifetimeSkew("Size Skew", float) = 1
		_RotationMultiplier("Size Multiplier",float) = 1

		//Modifiers
		_ColorIntensity ("Color intensity", float) = 1

		_VelocityScale("Velocity Scale", float) = 0.05

		_RefractionNormals("Refraction map (RG)", 2D) = "grey" {}
		_IndexOfRefraction("Index of refraction", float) = 0.81

		//Blend Modes
		_Src("Source", float) = 5
		_Dst("Destination", float) = 6
	}
	SubShader
	{
		GrabPass
		{
			"_BackgroundTexture"
		}

		Pass
		{
			//Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

			//Blend[_Src][_Dst]
			//Cull back ZWrite[_ZWrite] ZTest LEqual

			Tags{ "RenderType" = "Opaque" "Queue" = "Opaque" }

			Blend[_Src][_Dst]
			Cull back ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma shader_feature_local __ MAINTEX
			#pragma shader_feature_local POINT TRIANGLE BILLBOARD H_BILLBORD V_BILLBOARD S_BILLBOARD TS_BILLBOARD MESH //ANIMATED_MESH
			#pragma shader_feature_local __ TEXTUREEMITTER
			#pragma shader_feature_local __ TEXTURESHEET TEXTURESHEET_BLENDED TEXTURESHEET_MOTIONVECTORS
			#pragma shader_feature_local __ ROTATION
			#pragma shader_feature_local __ LINEAR_SIZE SMOOTH_SIZE CURVE_SIZE RANDOM2CURVES_SIZE
			#pragma shader_feature_local __ LINEAR_ROTATION SMOOTH_ROTATION CURVE_ROTATION RANDOM2CURVES_ROTATION
			#pragma shader_feature_local __ RANDOMINDEX
			//#pragma multi_compile_fog

			//#pragma exclude_renderers gles gles3 d3d9
			//#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "../Includes/GPUParticles.cginc"

			sampler2D _MainTex;
			sampler2D _Position;
			sampler2D _Velocity;
			sampler2D _Meta;

			sampler2D _ColorOverLifetime;

			float4 _SizeOverLifetimeBezierC1[10];
			float4 _SizeOverLifetimeBezierC2[10];
			int _SOLNumSegments = 2;

			float _SizeOverLifetimeSkew;
			float _SizeMultiplier;
			float _RotationOverLifetimeSkew;
			float _RotationMultiplier;
			float _Invert;

			float4 _RotationOverLifetimeBezierC1[10];
			float4 _RotationOverLifetimeBezierC2[10];
			int _ROLNumSegments = 2;

			float _ColorIntensity;
			float _VelocityScale;
			float _CustomTime;

			float _AspectRatio = 1.0;
			float4 _PositionOffset;

			sampler2D _BackgroundTexture;
			sampler2D _RefractionNormals;
			float _IndexOfRefraction;

			float4 _ForwardVector;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float2 uv3 : TEXCOORD2;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 color : TEXCOORD2;
				float Progress : TEXCOORD3;
				float4 screenPos : TEXCOORD4;
			};

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;

				float4 pos = tex2Dlod(_Position, float4(v.uv1, 0, 0));
				float4 meta = tex2Dlod(_Meta, float4(v.uv1, 0, 0));
				float4 vel = tex2Dlod(_Velocity, float4(v.uv1, 0, 0));

				float EndTime = meta.g - meta.r;
				float Time = _CustomTime - meta.r;
				float Progress = Time / EndTime;

				#ifdef RANDOMINDEX
					o.Progress = pos.a;
				#else
					#ifdef TEXTURESHEET
						o.Progress = Progress * _Cycles;
					#else
						o.Progress = Progress;
					#endif
				#endif

				float Scale = meta.b;
				int index = 0;
				float sizePerSegment;
				float partialStart;
				float partialEnd;
				float partialProgress;
				float2 P1;
				float2 P2;
				float2 C1;
				float2 C2;
				float2 value1;
				float2 value2;

				//Size over lifetime
				#ifdef LINEAR_SIZE
					float sizeProgress = pow(abs(_Invert - Progress), _SizeOverLifetimeSkew) * _SizeMultiplier;
					Scale = meta.b * sizeProgress;
				#elif SMOOTH_SIZE
					float sizeProgress = pow(Progress,_SizeOverLifetimeSkew);
					Scale = meta.b * abs(_Invert - (-sin(sizeProgress * 6.283 + 1.57) + 1) * 0.5);
					Scale *= _SizeMultiplier;
				#elif CURVE_SIZE
					index = floor(Progress * _SOLNumSegments);

					sizePerSegment = 1.0 / _SOLNumSegments;
					partialStart = index * sizePerSegment;
					partialEnd = (index + 1) * sizePerSegment;
					partialProgress = Remap(Progress, partialStart, partialEnd, 0.0, 1.0);
					index *= 2;

					P1 = float2(_SizeOverLifetimeBezierC1[index].x, _SizeOverLifetimeBezierC1[index].y);
					C1 = float2(_SizeOverLifetimeBezierC1[index].z, _SizeOverLifetimeBezierC1[index].w);
					P2 = float2(_SizeOverLifetimeBezierC1[index + 1].x, _SizeOverLifetimeBezierC1[index + 1].y);
					C2 = float2(_SizeOverLifetimeBezierC1[index + 1].z, _SizeOverLifetimeBezierC1[index + 1].w);
					value1 = BezierGetPosition(partialProgress, P1, C1, P2, C2);
					Scale = meta.b * value1.y;
				#elif RANDOM2CURVES_SIZE
					index = floor(Progress * _SOLNumSegments);

					sizePerSegment = 1.0 / _SOLNumSegments;
					partialStart = index * sizePerSegment;
					partialEnd = (index + 1) * sizePerSegment;
					partialProgress = Remap(Progress, partialStart, partialEnd, 0.0, 1.0);
					index *= 2;

					P1 = float2(_SizeOverLifetimeBezierC1[index].x, _SizeOverLifetimeBezierC1[index].y);
					C1 = float2(_SizeOverLifetimeBezierC1[index].z, _SizeOverLifetimeBezierC1[index].w);
					P2 = float2(_SizeOverLifetimeBezierC1[index + 1].x, _SizeOverLifetimeBezierC1[index + 1].y);
					C2 = float2(_SizeOverLifetimeBezierC1[index + 1].z, _SizeOverLifetimeBezierC1[index + 1].w);
					value1 = BezierGetPosition(partialProgress, P1, C1, P2, C2);

					P1 = float2(_SizeOverLifetimeBezierC2[index].x, _SizeOverLifetimeBezierC2[index].y);
					C1 = float2(_SizeOverLifetimeBezierC2[index].z, _SizeOverLifetimeBezierC2[index].w);
					P2 = float2(_SizeOverLifetimeBezierC2[index + 1].x, _SizeOverLifetimeBezierC2[index + 1].y);
					C2 = float2(_SizeOverLifetimeBezierC2[index + 1].z, _SizeOverLifetimeBezierC2[index + 1].w);
					value2 = BezierGetPosition(partialProgress, P1, C1, P2, C2);

					Scale = meta.b * lerp(value1.y, value2.y, pos.w);
				#endif

					//Rotation over lifetime
					float Rot = meta.a;

					#ifdef LINEAR_ROTATION
						float rotProgress = pow(Progress, _RotationOverLifetimeSkew) * _RotationMultiplier / 57.297;
						Rot = meta.a + rotProgress;
					#elif SMOOTH_ROTATION
						float rotProgress = pow(Progress, _RotationOverLifetimeSkew);
						Rot = meta.a + (-sin(rotProgress * 6.283 + 1.57) + 1)*0.5;
						Rot *= _RotationMultiplier / 57.297;
					#elif CURVE_ROTATION
						index = floor(Progress * _ROLNumSegments);

						sizePerSegment = 1.0 / _ROLNumSegments;
						partialStart = index * sizePerSegment;
						partialEnd = (index + 1) * sizePerSegment;
						partialProgress = Remap(Progress, partialStart, partialEnd, 0.0, 1.0);
						index *= 2;

						P1 = float2(_RotationOverLifetimeBezierC1[index].x, _RotationOverLifetimeBezierC1[index].y);
						C1 = float2(_RotationOverLifetimeBezierC1[index].z, _RotationOverLifetimeBezierC1[index].w);
						P2 = float2(_RotationOverLifetimeBezierC1[index + 1].x, _RotationOverLifetimeBezierC1[index + 1].y);
						C2 = float2(_RotationOverLifetimeBezierC1[index + 1].z, _RotationOverLifetimeBezierC1[index + 1].w);
						value1 = BezierGetPosition(partialProgress, P1, C1, P2, C2) / 57.297;
						Rot = meta.a + value1.y;
					#elif RANDOM2CURVES_ROTATION
						index = floor(Progress * _ROLNumSegments);

						sizePerSegment = 1.0 / _ROLNumSegments;
						partialStart = index * sizePerSegment;
						partialEnd = (index + 1) * sizePerSegment;
						partialProgress = Remap(Progress, partialStart, partialEnd, 0.0, 1.0);
						index *= 2;

						P1 = float2(_SizeOverLifetimeBezierC1[index].x, _SizeOverLifetimeBezierC1[index].y);
						C1 = float2(_SizeOverLifetimeBezierC1[index].z, _SizeOverLifetimeBezierC1[index].w);
						P2 = float2(_SizeOverLifetimeBezierC1[index + 1].x, _SizeOverLifetimeBezierC1[index + 1].y);
						C2 = float2(_SizeOverLifetimeBezierC1[index + 1].z, _SizeOverLifetimeBezierC1[index + 1].w);
						value1 = BezierGetPosition(partialProgress, P1, C1, P2, C2);

						P1 = float2(_SizeOverLifetimeBezierC2[index].x, _SizeOverLifetimeBezierC2[index].y);
						C1 = float2(_SizeOverLifetimeBezierC2[index].z, _SizeOverLifetimeBezierC2[index].w);
						P2 = float2(_SizeOverLifetimeBezierC2[index + 1].x, _SizeOverLifetimeBezierC2[index + 1].y);
						C2 = float2(_SizeOverLifetimeBezierC2[index + 1].z, _SizeOverLifetimeBezierC2[index + 1].w);
						value2 = BezierGetPosition(partialProgress, P1, C1, P2, C2);
						Rot = meta.a + lerp(value1.y, value2.y, pos.w);
					#endif

					float4 result1 = float4(0,0,0,1);
					float4 result2 = float4(0,0,0,1);

					pos.xyz += _PositionOffset.xyz * Scale;

					#ifdef POINT
						result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0));
					#endif

					#ifdef TRIANGLE
						#ifdef ROTATION

							float s = 0;
							float c = 0;
							sincos(Rot, s, c);

							//Rotation
							float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
															s, c, 0, 0,
															0, 0, 1, 0,
															0, 0, 0, 1
														);

							float4 BB = float4((v.uv2.x - 0.5) * Scale,
								(v.uv2.y - 0.5) * _AspectRatio * Scale,
								0.0, 0.0);

							result1 = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(pos.xyz, 1.0)) - mul(rotateZMatrix, BB));
						#else
							float4 BB = float4((v.uv2.x - 0.5) * Scale,
								(v.uv2.y - 0.5) * _AspectRatio * Scale,
								0.0, 0.0);

							result1 = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(pos.xyz, 1.0)) - BB);
						#endif
					#endif

					#ifdef BILLBOARD
						#ifdef ROTATION
							float s = 0;
							float c = 0;
							sincos(Rot, s, c);

							float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
															s, c, 0, 0,
															0, 0, 1, 0,
															0, 0, 0, 1
														);

							float4 BB = float4((v.uv2.x - 0.5) * Scale,
								(v.uv2.y - 0.5) * _AspectRatio * Scale,
								0.0, 0.0);

							result1 = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(pos.xyz, 1.0)) - mul(rotateZMatrix, BB));
						#else
							float4 BB = float4((v.uv2.x - 0.5) * Scale,
								(v.uv2.y - 0.5) * _AspectRatio * Scale,
								0.0, 0.0);

							result1 = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(pos.xyz, 1.0)) - BB);
						#endif



					#endif

					#ifdef H_BILLBORD
						#ifdef ROTATION

							float s = 0;
							float c = 0;
							sincos(Rot, s, c);

							//Rotation
							float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
															0, 1, 0, 0,
															-s, 0, c, 0,
															0, 0, 0, 1
														);

							float4 BB = float4((v.uv2.x - 0.5) * Scale,  0.0, (v.uv2.y - 0.5) * _AspectRatio * Scale, 0.0);
							result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0) - mul(rotateYMatrix, BB));
						#else
							float4 BB = float4((v.uv2.x - 0.5) * Scale,  0.0, (v.uv2.y - 0.5) * _AspectRatio * Scale, 0.0);
							result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0) - BB);
						#endif
					#endif

					#ifdef V_BILLBOARD
						#ifdef ROTATION

							float s = 0;
							float c = 0;
							sincos(Rot, s, c);

							//Rotation
							float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
															0, 1, 0, 0,
															-s, 0, c, 0,
															0, 0, 0, 1
														);

							float3 DirVector = float3(0,1,0);
							float3 CamVector = normalize(_WorldSpaceCameraPos - pos.xyz);
							float3 TangentVector = normalize(cross(DirVector, CamVector));

							pos.xyz += DirVector * ((v.uv2.x - 0.5)) * Scale;
							pos.xyz += (TangentVector * ((v.uv2.y - 0.5))) * Scale;

							result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0));
						#else
							float3 DirVector = float3(0,1,0);
							float3 CamVector = normalize(_WorldSpaceCameraPos - pos.xyz);
							float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

							pos.xyz += DirVector * ((v.uv2.x - 0.5)) * Scale;
							pos.xyz += (TangentVector * ((v.uv2.y - 0.5))) * Scale;

							result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0));
						#endif
					#endif

					#ifdef TS_BILLBOARD
						float3 StretchedVector = vel.xyz * _VelocityScale;
						float3 UnstretchedVector = normalize(vel.xyz) * Scale * 2;
						float3 DirVector = lerp(UnstretchedVector, StretchedVector, v.vertex.x);	//Calculate both and lerp to avoid an if statement

						float3 CamVector = UNITY_MATRIX_IT_MV[2].xyz;//normalize(_WorldSpaceCameraPos - pos.xyz);
						float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

						pos.xyz += DirVector * ((v.uv2.x - 0.5));
						pos.xyz += (TangentVector * ((v.uv2.y - 0.5))) * Scale;

						result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0));
					#endif

					#ifdef S_BILLBOARD
						//Stretched Billboard
						float3 DirVector = vel.xyz * _VelocityScale;
						float3 CamVector = UNITY_MATRIX_IT_MV[2].xyz;
						float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

						pos.xyz += DirVector * ((v.uv2.x - 0.5));
						pos.xyz += (TangentVector * ((v.uv2.y - 0.5))) * Scale;

						result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0));
					#endif

					#ifdef MESH
						//Rotate towards movement direction
						float3 dest = normalize(vel.xyz);
						float3 RotationAxis = normalize(cross(_ForwardVector.xyz, dest));
						float Angle = acos(dot(_ForwardVector.xyz, dest));
						v.vertex.xyz = RotateVertex(v.vertex.xyz, RotationAxis, Angle);

						//Scale and position update
						v.vertex.xyz *= Scale;
						v.vertex.xyz += pos.xyz;
						result1 = UnityObjectToClipPos(v.vertex);
					#endif

						//Transfer UVs
						o.uv1 = v.uv1;
						o.uv2 = 1 - v.uv2;

						//Color Over Lifetime
						float4 volUV = float4(Progress, pos.a, 0.0, 0.0);
						o.color = tex2Dlod(_ColorOverLifetime, volUV);

						//result1 = Particle is alive and is being positioned
						//result2 = Particle is dead and is hidden
						float IsAlive = clamp((sign(_CustomTime - meta.g) + 1.0) / 2.0, 0, 1);
						o.vertex = lerp(result1, result2, IsAlive);

						o.screenPos = ComputeScreenPos(o.vertex);
						o.screenPos.xy /= o.screenPos.w;

						UNITY_TRANSFER_FOG(o, o.vertex);

						return o;
					}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = float4(1,1,1,1);
				fixed4 normalsAlpha = tex2D(_RefractionNormals, i.uv2);
				float2 offset = normalize(normalsAlpha.rg * 2 - 1);
				float2 uv = i.screenPos + offset * _IndexOfRefraction;

				col.rgb = tex2D(_BackgroundTexture, uv).rgb;
				col.a = normalsAlpha.b;

				col *= i.color * _ColorIntensity;

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
		ENDCG
		}
	}
	//Fallback "GPUParticles/ParticlePositionSimple"
}