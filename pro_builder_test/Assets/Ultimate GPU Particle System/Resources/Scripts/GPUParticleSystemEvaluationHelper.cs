using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPUParticleSystemEvaluationHelper
{
	private static BoxCollider farPlaneCollider;
	private static Ray ray;
	private static RaycastHit rayHit;

	#region Evaluation
	public static Matrix4x4 CameraMVPMatrix(Transform t, Camera c)
	{
		return GL.GetGPUProjectionMatrix(c.projectionMatrix, false) * c.worldToCameraMatrix * t.localToWorldMatrix;
	}

	public static float EvaluateSingleFloatCurveBundle(SingleFloatCurveBundle bundle, float progress)
    {
        switch (bundle.mode)
        {
            case GPUParticleSystem.SimpleValueMode.Value:
                return bundle.value;

            case GPUParticleSystem.SimpleValueMode.Curve:
                return bundle.curve.Evaluate(progress);

            default:
                return bundle.value;
        }
    }

    public static float EvaluateFloatCurveBundle(FloatCurveBundle bundle, float progress)
    {
        switch (bundle.mode)
        {
            case GPUParticleSystem.ValueMode.Value:
                return bundle.value1;

            case GPUParticleSystem.ValueMode.RandomTwoValues:
                return Mathf.Lerp(bundle.value1, bundle.value2, bundle.seed);

            case GPUParticleSystem.ValueMode.Curve:
                return bundle.curve1.Evaluate(progress);

            case GPUParticleSystem.ValueMode.RandomTwoCurves:
                return Mathf.Lerp(bundle.curve1.Evaluate(progress), bundle.curve2.Evaluate(progress), bundle.seed);

            default:
                return bundle.value1;
        }
    }

    public static Vector4 EvaluateStartSizeRotation(FloatCurveBundle startSize, FloatCurveBundle startRotation, float progress)
    {
        Vector4 startSizeRot = new Vector4();

        switch (startSize.mode)
        {
            case GPUParticleSystem.ValueMode.Value:
                startSizeRot.x = startSizeRot.y = startSize.value1;
                break;

            case GPUParticleSystem.ValueMode.RandomTwoValues:
                startSizeRot.x = startSize.value1;
                startSizeRot.y = startSize.value2;
                break;

            case GPUParticleSystem.ValueMode.Curve:
                startSizeRot.x = startSizeRot.y = startSize.curve1.Evaluate(progress);
                break;

            case GPUParticleSystem.ValueMode.RandomTwoCurves:
                startSizeRot.x = startSize.curve1.Evaluate(progress);
                startSizeRot.y = startSize.curve2.Evaluate(progress);
                break;
        }

        switch (startRotation.mode)
        {
            case GPUParticleSystem.ValueMode.Value:
                startSizeRot.z = startSizeRot.w = startRotation.value1 * Mathf.Deg2Rad;
                break;

            case GPUParticleSystem.ValueMode.RandomTwoValues:
                startSizeRot.z = startRotation.value1 * Mathf.Deg2Rad;
                startSizeRot.w = startRotation.value2 * Mathf.Deg2Rad;
                break;

            case GPUParticleSystem.ValueMode.Curve:
                startSizeRot.z = startSizeRot.w = startRotation.curve1.Evaluate(progress) * Mathf.Deg2Rad;
                break;

            case GPUParticleSystem.ValueMode.RandomTwoCurves:
                startSizeRot.z = startRotation.curve1.Evaluate(progress) * Mathf.Deg2Rad;
                startSizeRot.w = startRotation.curve2.Evaluate(progress) * Mathf.Deg2Rad;
                break;
        }

        return startSizeRot;
    }

    public static Vector4 EvaluateLifeTimeStartSpeed(FloatCurveBundle startLifetime, FloatCurveBundle startSpeed, float progress)
    {
        Vector4 startLifeTimeSpeed = new Vector4();

        switch (startLifetime.mode)
        {
            case GPUParticleSystem.ValueMode.Value:
                startLifeTimeSpeed.x = startLifeTimeSpeed.y = startLifetime.value1;
                break;

            case GPUParticleSystem.ValueMode.RandomTwoValues:
                startLifeTimeSpeed.x = startLifetime.value1;
                startLifeTimeSpeed.y = startLifetime.value2;
                break;

            case GPUParticleSystem.ValueMode.Curve:
                startLifeTimeSpeed.x = startLifeTimeSpeed.y = startLifetime.curve1.Evaluate(progress);
                break;

            case GPUParticleSystem.ValueMode.RandomTwoCurves:
                startLifeTimeSpeed.x = startLifetime.curve1.Evaluate(progress);
                startLifeTimeSpeed.y = startLifetime.curve1.Evaluate(progress);
                break;
        }

        switch (startSpeed.mode)
        {
            case GPUParticleSystem.ValueMode.Value:
                startLifeTimeSpeed.z = startLifeTimeSpeed.w = startSpeed.value1;
                break;

            case GPUParticleSystem.ValueMode.RandomTwoValues:
                startLifeTimeSpeed.z = startSpeed.value1;
                startLifeTimeSpeed.w = startSpeed.value2;
                break;

            case GPUParticleSystem.ValueMode.Curve:
                startLifeTimeSpeed.z = startLifeTimeSpeed.w = startSpeed.curve1.Evaluate(progress);
                break;

            case GPUParticleSystem.ValueMode.RandomTwoCurves:
                startLifeTimeSpeed.z = startSpeed.curve1.Evaluate(progress);
                startLifeTimeSpeed.w = startSpeed.curve2.Evaluate(progress);
                break;
        }

        return startLifeTimeSpeed;
    }

    public static Vector3 EvaluateVector3Bundle(Vector3CurveBundle bundle, float progress)
    {
        Vector3 vec = new Vector3();
        switch (bundle.mode)
        {
            case GPUParticleSystem.ValueMode.Value:
                vec = bundle.value1;
                break;
            case GPUParticleSystem.ValueMode.RandomTwoValues:
                vec = Vector3.Lerp(bundle.value1, bundle.value2, bundle.seed);
                break;
            case GPUParticleSystem.ValueMode.Curve:
                float c1 = bundle.curve1_1.Evaluate(progress);
                float c2 = bundle.curve1_1.Evaluate(progress);
                float c3 = bundle.curve1_1.Evaluate(progress);

                vec = new Vector3(c1, c2, c3);
                break;
            case GPUParticleSystem.ValueMode.RandomTwoCurves:
                float c4 = bundle.curve1_1.Evaluate(progress);
                float c5 = bundle.curve1_1.Evaluate(progress);
                float c6 = bundle.curve1_1.Evaluate(progress);
                Vector3 vec1 = new Vector3(c4, c5, c6);

                float c7 = bundle.curve1_1.Evaluate(progress);
                float c8 = bundle.curve1_1.Evaluate(progress);
                float c9 = bundle.curve1_1.Evaluate(progress);
                Vector3 vec2 = new Vector3(c7, c8, c9);

                vec = Vector3.Lerp(vec1, vec2, bundle.seed);
                break;
        }
        return vec;
    }

    public static Texture3D DeserializeVectorField(TextAsset fgaFile)
    {
        string FullFile = fgaFile.text;
        string[] AllFloats = FullFile.Split(',');
        float Length = (float)AllFloats.Length - 10;
        int LengthPerSide = Mathf.RoundToInt(Mathf.Pow(Length / 3f, 1f / 3f));

        Texture3D VectorField = new Texture3D(LengthPerSide, LengthPerSide, LengthPerSide, TextureFormat.RGBAFloat, false);
        VectorField.wrapMode = TextureWrapMode.Repeat;

        float[] ConvertedFloats = new float[(int)Length];

        for (int i = 0; i < ConvertedFloats.Length - 1; i++)
        {
            ConvertedFloats[i] = float.Parse(AllFloats[i + 9]);
        }

        Color[] col = new Color[Mathf.RoundToInt(Length / 3f)];

        for (int i = 0; i < col.Length - 1; i++)
        {
            Vector3 v = Vector3.Normalize(new Vector3(ConvertedFloats[i * 3], ConvertedFloats[i * 3 + 1], ConvertedFloats[i * 3 + 2]));
            col[i] = new Color(v.x, v.y, v.z, 1f);
        }

        VectorField.SetPixels(col);
        VectorField.Apply(false);
        return VectorField;
    }

	public static float Remap(float value, float From1, float To1, float From2, float To2)
	{
		return From2 + (value - From1) * (To2 - From2) / (To1 - From1);
	}
	#endregion
}
