using UnityEngine;

[System.Serializable]
public class SingleFloatCurveBundle
{
#if UNITY_EDITOR
    public bool showEditor = false;
#endif

    public GPUParticleSystem.SimpleValueMode mode = GPUParticleSystem.SimpleValueMode.Value;
    public float value = 1f;
    public AnimationCurve curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public Vector2 minMax = new Vector2(0f,1f);
    public float seed = 0f;

	public SingleFloatCurveBundle(float value)
	{
		this.value = value;
	}
}
