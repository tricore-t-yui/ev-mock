using UnityEngine;

[System.Serializable]
public class Vector3CurveBundle
{
#if UNITY_EDITOR
    public bool showEditor = false;
#endif

    public GPUParticleSystem.ValueMode mode = GPUParticleSystem.ValueMode.Value;
    public Vector3 value1 = Vector3.zero;
    public Vector3 value2 = Vector3.zero;
    public AnimationCurve curve1_1 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public AnimationCurve curve1_2 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public AnimationCurve curve1_3 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public AnimationCurve curve2_1 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public AnimationCurve curve2_2 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public AnimationCurve curve2_3 = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    public Vector2 minMax = new Vector2(0f,1f);
    public float seed = 0f;
}
