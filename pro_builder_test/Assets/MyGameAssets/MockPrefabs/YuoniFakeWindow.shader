// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/FakeWindow"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_MainTexBrightness("Main Tex Brightness", Float) = 2
		_MainTexDepthScale("Main Tex Depth Scale", Range( 0 , 1)) = 0
		_FrontTex("FrontTex", 2D) = "white" {}
		_FrontTexBrightness("Front Tex Brightness", Float) = 2
		_FrontTexDepthScale("Front Tex Depth Scale", Range( 0 , 1)) = 0.3
		_BumpMap("Normal Map", 2D) = "white" {}
		_BumpScale1("NormalPower", Range( -1 , 1)) = 1
		_MetallicGlossMap("MetallicAndMask", 2D) = "white" {}
		_Glossiness1("Smoothness", Range( 0 , 1)) = 0.8
		_emmisivPower("emmisivPower", Float) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		BlendOp Add
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 2.5
		#pragma surface surf StandardSpecular keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform half _BumpScale1;
		uniform sampler2D _BumpMap;
		uniform half4 _BumpMap_ST;
		uniform half _emmisivPower;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _MainTexDepthScale;
		uniform float _MainTexBrightness;
		uniform sampler2D _FrontTex;
		uniform float _FrontTexDepthScale;
		uniform float _FrontTexBrightness;
		uniform sampler2D _MetallicGlossMap;
		uniform half _Glossiness1;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _BumpScale1 );
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			half2 Offset75 = ( ( 0.0 - 1 ) * i.viewDir.xy * _MainTexDepthScale ) + uv0_MainTex;
			half2 Offset76 = ( ( 0.0 - 1 ) * i.viewDir.xy * _FrontTexDepthScale ) + i.uv_texcoord;
			half4 tex2DNode47 = tex2D( _MetallicGlossMap, Offset76 );
			half4 lerpResult48 = lerp( ( tex2D( _MainTex, Offset75 ) * _MainTexBrightness ) , ( tex2D( _FrontTex, Offset76 ) * _FrontTexBrightness ) , tex2DNode47.g);
			o.Emission = ( _emmisivPower * lerpResult48 ).rgb;
			half3 temp_cast_1 = (tex2DNode47.r).xxx;
			o.Specular = temp_cast_1;
			o.Smoothness = saturate( ( tex2DNode47.a * _Glossiness1 ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
0;22.4;1536;779;1905.229;211.4303;1.583119;True;True
Node;AmplifyShaderEditor.RangedFloatNode;49;-1432.349,748.5056;Float;False;Property;_FrontTexDepthScale;Front Tex Depth Scale;5;0;Create;True;0;0;False;0;0.3;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1447.827,395.4421;Float;False;Property;_MainTexDepthScale;Main Tex Depth Scale;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;130;-1313.144,95.69469;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;120;-1279.327,-105.2167;Inherit;False;0;45;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;51;-1376.858,522.8198;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxMappingNode;75;-953.2018,119.736;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-198.9563,694.609;Float;False;Property;_FrontTexBrightness;Front Tex Brightness;4;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;76;-947.2023,332.5014;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;98;-7.037296,568.9813;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-295.3827,475.2704;Inherit;True;Property;_FrontTex;FrontTex;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-201.183,381.858;Float;False;Property;_MainTexBrightness;Main Tex Brightness;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;47;-170.571,842.8131;Inherit;True;Property;_MetallicGlossMap;MetallicAndMask;8;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;45;-326.5037,181.3044;Inherit;True;Property;_MainTex;MainTex;0;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;115.598,454.4031;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;96;78.49758,648.8132;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;39.09225,1074.096;Inherit;False;Property;_Glossiness1;Smoothness;9;0;Create;False;0;0;False;0;0.8;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;112.1981,277.5027;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;48;316.0967,430.2044;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;124;415.9983,46.87455;Inherit;False;Property;_BumpScale1;NormalPower;7;0;Create;False;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;471.1416,310.3576;Inherit;False;Property;_emmisivPower;emmisivPower;10;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;440.5419,1013.725;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;711.5587,352.7751;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;128;583.7869,1008.37;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;123;707.8617,29.48403;Inherit;True;Property;_BumpMap;Normal Map;6;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1026.3,428.7931;Half;False;True;-1;1;ASEMaterialInspector;0;0;StandardSpecular;Yuoni/FakeWindow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;75;0;120;0
WireConnection;75;2;54;0
WireConnection;75;3;51;0
WireConnection;76;0;130;0
WireConnection;76;2;49;0
WireConnection;76;3;51;0
WireConnection;98;0;64;0
WireConnection;46;1;76;0
WireConnection;47;1;76;0
WireConnection;45;1;75;0
WireConnection;63;0;46;0
WireConnection;63;1;98;0
WireConnection;96;0;47;2
WireConnection;59;0;45;0
WireConnection;59;1;61;0
WireConnection;48;0;59;0
WireConnection;48;1;63;0
WireConnection;48;2;96;0
WireConnection;127;0;47;4
WireConnection;127;1;126;0
WireConnection;122;0;121;0
WireConnection;122;1;48;0
WireConnection;128;0;127;0
WireConnection;123;5;124;0
WireConnection;0;1;123;0
WireConnection;0;2;122;0
WireConnection;0;3;47;1
WireConnection;0;4;128;0
ASEEND*/
//CHKSM=630EB5DEAC710584F19C491114ACFF388C0F1030