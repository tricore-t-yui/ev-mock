using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[InitializeOnLoad]
public class GPUParticleSystemHierarchyLabel
{
	static GUIStyle style;
	const float margin = 2;
	private static Texture2D uGSIcon;

	static GPUParticleSystemHierarchyLabel()
	{
		EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyLabel;
	}

	private const string propertyName = "EnableHierarchySubtitle";

	[MenuItem("Edit/Toggle Hierarchy Subtitle")]
	static void DoSomething()
	{
		Enabled = !Enabled;
	}

	static bool Enabled
	{
		get
		{
			return !EditorPrefs.HasKey(propertyName) || EditorPrefs.GetBool(propertyName);
		}
		set
		{
			EditorPrefs.SetBool(propertyName, value);
		}
	}

	static void DrawHierarchyLabel(int instanceID, Rect selectionRect)
	{
		if (!Enabled)
			return;

		if (style == null)
			GenerateStyles();

		if (uGSIcon == null)
		{
			if (EditorGUIUtility.isProSkin)
			{
				uGSIcon = Resources.Load<Texture2D>("Gizmos/Icon_small");
			}
			else
			{
				uGSIcon = Resources.Load<Texture2D>("Gizmos/Icon_small_dark");
			}
		}

		GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

		if (!gameObject)
			return;

		selectionRect.x += 18f + GUI.skin.label.CalcSize( new GUIContent(gameObject.name) ).x;
		selectionRect.y += margin;

		foreach (var label in CollectLabels(gameObject))
		{
			selectionRect.width = 22f;
			selectionRect.y -= 2f;
			selectionRect.height -= 1f;

			Rect imageRect = new Rect(0f, selectionRect.y, 16f, 16f);

			GPUParticleSystem gpu = gameObject.GetComponent<GPUParticleSystem>();

			if (gpu != null)
			{
				GUI.DrawTexture(imageRect, uGSIcon);

				if (GUI.Button(selectionRect, '\u25B6'.ToString()))
				{
					gpu.Play();
				}
				selectionRect.x += selectionRect.width;

				if (GUI.Button(selectionRect, "\u2225".ToString()))
				{
					gpu.Pause();
				}
				selectionRect.x += selectionRect.width;

				if (GUI.Button(selectionRect, '\u303C'.ToString()))
				{
					gpu.Stop();
				}
			}
		}
	}

	static IEnumerable<string> CollectLabels(GameObject gameObject)
	{
		yield return gameObject.transform.position.ToString("f1");
	}

	static void GenerateStyles()
	{
		style = new GUIStyle(GUI.skin.GetStyle("WhiteLabel"));
	}
}
