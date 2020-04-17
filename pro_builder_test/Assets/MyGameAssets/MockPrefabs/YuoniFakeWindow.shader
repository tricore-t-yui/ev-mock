// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/FakeWindow"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_MainTexBrightness("Main Tex Brightness", Float) = 2
		_MainTexDepthScale("Main Tex Depth Scale", Range( 0 , 1)) = 0
		_BumpMap("Normal Map", 2D) = "bump" {}
		_FrontTex("FrontTex", 2D) = "white" {}
		_BumpScale2("NormalPower", Range( -1 , 1)) = 1
		_FrontTexBrightness("Front Tex Brightness", Float) = 2
		_FrontTexDepthScale("Front Tex Depth Scale", Range( 0 , 1)) = 0.3
		_Mask("Mask", 2D) = "white" {}
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		_Glossiness("Smoothness", Range( 0 , 1)) = 0.8
		_emmisivPower("emmisivPower", Float) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZTest LEqual
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf StandardSpecular keepalpha noshadow 
		struct Input
		{
			float3 worldRefl;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 viewDir;
		};

		uniform float _BumpScale2;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _emmisivPower;
		uniform sampler2D _MainTex;
		uniform float _MainTexDepthScale;
		uniform float _MainTexBrightness;
		uniform sampler2D _FrontTex;
		uniform float _FrontTexDepthScale;
		uniform float _FrontTexBrightness;
		uniform sampler2D _Mask;
		uniform sampler2D _MetallicGlossMap;
		uniform float _Glossiness;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			o.Normal = WorldReflectionVector( i , UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _BumpScale2 ) );
			float2 Offset27 = ( ( 0.0 - 1 ) * i.viewDir.xy * _MainTexDepthScale ) + i.uv_texcoord;
			float2 Offset29 = ( ( 0.0 - 1 ) * i.viewDir.xy * _FrontTexDepthScale ) + i.uv_texcoord;
			float4 lerpResult43 = lerp( ( tex2D( _MainTex, Offset27 ) * _MainTexBrightness ) , ( tex2D( _FrontTex, Offset29 ) * _FrontTexBrightness ) , tex2D( _Mask, Offset29 ).g);
			o.Emission = ( _emmisivPower * lerpResult43 ).rgb;
			float4 tex2DNode38 = tex2D( _MetallicGlossMap, i.uv_texcoord );
			float3 temp_cast_1 = (tex2DNode38.r).xxx;
			o.Specular = temp_cast_1;
			o.Smoothness = saturate( ( tex2DNode38.a * _Glossiness ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
0;28;1536;775;1402.736;599.4424;1.700508;True;True
Node;AmplifyShaderEditor.RangedFloatNode;22;-2352.366,446.7194;Float;False;Property;_FrontTexDepthScale;Front Tex Depth Scale;7;0;Create;True;0;0;False;0;0.3;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-2233.161,-206.0915;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-2199.344,-407.0029;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;26;-2296.875,221.0336;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;23;-2367.844,93.65594;Float;False;Property;_MainTexDepthScale;Main Tex Depth Scale;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;27;-1873.219,-182.0502;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxMappingNode;29;-1867.219,30.71521;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1118.974,392.8228;Float;False;Property;_FrontTexBrightness;Front Tex Brightness;6;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-1215.4,173.4841;Inherit;True;Property;_FrontTex;FrontTex;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-1164.995,480.8682;Inherit;True;Property;_Mask;Mask;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-1246.521,-120.4818;Inherit;True;Property;_MainTex;MainTex;0;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-794.5097,483.5256;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;33;-927.0544,267.1951;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1124.055,80.07184;Float;False;Property;_MainTexBrightness;Main Tex Brightness;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-880.9248,772.3096;Inherit;False;Property;_Glossiness;Smoothness;10;0;Create;False;0;0;False;0;0.8;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;36;-841.5195,347.027;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-569.3008,435.072;Inherit;True;Property;_MetallicGlossMap;Metallic;9;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-649.356,-260.1454;Inherit;False;Property;_BumpScale2;NormalPower;5;0;Create;False;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-807.8188,-24.28352;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-804.4189,152.6168;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;43;-603.9202,128.4182;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-479.4751,711.9387;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-448.8754,8.571409;Inherit;False;Property;_emmisivPower;emmisivPower;11;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-321.6229,-321.9903;Inherit;True;Property;_BumpMap;Normal Map;3;0;Create;False;0;0;False;0;-1;None;c2f6910db685c904c8211b2d570e2ec7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;45;-336.23,706.5837;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-208.4582,50.98889;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldReflectionVector;54;10.38672,-303.5539;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;23,-97;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Yuoni/FakeWindow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;25;0
WireConnection;27;2;23;0
WireConnection;27;3;26;0
WireConnection;29;0;24;0
WireConnection;29;2;22;0
WireConnection;29;3;26;0
WireConnection;34;1;29;0
WireConnection;32;1;29;0
WireConnection;30;1;27;0
WireConnection;33;0;28;0
WireConnection;36;0;32;2
WireConnection;38;1;35;0
WireConnection;39;0;30;0
WireConnection;39;1;31;0
WireConnection;37;0;34;0
WireConnection;37;1;33;0
WireConnection;43;0;39;0
WireConnection;43;1;37;0
WireConnection;43;2;36;0
WireConnection;44;0;38;4
WireConnection;44;1;40;0
WireConnection;46;5;41;0
WireConnection;45;0;44;0
WireConnection;47;0;42;0
WireConnection;47;1;43;0
WireConnection;54;0;46;0
WireConnection;0;1;54;0
WireConnection;0;2;47;0
WireConnection;0;3;38;1
WireConnection;0;4;45;0
ASEEND*/
//CHKSM=DF3BC51EB04A6750B6264BF555706D91382ED006