using UnityEngine;
using UnityEditor;
using System.Collections;

public partial class GPUParticleSystemEditor
{
	//Buffer 
	private Vector3[] points;

	private void OnSceneGUI()
    {
		/*
		if (Camera.current != null)
		{
			LayerMask inverted = ~(1 << layerMask.intValue);
			Debug.Log(layerMask.intValue+" "+inverted);

			//Camera.current.cullingMask = inverted;
			SceneView.lastActiveSceneView.camera.cullingMask = inverted;
		}
		*/

        if (particleSystem == null)
            return;

        DrawWindows();

		if (ShowWSBounds.boolValue)
		{
			Bounds b = particleSystem.GetWSBounds();
			DrawWorldSpaceBounds(b.center, b.extents);
		}

		if (EditBounds.boolValue)
		{
			DrawAxisAlignedBounds(ref particleSystem.center, ref particleSystem.extents, particleSystem.transform);
		}
		else
		{
			switch (particleSystem.emitterShape)
			{
				case GPUParticleSystem.EmitterShape.Edge:
					DrawEdge();
					break;
				case GPUParticleSystem.EmitterShape.Circle:
					DrawCircle();
					break;
				case GPUParticleSystem.EmitterShape.Sphere:
					DrawSphere();
					break;
				case GPUParticleSystem.EmitterShape.Box:
					DrawBox();
					break;
				case GPUParticleSystem.EmitterShape.Cone:
					DrawConeEmitter();
					break;
				case GPUParticleSystem.EmitterShape.HemiSphere:
					DrawHemiSphere();
					break;
				case GPUParticleSystem.EmitterShape.Texture:
					DrawTextureEmitter();
					break;
				default:
					DrawCross();
					break;
			}
		}

		SceneView.RepaintAll();
    }

    private const int ControlWindowId = 1 << 0;
    private const float ControlWindowWidth = 220;
    private const float ControlWindowHeight = 120;
    private Rect ControlWindowRect;

    private void DrawWindows()
    {
        if (ControlWindowRect.width <= 0)
        {
            ControlWindowRect = new Rect(Camera.current.pixelWidth - ControlWindowWidth, (Camera.current.pixelHeight - ControlWindowHeight) / 2, ControlWindowWidth, ControlWindowHeight);
        }
        DrawControlWindow();
    }

    private void DrawControlWindow()
    {
        ControlWindowRect.x = Camera.current.pixelWidth - ControlWindowRect.width - 10f;
        ControlWindowRect.y = Camera.current.pixelHeight - Mathf.FloorToInt(ControlWindowRect.height - 5f);
        ControlWindowRect = GUI.Window(ControlWindowId, ControlWindowRect, DoControlWindow, "GPU Particle System");
    }

