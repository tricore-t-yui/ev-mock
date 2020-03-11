using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetCameraDepthNormalRenderMode : MonoBehaviour
{
    public DepthTextureMode renderMode = DepthTextureMode.Depth;
    public Camera depthCamera;

    private void OnEnable()
    {
        if (depthCamera != null)
        {
            depthCamera.depthTextureMode = renderMode;
        }
    }
}
