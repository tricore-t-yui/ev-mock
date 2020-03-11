using UnityEngine;

[System.Serializable]
public class ColorGradientBundle
{
#if UNITY_EDITOR
    public bool showEditor = false;
#endif
    public GPUParticleSystem.ColorMode mode = GPUParticleSystem.ColorMode.Gradient;
    public float intensity = 1f;
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Gradient gradient1 = new Gradient();
    public Gradient gradient2 = new Gradient();
    public float seed = 0f;
}