    private void DoControlWindow(int windowID)
    {
        //ShowControlWindow = EditorGUI.Foldout(new Rect(0, 0, 10, EditorGUIUtility.singleLineHeight), ShowControlWindow, new GUIContent("Layers"));
        EditorGUI.ProgressBar(new Rect(2f, EditorGUIUtility.singleLineHeight, ControlWindowWidth - 2f, EditorGUIUtility.singleLineHeight), particleSystem.progress, "Time " + (particleSystem.progress * 100f).ToString("f1") + "%");

        if (particleSystem.state == GPUParticleSystem.GPUParticleSystemState.Playing)
        {
            if (GUI.Button(new Rect(2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Pause"), EditorStyles.miniButtonLeft))
            {
                particleSystem.Pause();
            }
        }
        else
        {
            if (GUI.Button(new Rect(2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Play"), EditorStyles.miniButtonLeft))
            {
                particleSystem.Play();
            }
        }

        if (GUI.Button(new Rect(ControlWindowWidth / 3f - 2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Restart"), EditorStyles.miniButtonMid))
            particleSystem.Restart();

        if (GUI.Button(new Rect(ControlWindowWidth / 1.5f - 2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Stop"), EditorStyles.miniButtonRight))
            particleSystem.Stop();

        if (GUI.Button(new Rect(2f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("5"), EditorStyles.miniButtonLeft))
            particleSystem.Emit(5);
        
        if (GUI.Button(new Rect(ControlWindowWidth / 5f * 1f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("50"), EditorStyles.miniButtonMid))
            particleSystem.Emit(50);
        
        if (GUI.Button(new Rect(ControlWindowWidth / 5f * 2f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("500"), EditorStyles.miniButtonMid))
            particleSystem.Emit(500);

		if (GUI.Button(new Rect(ControlWindowWidth / 5f * 3f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("5k"), EditorStyles.miniButtonMid))
			particleSystem.Emit(5000);

		if (GUI.Button(new Rect(ControlWindowWidth / 5f * 4f - 2f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("50k"), EditorStyles.miniButtonRight))
            particleSystem.Emit(50000);
            
        GUI.DragWindow();
    }

	public void DrawAxisAlignedBounds(ref Vector3 center, ref Vector3 extents, Transform transformCenter)
	{
		Handles.color = Color.red;

		Vector3 pos = Vector3.zero;

		pos = transformCenter.position + center + new Vector3(extents.x, 0f, 0f);
		pos = Handles.Slider(pos, Vector3.right, 1.55f, Handles.ArrowHandleCap, 0f) - transformCenter.position  - center;
		extents.x = pos.x;
		
		pos = (transformCenter.position + center) + new Vector3(-extents.x, 0f, 0f);
		pos = (transformCenter.position + center) - Handles.Slider(pos, Vector3.left, 1.55f, Handles.ArrowHandleCap, 0f);
		extents.x = pos.x;
		
		Handles.color = Color.green;

		pos = transformCenter.position + center + new Vector3(0f, extents.y, 0f);
		pos = Handles.Slider(pos, Vector3.up, 1.55f, Handles.ArrowHandleCap, 0f) - center - transformCenter.position;
		extents.y = pos.y;
		
		pos = (transformCenter.position + center) + new Vector3(0f, -extents.y, 0f);
		pos = (transformCenter.position + center) - Handles.Slider(pos, Vector3.down, 1.55f, Handles.ArrowHandleCap, 0f);
		extents.y = pos.y;
		
		Handles.color = Color.blue;

		pos = transformCenter.position + center + new Vector3(0f, 0f, extents.z);
		pos = Handles.Slider(pos, Vector3.forward, 1.55f, Handles.ArrowHandleCap, 0f) - center - transformCenter.position;
		extents.z = pos.z;
		
		pos = (transformCenter.position + center) + new Vector3(0f, 0f, -extents.z);
		pos = (transformCenter.position + center) +- Handles.Slider(pos, Vector3.back, 1.55f, Handles.ArrowHandleCap, 0f);
		extents.z = pos.z;
		
		Handles.color = new Color(0.576f, 0.858f, 0.949f, 1f);
		points = new Vector3[10];

		points[0] = (transformCenter.position + center) + new Vector3(-extents.x, extents.y, -extents.z);
		points[1] = (transformCenter.position + center) + new Vector3(extents.x, extents.y, -extents.z);
		points[2] = (transformCenter.position + center) + new Vector3(extents.x, extents.y, extents.z);
		points[3] = (transformCenter.position + center) + new Vector3(-extents.x, extents.y, extents.z);
		points[4] = (transformCenter.position + center) + new Vector3(-extents.x, extents.y, -extents.z);

		points[5] = (transformCenter.position + center) + new Vector3(-extents.x, -extents.y, -extents.z);
		points[6] = (transformCenter.position + center) + new Vector3(extents.x, -extents.y, -extents.z);
		points[7] = (transformCenter.position + center) + new Vector3(extents.x, -extents.y, extents.z);
		points[8] = (transformCenter.position + center) + new Vector3(-extents.x, -extents.y, extents.z);
		points[9] = (transformCenter.position + center) + new Vector3(-extents.x, -extents.y, -extents.z);

		Handles.DrawAAPolyLine(points);
		points = new Vector3[2];

		points[0] = (transformCenter.position + center) + new Vector3(extents.x, -extents.y, -extents.z);
		points[1] = (transformCenter.position + center) + new Vector3(extents.x, extents.y, -extents.z);

		Handles.DrawAAPolyLine(points);
		points = new Vector3[2];

		points[0] = (transformCenter.position + center) + new Vector3(extents.x, -extents.y, extents.z);
		points[1] = (transformCenter.position + center) + new Vector3(extents.x, extents.y, extents.z);

		Handles.DrawAAPolyLine(points);
		points = new Vector3[2];

		points[0] = (transformCenter.position + center) + new Vector3(-extents.x, -extents.y, extents.z);
		points[1] = (transformCenter.position + center) + new Vector3(-extents.x, extents.y, extents.z);

		Handles.DrawAAPolyLine(2f, points);
		Handles.color = Color.white;
	}

	public void DrawWorldSpaceBounds(Vector3 center, Vector3 extents)
	{
		Handles.color = new Color(0.576f, 0.858f, 0.949f, 1f);
		points = new Vector3[10];

		points[0] = center + new Vector3(-extents.x, extents.y, -extents.z);
		points[1] = center + new Vector3(extents.x, extents.y, -extents.z);
		points[2] = center + new Vector3(extents.x, extents.y, extents.z);
		points[3] = center + new Vector3(-extents.x, extents.y, extents.z);
		points[4] = center + new Vector3(-extents.x, extents.y, -extents.z);

		points[5] = center + new Vector3(-extents.x, -extents.y, -extents.z);
		points[6] = center + new Vector3(extents.x, -extents.y, -extents.z);
		points[7] = center + new Vector3(extents.x, -extents.y, extents.z);
		points[8] = center + new Vector3(-extents.x, -extents.y, extents.z);
		points[9] = center + new Vector3(-extents.x, -extents.y, -extents.z);

		Handles.DrawAAPolyLine(points);
		points = new Vector3[2];

		points[0] = center + new Vector3(extents.x, -extents.y, -extents.z);
		points[1] = center + new Vector3(extents.x, extents.y, -extents.z);

		Handles.DrawAAPolyLine(points);
		points = new Vector3[2];

		points[0] = center + new Vector3(extents.x, -extents.y, extents.z);
		points[1] = center + new Vector3(extents.x, extents.y, extents.z);

		Handles.DrawAAPolyLine(points);
		points = new Vector3[2];

		points[0] = center + new Vector3(-extents.x, -extents.y, extents.z);
		points[1] = center + new Vector3(-extents.x, extents.y, extents.z);

		Handles.DrawAAPolyLine(2f, points);
		Handles.color = Color.white;
	}
}
