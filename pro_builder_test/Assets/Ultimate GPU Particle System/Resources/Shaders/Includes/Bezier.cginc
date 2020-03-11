//////////////////////////////////////////////////////////////////////////////////////////
//Bezier math
//////////////////////////////////////////////////////////////////////////////////////////

float2 BezierGetPosition(float Progress, float2 P1, float2 C1, float2 P2, float2 C2)
{
	float NegProgress = 1 - Progress;
    float x = NegProgress * NegProgress * NegProgress * P1.x + 3 * NegProgress * NegProgress * Progress * C1.x + 3 * NegProgress * Progress * Progress * C2.x + Progress * Progress * Progress * P2.x;
    float y = NegProgress * NegProgress * NegProgress * P1.y + 3 * NegProgress * NegProgress * Progress * C1.y + 3 * NegProgress * Progress * Progress * C2.y + Progress * Progress * Progress * P2.y;

	float2 Vec = float2(x,y);
    return Vec;
}

inline float2 EvaluateBezier(float4 StartCtrl1, in float4 Strl2End, float _Progress)
{
	float2 Q1 = lerp(StartCtrl1.xy, StartCtrl1.zw, _Progress);
	float2 Q2 = lerp(StartCtrl1.zw, Strl2End.xy, _Progress);
	float2 Q3 = lerp(Strl2End.xy, Strl2End.zw, _Progress);

	float2 R1 = lerp(Q1, Q2, _Progress);
	float2 R2 = lerp(Q2, Q3, _Progress);

	float2 R = lerp(R1, R2, _Progress);

	return R;
}