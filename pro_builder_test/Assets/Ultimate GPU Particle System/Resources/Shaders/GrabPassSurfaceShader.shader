// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GrabShader" {

	SubShader{
		// Draw ourselves after all opaque geometry
		Tags{ "Queue" = "Transparent" }

		// Grab the screen behind the object into _MyGrabTexture
		GrabPass{ "_MyGrabTexture" }

		CGPROGRAM
#pragma surface surf Lambert vertex:vert
#pragma debug

		sampler2D _MainTex;
	sampler2D _MyGrabTexture;

	struct Input {
		float4 grabUV;
	};

	void vert(inout appdata_full v, out Input o) {
		float4 hpos = UnityObjectToClipPos(v.vertex);
		o.grabUV = ComputeGrabScreenPos(hpos);
	}


	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = tex2Dproj(_MyGrabTexture, UNITY_PROJ_COORD(IN.grabUV));
	}
	ENDCG

	}

}