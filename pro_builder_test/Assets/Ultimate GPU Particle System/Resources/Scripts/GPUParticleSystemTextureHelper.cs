using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPUParticleSystemTextureHelper
{
    /*
    public static Texture2D BakeGradient(Gradient _Gradient, int _Length)
    {
        Color[] col = new Color[_Length];

        for (int i = 0; i < _Length; i++)
        {
            col[i] = _Gradient.Evaluate((float)i / (float)_Length);
        }

        Texture2D tex = new Texture2D(_Length, 1, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.SetPixels(col);
        tex.Apply(false);

        return tex;
    }
    */

    public static Texture2D BakeColor(Color color, Texture2D particleColor)
    {
        int width = particleColor.width;
        int len = width * width;

        Color[] col = new Color[len];

        for (int i = 0; i < len; i++)
        {
            col[i] = color;
        }

        particleColor = new Texture2D(width, 1, TextureFormat.RGBA32, false);
        particleColor.wrapMode = TextureWrapMode.Clamp;
        particleColor.SetPixels(col);
        particleColor.Apply(false);
        return particleColor;
    }

    public static Texture2D BakeColor(Color colorA, Color colorB, Texture2D particleColor)
    {
        int width = particleColor.width;
        int len = width * width;

        Color[] col = new Color[len];

        int index = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                col[index] = Color.Lerp(colorA, colorB, (float)x / (float)width);
                index++;
            }
        }

        particleColor = new Texture2D(width, width, TextureFormat.RGBA32, false);
        particleColor.wrapMode = TextureWrapMode.Clamp;
        particleColor.SetPixels(col);
        particleColor.Apply(false);
        return particleColor;
    }

    public static Texture2D BakeColor(Gradient gradient, Texture2D particleColor)
    {
        int width = particleColor.width;
        int len = width * width;

        Color[] col = new Color[len];

        int index = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                col[index] = gradient.Evaluate((float)y / (float)width);
                index++;
            }
        }

        particleColor = new Texture2D(width, width, TextureFormat.RGBA32, false);
        particleColor.wrapMode = TextureWrapMode.Clamp;
        particleColor.SetPixels(col);
        particleColor.Apply(false);
        return particleColor;
    }

    public static Texture2D BakeColor(Gradient gradientA, Gradient gradientB, Texture2D particleColor)
    {
        int width = particleColor.width;
        int len = width * width;

        Color[] col = new Color[len];

        int index = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                col[index] = Color.Lerp(
                    gradientA.Evaluate((float)y / (float)width),
                    gradientB.Evaluate((float)y / (float)width),
                    (float)x / (float)width);
                index++;
            }
        }

        particleColor = new Texture2D(width, width, TextureFormat.RGBA32, false);
        particleColor.wrapMode = TextureWrapMode.Clamp;
        particleColor.SetPixels(col);
        particleColor.Apply(false);
        return particleColor;
    }
}
