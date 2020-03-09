// Upgrade NOTE: upgraded instancing buffer 'YuoniShadowHumanBody' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yuoni/ShadowHumanBody"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0.4980392)
		_shadowClamp("shadowClamp", Range( 0 , 1)) = 0.625
		_shadowScale("shadowScale", Range( 0 , 10)) = 3.6
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _shadowClamp;
		uniform float _shadowScale;

		UNITY_INSTANCING_BUFFER_START(YuoniShadowHumanBody)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr YuoniShadowHumanBody
		UNITY_INSTANCING_BUFFER_END(YuoniShadowHumanBody)

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
			float rimPower65 = saturate( ( saturate( ( dotResult9 - _shadowClamp ) ) * _shadowScale ) );
			float4 temp_output_13_0 = ( _Color_Instance * rimPower65 );
			o.Emission = temp_output_13_0.rgb;
			float temp_output_21_0 = ( _Color_Instance.a * rimPower65 );
			o.Alpha = temp_output_21_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows exclude_path:deferred 

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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
1536;-302;1920;1019;761.3352;29.88586;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;12;-372.7897,-266.1754;Inherit;False;1183.942;414.3351;viewとworldで内側向きノーマル;10;65;15;14;26;25;9;7;8;80;84;viewとworldで内側向きノーマル;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-209.7897,-192.1754;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;8;-208.7897,-42.17542;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;25;-187.4609,91.18901;Inherit;False;Property;_shadowClamp;shadowClamp;2;0;Create;True;0;0;False;0;0.625;-0.33;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;9;21.21033,-130.1754;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;149.0854,-72.00089;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;157.8705,37.70864;Inherit;False;Property;_shadowScale;shadowScale;3;0;Create;True;0;0;False;0;3.6;1.45;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;144.0039,-198.8363;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;327.2705,-193.8913;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;80;621.9117,-131.3082;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;808.2809,-113.0947;Inherit;False;rimPower;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;90;874.0785,1093.039;Inherit;False;830.728;358.1541;Distortion;4;94;93;92;91;Distortion Anim Speed;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;57;-1004.511,1074.449;Inherit;False;1756.102;881.4008;GrabScreen;10;72;74;67;49;46;44;75;41;77;76;GrabScreen;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;2;-341.788,211.8268;Float;False;InstancedProperty;_Color;Color;1;0;Create;True;0;0;False;0;0,0,0,0.4980392;0,0,0,0.4980392;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;95;886.3769,1541.602;Inherit;False;1223.975;464.9008;Distortion;11;106;105;104;103;102;101;100;99;98;97;96;Distortion;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-412.7857,424.5604;Inherit;False;65;rimPower;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;77;-540.9177,1432.709;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;41;-533.2482,1134.796;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-220.0662,1503.296;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;1860.352,1690.075;Float;False;VertexOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;975.5048,1848.435;Float;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;1240.207,1266.452;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-145.2751,1177.978;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;173.4119,340.5917;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-977.5371,1418.401;Inherit;False;Property;_grabScreenScale;grabScreenScale;4;0;Create;True;0;0;False;0;8.9;1.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;49;229.9591,1362.901;Inherit;False;FLOAT3;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;76;-735.6793,1429.275;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;10;False;3;FLOAT;10;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;106;936.3768,1750.219;Inherit;False;94;ShieldSpeed;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-503.5577,1748.992;Inherit;False;Property;_grabScreenLength;grabScreenLength;5;0;Create;True;0;0;False;0;0.09;1.45;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;1456.807,1277.588;Float;False;ShieldSpeed;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;178.4788,204.3174;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TimeNode;91;997.6708,1143.038;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;101;1474.686,1636.807;Inherit;False;Simplex3D;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;1171.155,1891.503;Float;False;Property;_DistortionPow;DistortionPow;7;0;Create;True;0;0;False;0;0.01;0;0;0.03;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;104;1664.055,1682.044;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.01;False;4;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;1328.893,1621.469;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;107;356.6906,514.4246;Inherit;False;105;VertexOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;83;14.81186,386.0919;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-135.7193,444.0634;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;14.19583,552.6763;Inherit;False;67;noisedScreenColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;98;1160.292,1778.878;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalVertexDataNode;97;1065.525,1591.602;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;1494.655,1734.404;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;924.0786,1336.193;Float;False;Property;_DistotionSpeed;DistotionSpeed;6;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;46;13.95433,1270.096;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;Object;-1;True;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;102;1586.552,1883.975;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;452.3942,1362.29;Inherit;False;noisedScreenColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;387.9073,246.9742;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;627.0748,199.0353;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Yuoni/ShadowHumanBody;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;150;False;-1;255;False;-1;255;False;-1;1;False;-1;3;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;84;0;9;0
WireConnection;84;1;25;0
WireConnection;26;0;84;0
WireConnection;15;0;26;0
WireConnection;15;1;14;0
WireConnection;80;0;15;0
WireConnection;65;0;80;0
WireConnection;77;3;76;0
WireConnection;75;0;77;0
WireConnection;75;1;74;0
WireConnection;105;0;104;0
WireConnection;93;0;91;0
WireConnection;93;1;92;0
WireConnection;44;0;41;0
WireConnection;44;1;75;0
WireConnection;82;0;83;0
WireConnection;82;1;68;0
WireConnection;49;0;46;0
WireConnection;76;0;72;0
WireConnection;94;0;93;0
WireConnection;13;0;2;0
WireConnection;13;1;66;0
WireConnection;101;0;100;0
WireConnection;104;0;101;0
WireConnection;104;3;103;0
WireConnection;104;4;102;0
WireConnection;100;0;97;0
WireConnection;100;1;98;0
WireConnection;83;0;21;0
WireConnection;21;0;2;4
WireConnection;21;1;66;0
WireConnection;98;0;106;0
WireConnection;98;1;96;0
WireConnection;103;0;99;0
WireConnection;46;0;44;0
WireConnection;102;0;99;0
WireConnection;67;0;49;0
WireConnection;81;0;13;0
WireConnection;81;1;82;0
WireConnection;6;2;13;0
WireConnection;6;9;21;0
ASEEND*/
//CHKSM=37D82E3EAC24D37591E7EB5E843AC7F177258770