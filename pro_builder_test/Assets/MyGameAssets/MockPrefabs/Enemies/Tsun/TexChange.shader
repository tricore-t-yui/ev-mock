// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/TexChange"
{
	Properties
	{
		_Teex1("Teex1", 2D) = "white" {}
		_Tex2("Tex2", 2D) = "white" {}
		_Tex3("Tex3", 2D) = "white" {}
		_HazeMaskTickness("HazeMaskTickness", Range( 0 , 10)) = 1.475511
		_TexChangeSpeed("TexChangeSpeed", Range( 0 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Tex3;
		uniform float4 _Tex3_ST;
		uniform sampler2D _Teex1;
		uniform float4 _Teex1_ST;
		uniform float _TexChangeSpeed;
		uniform sampler2D _Tex2;
		uniform float4 _Tex2_ST;
		uniform float _HazeMaskTickness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Tex3 = i.uv_texcoord * _Tex3_ST.xy + _Tex3_ST.zw;
			float4 tex2DNode4 = tex2D( _Tex3, uv_Tex3 );
			float2 uv_Teex1 = i.uv_texcoord * _Teex1_ST.xy + _Teex1_ST.zw;
			float4 tex2DNode2 = tex2D( _Teex1, uv_Teex1 );
			float time7 = ( frac( ( _TexChangeSpeed * _Time.y ) ) * 3.0 );
			float clampResult17 = clamp( time7 , 0.0 , 1.0 );
			float4 lerpResult26 = lerp( tex2DNode4 , tex2DNode2 , clampResult17);
			float2 uv_Tex2 = i.uv_texcoord * _Tex2_ST.xy + _Tex2_ST.zw;
			float4 tex2DNode3 = tex2D( _Tex2, uv_Tex2 );
			float clampResult18 = clamp( ( time7 + -1.0 ) , 0.0 , 1.0 );
			float4 lerpResult27 = lerp( lerpResult26 , tex2DNode3 , clampResult18);
			float clampResult23 = clamp( ( time7 + -2.0 ) , 0.0 , 1.0 );
			float4 lerpResult28 = lerp( lerpResult27 , tex2DNode4 , clampResult23);
			o.Albedo = lerpResult28.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult53 = dot( ase_worldViewDir , ase_normWorldNormal );
			o.Alpha = ( 1.0 - cos( ( saturate( ( dotResult53 * _HazeMaskTickness ) ) * 1.570796 ) ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1681;-178;1704;797;651.4676;576.3628;2.64153;True;True
Node;AmplifyShaderEditor.CommentaryNode;6;-310.8802,-834.324;Inherit;False;871.5007;485.4033;1-texnum繰り返し;8;8;7;29;31;32;33;34;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-326.5656,-684.3355;Inherit;False;Property;_TexChangeSpeed;TexChangeSpeed;4;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;29;-272.1718,-542.5027;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-95.07062,-639.1008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;31;39.75774,-734.6287;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-219.6522,-445.5434;Inherit;False;Constant;_texnum;texnum;3;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;50;-1336.031,1517.067;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;172.3794,-741.2322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;51;-1337.031,1367.067;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;53;-1106.032,1429.067;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1243.001,1684.09;Inherit;False;Property;_HazeMaskTickness;HazeMaskTickness;3;0;Create;True;0;0;False;0;1.475511;1.15;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;7;324.7839,-674.9004;Inherit;False;time;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-774.8331,514.6614;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;9;-1102.929,33.85434;Inherit;False;7;time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-937.4847,1371.822;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-681.4738,671.8854;Inherit;True;Property;_Tex3;Tex3;2;0;Create;True;0;0;False;0;-1;5b1a64ea234fb2343b8d0686c51280de;5b1a64ea234fb2343b8d0686c51280de;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-609.8715,471.9219;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-745.9908,-141.2992;Inherit;True;Property;_Teex1;Teex1;0;0;Create;True;0;0;False;0;-1;eb5f6e2757c821940b69cf1456f7865a;eb5f6e2757c821940b69cf1456f7865a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;17;-530.9302,62.67649;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;56;-711.4508,1357.17;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-674.3538,1703.042;Inherit;False;Constant;_pihalf;pihalf;5;0;Create;True;0;0;False;0;1.570796;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-772.201,889.5794;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-36.98603,-134.6493;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;18;-484.3741,473.3871;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-713.0876,216.2963;Inherit;True;Property;_Tex2;Tex2;1;0;Create;True;0;0;False;0;-1;49b611e658efbf443b686a4036f74fe3;49b611e658efbf443b686a4036f74fe3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-590.2227,903.0045;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-498.0148,1500.817;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;58;-303.8807,1431.253;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;23;-464.7253,904.4697;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;141.9269,113.9933;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;28;170.2059,394.7711;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-324.6473,-124.3284;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;59;-42.46913,1443.017;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-316.1879,213.1924;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-305.8813,620.3044;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-268.6361,-779.2527;Inherit;False;Constant;_debug;debug;3;0;Create;True;0;0;False;0;2.93;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;590.0549,-18.03661;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Yuoni/TexChange;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;34;0
WireConnection;35;1;29;0
WireConnection;31;0;35;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;53;0;51;0
WireConnection;53;1;50;0
WireConnection;7;0;32;0
WireConnection;54;0;53;0
WireConnection;54;1;52;0
WireConnection;15;0;9;0
WireConnection;15;1;19;0
WireConnection;17;0;9;0
WireConnection;56;0;54;0
WireConnection;26;0;4;0
WireConnection;26;1;2;0
WireConnection;26;2;17;0
WireConnection;18;0;15;0
WireConnection;22;0;9;0
WireConnection;22;1;13;0
WireConnection;57;0;56;0
WireConnection;57;1;55;0
WireConnection;58;0;57;0
WireConnection;23;0;22;0
WireConnection;27;0;26;0
WireConnection;27;1;3;0
WireConnection;27;2;18;0
WireConnection;28;0;27;0
WireConnection;28;1;4;0
WireConnection;28;2;23;0
WireConnection;10;0;2;0
WireConnection;10;1;17;0
WireConnection;59;0;58;0
WireConnection;20;0;3;0
WireConnection;20;1;18;0
WireConnection;21;0;4;0
WireConnection;21;1;23;0
WireConnection;0;0;28;0
WireConnection;0;9;59;0
ASEEND*/
//CHKSM=470A75B86EECF54CFEBF929CA740143251881269