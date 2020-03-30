﻿Shader "GPUParticles/ParticleStandard"  
{
	Properties 
	{
		_MainTex("_MainTex (RGBA)", 2D) = "white" {}
		_Position("_Position (RGB)", 2D) = "white" {}
		_Velocity("_Velocity (RGB)", 2D) = "white" {}
		_ColorOverLifetime("_ColorOverLifetime (RGBA)", 2D) = "white" {}
		_Meta("_Meta (RGB)", 2D) = "white" {}
		_ForwardVector("Forward Vector", vector) = (1,0,0,0)
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }

		//ZWrite[_ZWrite] ZTest LEqual Cull back
		ZWrite On ZTest LEqual Cull back

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows addshadow vertex:vert
		//#pragma nolightmap nometa nodynlightmap nodirlightmap 
		
		#pragma target 3.0
		#include "../Includes/GPUParticles.cginc"

		#pragma shader_feature __ MAINTEX
		#pragma shader_feature __ METALLIC_SMOOTHNESS
		#pragma shader_feature __ NORMAL_MAP
		#pragma shader_feature __ EMISSION_MAP
		#pragma shader_feature __ POINT TRIANGLE BILLBOARD H_BILLBORD V_BILLBOARD S_BILLBOARD TS_BILLBOARD MESH
		#pragma shader_feature __ ROTATION
		//#pragma shader_feature __ LINEAR_SIZE SMOOTH_SIZE CURVE_SIZE RANDOM2CURVES_SIZE
		//#pragma shader_feature __ LINEAR_ROTATION SMOOTH_ROTATION CURVE_ROTATION RANDOM2CURVES_ROTATION

		sampler2D _MainTex;
		sampler2D _Position;
		sampler2D _Velocity;
		sampler2D _Meta;

		sampler2D _ColorOverLifetime;

		sampler2D _MetallicSmoothness;
		sampler2D _BumpMap;
		sampler2D _Emission;

		float _Metallic;
		float _Smoothness;

		float _ColorIntensity;
		float _VelocityScale;
		float _CustomTime;
		float4 _ForwardVector;
		
		float _AspectRatio = 1.0;
		float4 _PositionOffset;

		float _Invert;

		float4 _SizeOverLifetimeBezierC1[10];
		float4 _SizeOverLifetimeBezierC2[10];
		int _SOLNumSegments = 2;

		float _SizeOverLifetimeSkew;
		float _SizeMultiplier;
		float _RotationOverLifetimeSkew;
		float _RotationMultiplier;

		float4 _RotationOverLifetimeBezierC1[10];
		float4 _RotationOverLifetimeBezierC2[10];
		int _ROLNumSegments = 2;
		
		struct Input {
			float2 uv2_MainTex;
			float2 uv_SpecMap;
			float4 tintColor;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			float4 pos = tex2Dlod(_Position, v.texcoord);
			float4 meta = tex2Dlod(_Meta, v.texcoord);
			float4 vel = tex2Dlod(_Velocity, v.texcoord);

			float EndTime = meta.g - meta.r;
			float Time = _CustomTime - meta.r;
			float Progress = Time / EndTime;

			float4 result1 = float4(0, 0, 0, 1);
			float4 result2 = float4(0, 0, 0, 1);

			float Scale = meta.b;
			float Rot = meta.a;

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
			float sizeProgress = pow(Progress, _SizeOverLifetimeSkew);
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
		
			pos.xyz += _PositionOffset.xyz * Scale;

		#ifdef POINT
			result1 = float4(pos.xyz, 1.0);
		#endif

		#ifdef TRIANGLE
			#ifdef ROTATION

				float s = 0;
				float c = 0;
				sincos(meta.a + Rot, s, c);

				//Rotation
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
					s, c, 0, 0,
					0, 0, 1, 0,
					0, 0, 0, 1
					);

				float4 BB = float4((v.texcoord1.x - 0.5) * Scale, (v.texcoord1.y - 0.5) * _AspectRatio * Scale, 0.0, 0.0);
				result1 = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(pos.xyz, 1.0)) - mul(rotateZMatrix, BB));
			#else
					float3 CamZ = normalize(UNITY_MATRIX_IT_MV[2].xyz);
					float3 CamY = normalize(UNITY_MATRIX_IT_MV[1].xyz);
					float3 CamX = normalize(UNITY_MATRIX_IT_MV[0].xyz);

					float2 Offset = v.texcoord1 * 2 - 1;

					result1.xyz = pos.xyz + (CamX * Offset.x * Scale) + (CamY * Offset.y * _AspectRatio * Scale);
					v.normal = CamZ;
					v.tangent = float4(1.0, 0.0, 0.0, -1.0);
			#endif
		#endif

		#ifdef BILLBOARD
			#ifdef ROTATION
				float3 CamZ = normalize(UNITY_MATRIX_IT_MV[2].xyz);
				float3 CamY = normalize(UNITY_MATRIX_IT_MV[1].xyz);
				float3 CamX = normalize(UNITY_MATRIX_IT_MV[0].xyz);

				float2 Offset = (v.texcoord1 * 2 - 1) * 0.5;

				result1.xyz = pos.xyz + RotateVertex((CamX * Offset.x * Scale) + (CamY * Offset.y * _AspectRatio * Scale), CamZ, meta.a + Rot);
				v.normal = CamZ;
				v.tangent = float4(RotateVertex(v.tangent.xyz, CamZ, meta.a + Rot), -1.0);
			#else
				float3 CamZ = normalize(UNITY_MATRIX_IT_MV[2].xyz);
				float3 CamY = normalize(UNITY_MATRIX_IT_MV[1].xyz);
				float3 CamX = normalize(UNITY_MATRIX_IT_MV[0].xyz);

				float2 Offset = v.texcoord1 * 2 - 1;

				result1.xyz = pos.xyz + (CamX * Offset.x * Scale) + (CamY * Offset.y * _AspectRatio * Scale);
				v.normal = CamZ;
				v.tangent.w = -1.0;
			#endif
		#endif

		#ifdef H_BILLBORD
			#ifdef ROTATION

				float s = 0;
				float c = 0;
				sincos(meta.a + Rot, s, c);

				//Rotation
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
					0, 1, 0, 0,
					-s, 0, c, 0,
					0, 0, 0, 1
					);

				float4 Offset = float4((v.texcoord1.x - 0.5) * Scale, 0.0, (v.texcoord1.y - 0.5) * _AspectRatio * Scale, 0.0);
				result1.xyz = pos.xyz + mul(rotateYMatrix, Offset).xyz;
				v.normal = float4(0.0, 1.0, 0.0, 0.0);
				v.tangent = float4(1.0, 0.0, 0.0, -1.0);
			#else	
				float4 Offset = float4((v.texcoord1.x - 0.5) * Scale, 0.0, (v.texcoord1.y - 0.5) * _AspectRatio * Scale, 0.0);
				result1.xyz = pos.xyz + Offset.xyz;
				v.normal = float4(0.0, 1.0, 0.0, 0.0);
				v.tangent = float4(1.0, 0.0, 0.0, -1.0);
			#endif
		#endif

		#ifdef V_BILLBOARD
			#ifdef ROTATION
				float s = 0;
				float c = 0;
				sincos(meta.a + Rot, s, c);

				//Rotation
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
					0, 1, 0, 0,
					-s, 0, c, 0,
					0, 0, 0, 1
					);

				float3 DirVector = float3(0, 1, 0);
				float3 CamVector = normalize(_WorldSpaceCameraPos - pos.xyz);
				float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

				pos.xyz += DirVector * ((v.texcoord1.x - 0.5)) * Scale;
				pos.xyz += (TangentVector * ((v.texcoord1.y - 0.5))) * Scale;

				result1 = UnityObjectToClipPos(float4(pos.xyz, 1.0));
			#else
				float3 DirVector = float3(0, 1, 0);
				float3 CamVector = normalize(_WorldSpaceCameraPos - pos.xyz);
				float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

				pos.xyz += DirVector * ((v.texcoord1.x - 0.5))*Scale;
				pos.xyz += (TangentVector * ((v.texcoord1.y - 0.5)))*Scale;
				result1 = pos;

				v.normal = float4(CamVector, 0.0);
				v.tangent = float4(1.0, 0.0, 0.0, -1.0);
			#endif
		#endif

		#ifdef TS_BILLBOARD
				float3 StretchedVector = vel.xyz * _VelocityScale;
				float3 UnstretchedVector = vel.xyz * Scale;
				float3 DirVector = lerp(UnstretchedVector, StretchedVector, v.vertex.x);

				float3 CamVector = normalize(_WorldSpaceCameraPos - pos.xyz);
				float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

				pos.xyz += DirVector * ((v.texcoord1.x - 0.5));
				pos.xyz += (TangentVector * ((v.texcoord1.y - 0.5))) * Scale;

				result1 = float4(pos.xyz, 1.0);
		#endif

		#ifdef S_BILLBOARD
			//Stretched Billboard
			float3 DirVector = vel.xyz * _VelocityScale;
			float3 CamVector = normalize(_WorldSpaceCameraPos - pos.xyz);
			float3 TangentVector = normalize(cross(DirVector, CamVector)) * _AspectRatio;

			pos.xyz += DirVector * ((v.texcoord1.x - 0.5));
			pos.xyz += (TangentVector * ((v.texcoord1.y - 0.5))) * Scale;

			result1 = float4(pos.xyz, 1.0);
		#endif
		
		#ifdef MESH
			//Rotate towards movement direction
			float3 dest = normalize(vel.xyz);
			float3 RotationAxis = normalize(cross(_ForwardVector.xyz, dest));
			float Angle = acos(dot(_ForwardVector.xyz, dest));
			v.vertex.xyz = RotateVertex(v.vertex.xyz, RotationAxis, Angle);
			v.normal = RotateVertex(v.normal.xyz, RotationAxis, Angle);
			v.tangent.xyz = RotateVertex(v.tangent.xyz, RotationAxis, Angle);
				
			//Scale and position update
			v.vertex.xyz *= Scale;
			v.vertex.xyz += pos.xyz;
			result1 = v.vertex;
		#endif

			o.uv2_MainTex = v.texcoord1;

			float4 volUV = float4(Progress, pos.a, 0.0, 0.0);
			o.tintColor = tex2Dlod(_ColorOverLifetime, volUV);

			float IsAlive = saturate((sign(_CustomTime - meta.g) + 1.0) / 2.0);
			v.vertex = lerp(result1, result2, IsAlive);
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 color = float4(1, 1, 1, 1);
			
			#if MAINTEX
				color *= tex2D(_MainTex, IN.uv2_MainTex);
			#endif
			
			o.Albedo = color * IN.tintColor;
			
			#if METALLIC_SMOOTHNESS
				fixed4 sm = tex2D(_MetallicSmoothness, IN.uv_SpecMap);
				o.Metallic = sm.r;
				o.Smoothness = sm.a * _Smoothness;
			#else
				o.Metallic = _Metallic;
				o.Smoothness = _Smoothness;
			#endif

			#if NORMAL_MAP
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv2_MainTex));
			#endif
		
			#if EMISSION_MAP
				o.Emission = tex2D(_Emission, IN.uv2_MainTex).rgb;
			#endif
		}

		ENDCG
	}

	FallBack "Diffuse"
}
