using UnityEngine;

[System.Serializable]
public class IntCurveBundle
{
    public GPUParticleSystem.ValueMode mode = GPUParticleSystem.ValueMode.Value;
    public int value1 = 0;
    public int value2 = 0;
    public AnimationCurve curve1 = AnimationCurve.Linear(0f, 1f, 0f, 1f);
    public AnimationCurve curve2 = AnimationCurve.Linear(0f, 1f, 0f, 1f);
    public float seed = 0f;
}
