// Upgrade NOTE: upgraded instancing buffer 'YuoniStandardOpaque' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/StandardOpaque"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_BumpMap("Normal Map", 2D) = "bump" {}
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_Glossiness("Smoothness", Float) = 0.8
		_OcclusionStrength1("Occlusion Strength", Range( 0 , 1)) = 1
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

		uniform sampler2D _BumpMap;
		uniform sampler2D _MainTex;
		uniform sampler2D _MetallicGlossMap;
		uniform float _Glossiness;
		uniform sampler2D _OcclusionMap;
		uniform float _OcclusionStrength1;

		UNITY_INSTANCING_BUFFER_START(YuoniStandardOpaque)
			UNITY_DEFINE_INSTANCED_PROP(float4, _BumpMap_ST)
#define _BumpMap_ST_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTex_ST)
#define _MainTex_ST_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _MetallicGlossMap_ST)
#define _MetallicGlossMap_ST_arr YuoniStandardOpaque
			UNITY_DEFINE_INSTANCED_PROP(float4, _OcclusionMap_ST)
#define _OcclusionMap_ST_arr YuoniStandardOpaque
		UNITY_INSTANCING_BUFFER_END(YuoniStandardOpaque)

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _BumpMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_BumpMap_ST_arr, _BumpMap_ST);
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST_Instance.xy + _BumpMap_ST_Instance.zw;
			o.Normal = UnpackNormal( tex2D( _BumpMap, uv_BumpMap ) );
			float4 _MainTex_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTex_ST_arr, _MainTex_ST);
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST_Instance.xy + _MainTex_ST_Instance.zw;
			float4 _Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			o.Albedo = ( tex2D( _MainTex, uv_MainTex ) * _Color_Instance ).rgb;
			float4 _MetallicGlossMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MetallicGlossMap_ST_arr, _MetallicGlossMap_ST);
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST_Instance.xy + _MetallicGlossMap_ST_Instance.zw;
			float4 tex2DNode4 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
			float3 temp_cast_1 = (tex2DNode4.r).xxx;
			o.Specular = temp_cast_1;
			o.Smoothness = saturate( ( tex2DNode4.a * _Glossiness ) );
			float4 _OcclusionMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_OcclusionMap_ST_arr, _OcclusionMap_ST);
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST_Instance.xy + _OcclusionMap_ST_Instance.zw;
			o.Occlusion = saturate( ( tex2D( _OcclusionMap, uv_OcclusionMap ).r * _OcclusionStrength1 ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1681;-178;1704;797;1134.123;231.0026;1.338738;True;True
Node;AmplifyShaderEditor.RangedFloatNode;14;-697.6943,174.6352;Inherit;False;Property;_Glossiness;Smoothness;5;0;Create;False;0;0;False;0;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-661.6782,-45.69917;Inherit;True;Property;_MetallicGlossMap;Metallic;3;0;Create;False;0;0;False;0;-1;None;6618005f6bafebf40b3d09f498401fba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-636.5329,566.8278;Inherit;False;Property;_OcclusionStrength1;Occlusion Strength;6;0;Create;False;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-580.1434,307.2937;Inherit;True;Property;_OcclusionMap;Occlusion;4;0;Create;False;0;0;False;0;-1;None;a8de9c9c15d9c7e4eaa883c727391bee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-333.5583,86.27843;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-485.093,-535.0509;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;-1;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-612.015,-391.6512;Float;False;InstancedProperty;_Color;Color;1;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-260.002,455.223;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-148.812,-375.5863;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;-190.3131,80.92344;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;18;-105.476,445.6647;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-470.3668,-182.4018;Inherit;True;Property;_BumpMap;Normal Map;2;0;Create;False;0;0;False;0;-1;None;11f03d9db1a617e40b7ece71f0a84f6f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;23,-97;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Yuoni/StandardOpaque;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;4;4
WireConnection;15;1;14;0
WireConnection;19;0;5;1
WireConnection;19;1;20;0
WireConnection;11;0;6;0
WireConnection;11;1;17;0
WireConnection;16;0;15;0
WireConnection;18;0;19;0
WireConnection;0;0;11;0
WireConnection;0;1;7;0
WireConnection;0;3;4;1
WireConnection;0;4;16;0
WireConnection;0;5;18;0
ASEEND*/
//CHKSM=24647976B7D31546A6FC08D8618D2DB4260AF2CF