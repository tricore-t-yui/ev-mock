using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class SetUpGizmo : MonoBehaviour
{
	static SetUpGizmo()
	{
		if (!File.Exists(Application.dataPath + "/Gizmos/Icon_big.png"))
		{
			if (File.Exists(Application.dataPath + "/Ultimate GPU Particle System/Resources/Gizmos/Icon_big.png"))
			{
				File.Copy(Application.dataPath + "/Ultimate GPU Particle System/Resources/Gizmos/Icon_big.png", Application.dataPath + "/Gizmos/Icon_big.png");
				Debug.Log("Copied Gizmo texture to correct Gizmos folder (Right click > Refresh if not immediately visible).");
			}
		}
		/*
		if (!File.Exists(Application.dataPath + "/Editor Default Resources/Logo.png"))
		{
			if (File.Exists(Application.dataPath + "/Ultimate GPU Particle System/Editor Default Resources/Logo.png"))
			{
				File.Copy(Application.dataPath + "/Ultimate GPU Particle System/Editor Default Resources/Logo.png", Application.dataPath + "/Editor Default Resources/Logo.png");
				Debug.Log("Copied Logo texture to Editor Default Resources folder.");
			}
		}
		*/
	}
}