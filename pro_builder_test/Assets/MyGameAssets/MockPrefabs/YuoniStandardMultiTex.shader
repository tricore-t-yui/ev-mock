// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/Standard MultiTex"
{
	Properties
	{
		_MainTex("Albedo", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range( 0 , 1)) = 0.8
		_MetallicGlossMap("Metallic", 2D) = "black" {}
		[Normal]_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpScale("NormalPower", Range( -1 , 1)) = 1
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionStrength2("Occlusion Strength", Range( 0 , 1)) = 1
		_Texture2nd("Texture2nd", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _BumpScale;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _Color;
		uniform sampler2D _Texture2nd;
		uniform float4 _Texture2nd_ST;
		uniform sampler2D _MetallicGlossMap;
		uniform float4 _MetallicGlossMap_ST;
		uniform float _Glossiness;
		uniform sampler2D _OcclusionMap;
		uniform float4 _OcclusionMap_ST;
		uniform float _OcclusionStrength2;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			o.Normal = saturate( UnpackScaleNormal( tex2D( _BumpMap, uv_BumpMap ), _BumpScale ) );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 blendOpSrc6 = tex2D( _MainTex, uv_MainTex );
			float4 blendOpDest6 = _Color;
			float2 uv0_Texture2nd = i.uv_texcoord * _Texture2nd_ST.xy + _Texture2nd_ST.zw;
			float4 tex2DNode54 = tex2D( _Texture2nd, uv0_Texture2nd );
			o.Albedo = ( ( ( saturate( ( blendOpSrc6 * blendOpDest6 ) )) * ( 1.0 - tex2DNode54.a ) ) + ( tex2DNode54 * tex2DNode54.a ) ).rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			float4 tex2DNode2 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
			float3 temp_cast_2 = (tex2DNode2.r).xxx;
			o.Specular = temp_cast_2;
			o.Smoothness = saturate( ( tex2DNode2.a * _Glossiness ) );
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			o.Occlusion = saturate( ( tex2D( _OcclusionMap, uv_OcclusionMap ).r * _OcclusionStrength2 ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Standard"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;6.4;1536;785;913.4037;190.6561;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;-952.22,-92.3047;Inherit;False;0;54;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-1208.2,-382.8;Float;False;Property;_Color;Color;1;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1240.9,-194.1;Inherit;True;Property;_MainTex;Albedo;0;0;Create;False;0;0;False;0;-1;3873092abd2d9f34d88cb9eb8486006a;3873092abd2d9f34d88cb9eb8486006a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;-650.467,-82.58124;Inherit;True;Property;_Texture2nd;Texture2nd;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1220.5,38.1;Inherit;True;Property;_MetallicGlossMap;Metallic;3;0;Create;False;0;0;False;0;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1200.12,257.0953;Inherit;True;Property;_BumpMap;Normal Map;4;1;[Normal];Create;False;0;0;False;0;-1;None;2c33ff667421dd549bcb8bc501f88ecb;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;64;-661.158,751.7239;Inherit;False;Property;_OcclusionStrength2;Occlusion Strength;7;0;Create;False;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-899.9509,425.5836;Inherit;False;Property;_BumpScale;NormalPower;5;0;Create;False;0;0;False;0;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;57;-639.467,-261.5812;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;-887.4812,542.7583;Inherit;True;Property;_OcclusionMap;Occlusion;6;0;Create;False;0;0;False;0;-1;None;a8de9c9c15d9c7e4eaa883c727391bee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-859.9995,188.448;Inherit;False;Property;_Glossiness;Smoothness;2;0;Create;False;0;0;False;0;0.8;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;6;-908.1999,-392.3998;Inherit;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;46;-569.6201,281.7954;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-575.1332,555.7784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-535.7197,150.5943;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-311.467,-104.5812;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-487.467,-426.5812;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;67;-320.7313,266.9222;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;62;-392.4747,145.2393;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-242.467,-352.5812;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;66;-420.6071,546.2201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;8.97832,-3.36687;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Yuoni/Standard MultiTex;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;Standard;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;54;1;53;0
WireConnection;57;0;54;4
WireConnection;6;0;1;0
WireConnection;6;1;5;0
WireConnection;46;0;41;0
WireConnection;46;1;7;0
WireConnection;65;0;63;1
WireConnection;65;1;64;0
WireConnection;61;0;2;4
WireConnection;61;1;60;0
WireConnection;59;0;54;0
WireConnection;59;1;54;4
WireConnection;55;0;6;0
WireConnection;55;1;57;0
WireConnection;67;0;46;0
WireConnection;62;0;61;0
WireConnection;58;0;55;0
WireConnection;58;1;59;0
WireConnection;66;0;65;0
WireConnection;0;0;58;0
WireConnection;0;1;67;0
WireConnection;0;3;2;1
WireConnection;0;4;62;0
WireConnection;0;5;66;0
ASEEND*/
//CHKSM=B48872D4497C7469008608E8F22664C73E501FFD