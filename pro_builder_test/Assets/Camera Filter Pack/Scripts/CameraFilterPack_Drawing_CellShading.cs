////////////////////////////////////////////
// CameraFilterPack - by VETASOFT 2018 /////
////////////////////////////////////////////
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu ("Camera Filter Pack/Drawing/CellShading")]
public class CameraFilterPack_Drawing_CellShading : MonoBehaviour {
#region Variables
public Shader SCShader;
private float TimeX = 1.0f;
 
private Material SCMaterial;
[Range(0, 1)]
public float EdgeSize = 0.1f;
[Range(0, 10)]
public float ColorLevel = 4f;
#endregion
#region Properties
Material material
{
get
{
if(SCMaterial == null)
{
SCMaterial = new Material(SCShader);
SCMaterial.hideFlags = HideFlags.HideAndDontSave;	
}
return SCMaterial;
}
}
#endregion
void Start () 
{
SCShader = Shader.Find("CameraFilterPack/Drawing_CellShading");
if(!SystemInfo.supportsImageEffects)
{
enabled = false;
return;
}
}
void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
{
if(SCShader != null)
{
TimeX+=Time.deltaTime;
if (TimeX>100)  TimeX=0;
material.SetFloat("_TimeX", TimeX);
material.SetFloat("_EdgeSize", EdgeSize);
material.SetFloat("_ColorLevel", ColorLevel);
material.SetVector("_ScreenResolution",new Vector2(Screen.width,Screen.height));
Graphics.Blit(sourceTexture, destTexture, material);
}
else
{
Graphics.Blit(sourceTexture, destTexture);	
}
}
void Update () 
{
#if UNITY_EDITOR
if (Application.isPlaying!=true)
{
SCShader = Shader.Find("CameraFilterPack/Drawing_CellShading");
}
#endif
}
void OnDisable ()
{
if(SCMaterial)
{
DestroyImmediate(SCMaterial);	
}
}
}