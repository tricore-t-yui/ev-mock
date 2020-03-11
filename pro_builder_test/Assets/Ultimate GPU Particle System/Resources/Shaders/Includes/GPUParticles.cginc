//////////////////////////////////////////////////////////////////////////////////////////
//Math functions
//////////////////////////////////////////////////////////////////////////////////////////

float Random(float2 uv){
    return frac(sin(dot(uv.xy ,float2(12.9898,78.233))) * 43758.5453);
}

float Remap(float value, float From1, float To1, float From2, float To2)
{
	return From2 + (value - From1) * (To2 - From2) / (To1 - From1);
}

float4 ClampMagnitude(float4 Vector, float Magnitude)
{
	float mag = length(Vector.xyz);
	Vector.xyz = normalize(Vector.xyz);
	Vector.xyz *= clamp(mag, 0.0, Magnitude);
	return Vector;
}

#include "Emitter.cginc"
#include "Quaternion.cginc"
#include "Bezier.cginc"