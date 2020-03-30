using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSupportTest : MonoBehaviour
{
	public Shader[] shaders;

	private void OnGUI()
	{
		GUILayout.BeginVertical();
		{
			for (int i = 0; i < shaders.Length;i++)
			{
				GUILayout.Label(shaders[i].name+" "+shaders[i].isSupported);
			}
		}
		GUILayout.EndVertical();
	}
}
