// Upgrade NOTE: upgraded instancing buffer 'YuoniShadowHumanBody' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/ShadowHumanBody"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0.4980392)
		_ShadowClamp("ShadowClamp", Range( 0 , 1)) = 0.625
		_ShadowScale("ShadowScale", Range( 0 , 10)) = 3.6
		[NoScaleOffset]_HazeNormal("HazeNormal", 2D) = "bump" {}
		_HazeNormalScale("Haze Normal Scale", Range( 0 , 1)) = 0.04
		_HazeMaskTickness("HazeMaskTickness", Range( 0 , 10)) = 1.475511
		_HazeVSpeed("HazeVSpeed", Float) = 0.08
		_HazeHFreq("HazeHFreq", Float) = 0
		_HazeHAmp("HazeHAmp", Float) = 0.005
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#include "Assets/VertExmotion/Shaders/VertExmotion.cginc"
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform float _ShadowClamp;
		uniform float _ShadowScale;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _HazeNormalScale;
		uniform float _HazeMaskTickness;
		uniform sampler2D _HazeNormal;
		uniform float _HazeHAmp;
		uniform float _HazeHFreq;
		uniform float _HazeVSpeed;

		UNITY_INSTANCING_BUFFER_START(YuoniShadowHumanBody)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr YuoniShadowHumanBody
		UNITY_INSTANCING_BUFFER_END(YuoniShadowHumanBody)


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += VertExmotionASE(v);
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 _Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult9 = dot( ase_worldViewDir , ase_normWorldNormal );
			float rimPower65 = saturate( ( saturate( ( dotResult9 - _ShadowClamp ) ) * _ShadowScale ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float dotResult213 = dot( ase_worldViewDir , ase_normWorldNormal );
			float temp_output_216_0 = ( saturate( ( dotResult213 * _HazeMaskTickness ) ) * 1.570796 );
			float temp_output_222_0 = ( 1.0 - cos( temp_output_216_0 ) );
			float mulTime196 = _Time.y * _HazeHFreq;
			float mulTime199 = _Time.y * _HazeVSpeed;
			float2 appendResult193 = (float2(( _HazeHAmp * cos( mulTime196 ) ) , -mulTime199));
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 screenColor155 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm + float4( ( UnpackScaleNormal( tex2D( _HazeNormal, ( appendResult193 + (ase_screenPosNorm).xy ) ), ( _HazeNormalScale * temp_output_222_0 * _Color_Instance.a ) ) * temp_output_222_0 ) , 0.0 ) ).xy/( ase_grabScreenPosNorm + float4( ( UnpackScaleNormal( tex2D( _HazeNormal, ( appendResult193 + (ase_screenPosNorm).xy ) ), ( _HazeNormalScale * temp_output_222_0 * _Color_Instance.a ) ) * temp_output_222_0 ) , 0.0 ) ).w);
			o.Emission = ( ( 1.0 - ( _Color_Instance.a * rimPower65 ) ) * screenColor155 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
6.4;5.6;1524;798;347.3827;-34.59073;1;True;True
Node;AmplifyShaderEditor.WorldNormalVector;214;-2520.35,1646.122;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;215;-2521.35,1496.122;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;12;-372.7897,-266.1754;Inherit;False;1183.942;414.3351;viewとworldで内側向きノーマル;10;65;15;14;26;25;9;7;8;80;84;viewとworldで内側向きノーマル;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-2427.32,1813.145;Inherit;False;Property;_HazeMaskTickness;HazeMaskTickness;5;0;Create;True;0;0;False;0;1.475511;1.15;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;213;-2290.351,1558.122;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;194;-2255.271,635.8094;Float;False;Property;_HazeHFreq;HazeHFreq;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;211;-2121.804,1500.877;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-209.7897,-192.1754;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;8;-208.7897,-42.17542;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;219;-1858.673,1832.097;Inherit;False;Constant;_pihalf;pihalf;5;0;Create;True;0;0;False;0;1.570796;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-187.4609,91.18901;Inherit;False;Property;_ShadowClamp;ShadowClamp;1;0;Create;True;0;0;False;0;0.625;0.082;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;9;21.21033,-130.1754;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;196;-2061.921,664.4595;Inherit;False;1;0;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;212;-1895.77,1486.225;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-2287.376,789.2036;Float;False;Property;_HazeVSpeed;HazeVSpeed;6;0;Create;True;0;0;False;0;0.08;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-2203.271,443.8094;Float;False;Property;_HazeHAmp;HazeHAmp;8;0;Create;True;0;0;False;0;0.005;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;198;-1817.776,672.1029;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;199;-2082.677,805.5038;Inherit;False;1;0;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-1682.334,1629.872;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;149.0854,-72.00089;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;218;-1488.2,1560.308;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;157.8705,37.70864;Inherit;False;Property;_ShadowScale;ShadowScale;2;0;Create;True;0;0;False;0;3.6;1.15;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;203;-2105.421,1082.277;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;200;-1810.278,750.4079;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-1660.271,541.8094;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;144.0039,-198.8363;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-1454.065,1094.98;Float;False;Property;_HazeNormalScale;Haze Normal Scale;4;0;Create;True;0;0;False;0;0.04;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;193;-1568.571,773.0089;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;204;-1849.42,1082.277;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;2;-858.9602,112.0823;Float;False;InstancedProperty;_Color;Color;0;0;Create;True;0;0;False;0;0,0,0,0.4980392;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;327.2705,-193.8913;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;222;-1289.212,1524.716;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;224;-1405.829,921.3208;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;80;621.9117,-131.3082;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-1124.919,984.2032;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;808.2809,-113.0947;Inherit;False;rimPower;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;145;-652.3197,615.3743;Inherit;False;681.9003;359.4998;Simple Refraction with normal perturbance;3;155;153;148;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;202;-983.718,830.2751;Inherit;True;Property;_HazeNormal;HazeNormal;3;1;[NoScaleOffset];Create;True;0;0;False;0;-1;d3de8e37d10aad442b977001befd23e0;d3de8e37d10aad442b977001befd23e0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;66;-463.5739,442.83;Inherit;False;65;rimPower;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;148;-602.3201,665.374;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-405.5648,1073.501;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;-355.1196,787.2742;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-120.8137,247.7508;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;164;120.9069,217.2417;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;155;-187.4196,762.874;Float;False;Global;_GrabScreen0;Grab Screen 0;1;0;Create;True;0;0;False;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;373.8884,413.5206;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;221;-1494.671,1752.825;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertExmotionASENode;226;335.5175,678.4338;Inherit;False;0;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;138;627.0748,200.0353;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Yuoni/ShadowHumanBody;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;False;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;1;Include;Assets/VertExmotion/Shaders/VertExmotion.cginc;False;;Custom;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;213;0;215;0
WireConnection;213;1;214;0
WireConnection;211;0;213;0
WireConnection;211;1;210;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;196;0;194;0
WireConnection;212;0;211;0
WireConnection;198;0;196;0
WireConnection;199;0;195;0
WireConnection;216;0;212;0
WireConnection;216;1;219;0
WireConnection;84;0;9;0
WireConnection;84;1;25;0
WireConnection;218;0;216;0
WireConnection;200;0;199;0
WireConnection;192;0;197;0
WireConnection;192;1;198;0
WireConnection;26;0;84;0
WireConnection;193;0;192;0
WireConnection;193;1;200;0
WireConnection;204;0;203;0
WireConnection;15;0;26;0
WireConnection;15;1;14;0
WireConnection;222;0;218;0
WireConnection;224;0;193;0
WireConnection;224;1;204;0
WireConnection;80;0;15;0
WireConnection;223;0;139;0
WireConnection;223;1;222;0
WireConnection;223;2;2;4
WireConnection;65;0;80;0
WireConnection;202;1;224;0
WireConnection;202;5;223;0
WireConnection;188;0;202;0
WireConnection;188;1;222;0
WireConnection;153;0;148;0
WireConnection;153;1;188;0
WireConnection;13;0;2;4
WireConnection;13;1;66;0
WireConnection;164;0;13;0
WireConnection;155;0;153;0
WireConnection;161;0;164;0
WireConnection;161;1;155;0
WireConnection;221;0;216;0
WireConnection;138;2;161;0
WireConnection;138;11;226;0
ASEEND*/
//CHKSM=6D5257657134D8D47B7198A073714109C7FB415B