using UnityEngine;

[System.Serializable]
public class FloatCurveBundle
{
#if UNITY_EDITOR
    public bool showEditor = false;
#endif

    public GPUParticleSystem.ValueMode mode = GPUParticleSystem.ValueMode.Value;
    public float value1 = 1f;
    public float value2 = 0f;
    public AnimationCurve curve1 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public AnimationCurve curve2 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public Vector2 minMax = new Vector2(0f,1f);
    public float seed = 0f;

	public FloatCurveBundle(float value1, float value2)
	{
		this.value1 = value1;
		this.value2 = value2;
	}
}
