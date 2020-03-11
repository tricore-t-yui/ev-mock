using UnityEngine;

[System.Serializable]
public class ShaderCurveBundle
{
#if UNITY_EDITOR
    public bool showEditor = false;
#endif
    public GPUParticleSystem.CurveMode mode = GPUParticleSystem.CurveMode.Off;
    public float multiplier = 1f;
    public float skew = 1f;
    public bool invert;
    public AnimationCurve curve1 = AnimationCurve.Linear(0f, 1f, 0f, 1f);
    public AnimationCurve curve2 = AnimationCurve.Linear(0f, 1f, 0f, 1f);
    public Vector2 minMax = new Vector2(0f, 1f);
    public float seed = 0f;
    public Vector4[] bezier1;
    public Vector4[] bezier2;
    private Material preview { get; set; }

    public void Apply()
    {
        if (preview != null)
        {
            if (mode == GPUParticleSystem.CurveMode.Linear)
            {
                preview.EnableKeyword("LINEAR");
            }
            else
            {
                preview.DisableKeyword("LINEAR");
            }

            preview.SetFloat("_Skew", skew);
            preview.SetFloat("_Multiplier", multiplier);

            if (invert)
            {
                preview.SetFloat("_Invert", 1);
            }
            else
            {
                preview.SetFloat("_Invert", 0);
            }
        }
    }

    public Material curvePreview
    {
        get
        {
            if (preview == null)
            {
                preview = new Material(Shader.Find("GPUParticles/Debug/DrawCurve"));
            }

            return this.preview;
        }
        set
        {
            this.preview = value;
        }
    }
}
