// Upgrade NOTE: upgraded instancing buffer 'YuoniStandardOpaque' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/StandardOpaque"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Color1("Color", Color) = (1,1,1,1)
		_Normals("Normals", 2D) = "bump" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Occlusion("Occlusion", 2D) = "white" {}
		_Smoothness("Smoothness", Float) = 0.8
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normals;
		uniform sampler2D _Albedo;
		uniform sampler2D _Metallic;
		uniform float _Smoothness;
		uniform sampler2D _Occlusion;

		UNITY_INSTANCING_BUFFER_START(YuoniStandardOpaque)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Normals_ST)
#define _Normals_ST_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _Albedo_ST)
#define _Albedo_ST_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color1)
#define _Color1_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _Metallic_ST)
#define _Metallic_ST_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _Occlusion_ST)
#define _Occlusion_ST_arr YuoniStandardOpaque
		UNITY_INSTANCING_BUFFER_END(YuoniStandardOpaque)

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normals_ST_arr, _Normals_ST);
			float2 uv_Normals = i.uv_texcoord * _Normals_ST_Instance.xy + _Normals_ST_Instance.zw;
			o.Normal = UnpackNormal( tex2D( _Normals, uv_Normals ) );
			float4 _Albedo_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Albedo_ST_arr, _Albedo_ST);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST_Instance.xy + _Albedo_ST_Instance.zw;
			float4 _Color1_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color1_arr, _Color1);
			o.Albedo = ( tex2D( _Albedo, uv_Albedo ) * _Color1_Instance ).rgb;
			float4 _Metallic_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Metallic_ST_arr, _Metallic_ST);
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST_Instance.xy + _Metallic_ST_Instance.zw;
			float4 tex2DNode4 = tex2D( _Metallic, uv_Metallic );
			float3 temp_cast_1 = (tex2DNode4.r).xxx;
			o.Specular = temp_cast_1;
			o.Smoothness = saturate( ( tex2DNode4.a * _Smoothness ) );
			float4 _Occlusion_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Occlusion_ST_arr, _Occlusion_ST);
			float2 uv_Occlusion = i.uv_texcoord * _Occlusion_ST_Instance.xy + _Occlusion_ST_Instance.zw;
			o.Occlusion = tex2D( _Occlusion, uv_Occlusion ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1681;-178;1704;797;1205.076;516.1536;1.338738;True;True
Node;AmplifyShaderEditor.SamplerNode;4;-661.6782,-45.69917;Inherit;True;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;-1;None;6618005f6bafebf40b3d09f498401fba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-697.6943,174.6352;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-485.093,-535.0509;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;-1;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-475.4643,-347.4731;Float;False;InstancedProperty;_Color1;Color;1;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-333.5583,86.27843;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-470.3668,217.5982;Inherit;True;Property;_Occlusion;Occlusion;4;0;Create;True;0;0;False;0;-1;None;a8de9c9c15d9c7e4eaa883c727391bee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-470.3668,-182.4018;Inherit;True;Property;_Normals;Normals;2;0;Create;True;0;0;False;0;-1;None;11f03d9db1a617e40b7ece71f0a84f6f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-148.812,-375.5863;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;-190.3131,80.92344;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;23,-97;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Yuoni/StandardOpaque;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;4;4
WireConnection;15;1;14;0
WireConnection;11;0;6;0
WireConnection;11;1;8;0
WireConnection;16;0;15;0
WireConnection;0;0;11;0
WireConnection;0;1;7;0
WireConnection;0;3;4;1
WireConnection;0;4;16;0
WireConnection;0;5;5;0
ASEEND*/
//CHKSM=E4D13CBA945405408ADD513A45D64F609AB52208