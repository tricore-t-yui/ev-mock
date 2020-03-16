// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/TexChange"
{
	Properties
	{
		_Teex1("Teex1", 2D) = "white" {}
		_Normal1("Normal1", 2D) = "white" {}
		_Tex2("Tex2", 2D) = "white" {}
		_Normal2("Normal2", 2D) = "white" {}
		_Tex3("Tex3", 2D) = "white" {}
		_Normal3("Normal3", 2D) = "white" {}
		_TexChangeSpeed("TexChangeSpeed", Range( 0 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal3;
		uniform float4 _Normal3_ST;
		uniform sampler2D _Normal1;
		uniform float4 _Normal1_ST;
		uniform float _TexChangeSpeed;
		uniform sampler2D _Normal2;
		uniform float4 _Normal2_ST;
		uniform sampler2D _Tex3;
		uniform float4 _Tex3_ST;
		uniform sampler2D _Teex1;
		uniform float4 _Teex1_ST;
		uniform sampler2D _Tex2;
		uniform float4 _Tex2_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal3 = i.uv_texcoord * _Normal3_ST.xy + _Normal3_ST.zw;
			float4 tex2DNode42 = tex2D( _Normal3, uv_Normal3 );
			float2 uv_Normal1 = i.uv_texcoord * _Normal1_ST.xy + _Normal1_ST.zw;
			float time7 = ( frac( ( _TexChangeSpeed * _Time.y ) ) * 3.0 );
			float clampResult40 = clamp( time7 , 0.0 , 1.0 );
			float4 lerpResult43 = lerp( tex2DNode42 , tex2D( _Normal1, uv_Normal1 ) , clampResult40);
			float2 uv_Normal2 = i.uv_texcoord * _Normal2_ST.xy + _Normal2_ST.zw;
			float clampResult44 = clamp( ( time7 + -1.0 ) , 0.0 , 1.0 );
			float4 lerpResult48 = lerp( lerpResult43 , tex2D( _Normal2, uv_Normal2 ) , clampResult44);
			float clampResult47 = clamp( ( time7 + -2.0 ) , 0.0 , 1.0 );
			float4 lerpResult49 = lerp( lerpResult48 , tex2DNode42 , clampResult47);
			o.Normal = lerpResult49.rgb;
			float2 uv_Tex3 = i.uv_texcoord * _Tex3_ST.xy + _Tex3_ST.zw;
			float4 tex2DNode4 = tex2D( _Tex3, uv_Tex3 );
			float2 uv_Teex1 = i.uv_texcoord * _Teex1_ST.xy + _Teex1_ST.zw;
			float4 tex2DNode2 = tex2D( _Teex1, uv_Teex1 );
			float clampResult17 = clamp( time7 , 0.0 , 1.0 );
			float4 lerpResult26 = lerp( tex2DNode4 , tex2DNode2 , clampResult17);
			float2 uv_Tex2 = i.uv_texcoord * _Tex2_ST.xy + _Tex2_ST.zw;
			float4 tex2DNode3 = tex2D( _Tex2, uv_Tex2 );
			float clampResult18 = clamp( ( time7 + -1.0 ) , 0.0 , 1.0 );
			float4 lerpResult27 = lerp( lerpResult26 , tex2DNode3 , clampResult18);
			float clampResult23 = clamp( ( time7 + -2.0 ) , 0.0 , 1.0 );
			float4 lerpResult28 = lerp( lerpResult27 , tex2DNode4 , clampResult23);
			o.Albedo = lerpResult28.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1558;-144;1704;797;1999.855;-514.1691;2.060863;True;True
Node;AmplifyShaderEditor.CommentaryNode;6;-310.8802,-834.324;Inherit;False;871.5007;485.4033;1-texnum繰り返し;8;8;7;29;31;32;33;34;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;29;-272.1718,-542.5027;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-326.5656,-684.3355;Inherit;False;Property;_TexChangeSpeed;TexChangeSpeed;6;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-95.07062,-639.1008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;31;39.75774,-734.6287;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-219.6522,-445.5434;Inherit;False;Constant;_texnum;texnum;3;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;172.3794,-741.2322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;7;324.7839,-674.9004;Inherit;False;time;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-982.6036,1635.909;Inherit;False;Constant;_Float2;Float 2;3;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;9;-1102.929,33.85434;Inherit;False;7;time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-774.8331,514.6614;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-1310.699,1155.102;Inherit;False;7;time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-817.6421,1593.169;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;-738.7007,1183.924;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;39;-953.7614,979.9481;Inherit;True;Property;_Normal1;Normal1;1;0;Create;True;0;0;False;0;-1;f0d8945c7972e4747aa776e16494fa37;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-745.9908,-141.2992;Inherit;True;Property;_Teex1;Teex1;0;0;Create;True;0;0;False;0;-1;eb5f6e2757c821940b69cf1456f7865a;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-979.9716,2010.827;Inherit;False;Constant;_Float3;Float 3;3;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-681.4738,671.8854;Inherit;True;Property;_Tex3;Tex3;4;0;Create;True;0;0;False;0;-1;5b1a64ea234fb2343b8d0686c51280de;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-772.201,889.5794;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-609.8715,471.9219;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;17;-530.9302,62.67649;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;42;-889.2444,1793.133;Inherit;True;Property;_Normal3;Normal3;5;0;Create;True;0;0;False;0;-1;55e2334423fefa34fb4916f6780413d3;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-713.0876,216.2963;Inherit;True;Property;_Tex2;Tex2;2;0;Create;True;0;0;False;0;-1;49b611e658efbf443b686a4036f74fe3;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;18;-484.3741,473.3871;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;44;-692.1447,1594.634;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-36.98603,-134.6493;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;43;-244.7566,986.598;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;45;-494.2597,1253.048;Inherit;True;Property;_Normal2;Normal2;3;0;Create;True;0;0;False;0;-1;7bdd8cb386e0b554b84cb783663bb4fc;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-797.9933,2024.252;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-590.2227,903.0045;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-65.84364,1235.241;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;47;-672.4958,2025.717;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;141.9269,113.9933;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;23;-464.7253,904.4697;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;28;170.2059,394.7711;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-316.1879,213.1924;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-324.6473,-124.3284;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;49;-37.56464,1516.018;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-268.6361,-779.2527;Inherit;False;Constant;_debug;debug;3;0;Create;True;0;0;False;0;2.93;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-305.8813,620.3044;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;590.0549,-18.03661;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Yuoni/TexChange;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;34;0
WireConnection;35;1;29;0
WireConnection;31;0;35;0
WireConnection;32;0;31;0
WireConnection;32;1;33;0
WireConnection;7;0;32;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;40;0;36;0
WireConnection;15;0;9;0
WireConnection;15;1;19;0
WireConnection;17;0;9;0
WireConnection;18;0;15;0
WireConnection;44;0;38;0
WireConnection;26;0;4;0
WireConnection;26;1;2;0
WireConnection;26;2;17;0
WireConnection;43;0;42;0
WireConnection;43;1;39;0
WireConnection;43;2;40;0
WireConnection;46;0;36;0
WireConnection;46;1;41;0
WireConnection;22;0;9;0
WireConnection;22;1;13;0
WireConnection;48;0;43;0
WireConnection;48;1;45;0
WireConnection;48;2;44;0
WireConnection;47;0;46;0
WireConnection;27;0;26;0
WireConnection;27;1;3;0
WireConnection;27;2;18;0
WireConnection;23;0;22;0
WireConnection;28;0;27;0
WireConnection;28;1;4;0
WireConnection;28;2;23;0
WireConnection;20;0;3;0
WireConnection;20;1;18;0
WireConnection;10;0;2;0
WireConnection;10;1;17;0
WireConnection;49;0;48;0
WireConnection;49;1;42;0
WireConnection;49;2;47;0
WireConnection;21;0;4;0
WireConnection;21;1;23;0
WireConnection;0;0;28;0
WireConnection;0;1;49;0
ASEEND*/
//CHKSM=E2E6FBA2414ECBD0DEB2AE47188EC0823D19B8CD