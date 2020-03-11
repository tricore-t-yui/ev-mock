using UnityEngine;
using UnityEditor;
using System.Collections;

public partial class GPUParticleSystemEditor
{
	private void DrawSingleFloatCurveToggleBundel(SingleFloatCurveBundle bundle, string propertyName, string name)
    {
        int tbint = (int)bundle.mode;

        SerializedProperty b = serializedObject.FindProperty(propertyName).FindPropertyRelative("showEditor");

        EditorGUILayout.BeginHorizontal();
        {
            if (b.boolValue)
            {
                GUI.color = new Color(0.5f, 1f, 0.5f, 1f);

				if (GUILayout.Button("(x)", EditorStyles.toolbarButton, GUILayout.MaxWidth(25f)))
                {
                    b.boolValue = !b.boolValue;
                }
                GUI.color = Color.white;

                GUILayout.Button(name, EditorStyles.toolbarButton);

                string[] toolbarStrings = new string[] { "V", "C" };
                tbint = GUILayout.Toolbar(tbint, toolbarStrings, EditorStyles.toolbarButton, GUILayout.MaxWidth(100f));
            }
            else
            {
				GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
				if (GUILayout.Button("(+)", EditorStyles.toolbarButton, GUILayout.MaxWidth(25f)))
                {
                    b.boolValue = !b.boolValue;
                }
				GUI.color = Color.white;
				GUILayout.Button(name, EditorStyles.toolbarButton);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (b.boolValue)
        {
            EditorGUILayout.BeginVertical();
            {
                SerializedProperty mm = serializedObject.FindProperty(propertyName).FindPropertyRelative("minMax");
                SerializedProperty v1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("value");
                SerializedProperty c1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve");

                switch (tbint)
                {
                    case 0:
                        EditorGUILayout.PropertyField(v1, new GUIContent("Value", "Color 1"));
                        break;

                    case 1:
                        EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
                        EditorGUILayout.PropertyField(c1, new GUIContent("Curve", "Curve 1"));
                        break;
                }

                bundle.mode = (GPUParticleSystem.SimpleValueMode)tbint;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }
    }

    private void DrawSingleFloatCurveBundel(SingleFloatCurveBundle bundle, string propertyName, string name)
    {
        int tbint = (int)bundle.mode;

        SerializedProperty b = serializedObject.FindProperty(propertyName).FindPropertyRelative("showEditor");

		EditorGUILayout.BeginHorizontal();
		{
			if (b.boolValue)
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				bundle.mode = (GPUParticleSystem.SimpleValueMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
			}
			else
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				EditorGUI.BeginDisabledGroup(true);
				{
					bundle.mode = (GPUParticleSystem.SimpleValueMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
				}
				EditorGUI.EndDisabledGroup();
			}
		}
		EditorGUILayout.EndHorizontal();

        if (b.boolValue)
        {
            EditorGUILayout.BeginVertical();
            {
                SerializedProperty mm = serializedObject.FindProperty(propertyName).FindPropertyRelative("minMax");
                SerializedProperty v1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("value");
                SerializedProperty c1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve");

                switch (tbint)
                {
                    case 0:
                        EditorGUILayout.PropertyField(v1, new GUIContent("Value", "Color 1"));
                        break;

                    case 1:
                        EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
                        EditorGUILayout.PropertyField(c1, new GUIContent("Curve", "Curve 1"));
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }
    }
    
    private void DrawFloatCurveBundel(FloatCurveBundle bundle, string propertyName, string name)
    {
        SerializedProperty b = serializedObject.FindProperty(propertyName).FindPropertyRelative("showEditor");
		EditorGUILayout.BeginHorizontal();
		{
			if (b.boolValue)
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				bundle.mode = (GPUParticleSystem.ValueMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
			}
			else
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				EditorGUI.BeginDisabledGroup(true);
				{
					bundle.mode = (GPUParticleSystem.ValueMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
				}
				EditorGUI.EndDisabledGroup();
			}
		}
		EditorGUILayout.EndHorizontal();

		if (b.boolValue)
		{
			GUILayout.Space(-4f);
			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUILayout.BeginVertical();
				{
					SerializedProperty mm = serializedObject.FindProperty(propertyName).FindPropertyRelative("minMax");
					SerializedProperty v1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("value1");
					SerializedProperty v2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("value2");
					SerializedProperty c1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve1");
					SerializedProperty c2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve2");

					switch (bundle.mode)
					{
						case GPUParticleSystem.ValueMode.Value:
							EditorGUILayout.PropertyField(v1, new GUIContent("Value", "Color 1"));
							break;

						case GPUParticleSystem.ValueMode.RandomTwoValues:
							EditorGUILayout.PropertyField(v2, new GUIContent("Max", ""));
							EditorGUILayout.PropertyField(v1, new GUIContent("Min", ""));
							break;

						case GPUParticleSystem.ValueMode.Curve:
							EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
							EditorGUILayout.PropertyField(c1, new GUIContent("Curve", "Curve 1"));
							break;

						case GPUParticleSystem.ValueMode.RandomTwoCurves:
							EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
							EditorGUILayout.PropertyField(c2, new GUIContent("Max", "Curve Range min/max"));
							EditorGUILayout.PropertyField(c1, new GUIContent("Min", "Curve Range min/max"));
							break;
					}

					//bundle.mode = (GPUParticleSystem.ValueMode)tbint;
				}
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(5f);
			}
			EditorGUILayout.EndVertical();
		}
	}

    private void DrawVector3CurveBundel(Vector3CurveBundle bundle, string propertyName, string name)
    {
        int tbint = (int)bundle.mode;

        SerializedProperty b = serializedObject.FindProperty(propertyName).FindPropertyRelative("showEditor");

		EditorGUILayout.BeginHorizontal();
		{
			if (b.boolValue)
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				bundle.mode = (GPUParticleSystem.ValueMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
			}
			else
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				EditorGUI.BeginDisabledGroup(true);
				{
					bundle.mode = (GPUParticleSystem.ValueMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
				}
				EditorGUI.EndDisabledGroup();
			}
		}
		EditorGUILayout.EndHorizontal();

		if (b.boolValue)
        {
            EditorGUILayout.BeginVertical();
            {
                SerializedProperty mm = serializedObject.FindProperty(propertyName).FindPropertyRelative("minMax");
                SerializedProperty v1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("value1");
                SerializedProperty v2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("value2");
                SerializedProperty c1_1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve1_1");
                SerializedProperty c1_2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve1_2");
                SerializedProperty c1_3 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve1_3");
                SerializedProperty c2_1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve2_1");
                SerializedProperty c2_2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve2_2");
                SerializedProperty c2_3 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve2_3");

                switch (tbint)
                {
                    case 0:
                        EditorGUILayout.PropertyField(v1, new GUIContent("Value", ""));
                        break;

                    case 1:
                        EditorGUILayout.PropertyField(v2, new GUIContent("Max", ""));
                        EditorGUILayout.PropertyField(v1, new GUIContent("Min", ""));
                        break;

                    case 2:
                        EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel(name);
                            EditorGUILayout.PropertyField(c1_1, new GUIContent("", ""), false);
                            EditorGUILayout.PropertyField(c1_2, new GUIContent("", ""), false);
                            EditorGUILayout.PropertyField(c1_3, new GUIContent("", ""), false);
                        }
                        EditorGUILayout.EndHorizontal();
                        break;

                    case 3:
                        EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel(name + " max");
                            EditorGUILayout.PropertyField(c1_1, new GUIContent("", ""), false);
                            EditorGUILayout.PropertyField(c1_2, new GUIContent("", ""), false);
                            EditorGUILayout.PropertyField(c1_3, new GUIContent("", ""), false);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel(name + " min");
                            EditorGUILayout.PropertyField(c2_1, new GUIContent("", ""), false);
                            EditorGUILayout.PropertyField(c2_2, new GUIContent("", ""), false);
                            EditorGUILayout.PropertyField(c2_3, new GUIContent("", ""), false);
                        }
                        EditorGUILayout.EndHorizontal();
                        break;
                }

            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }
    }

    private void DrawColorGradientBundel(ColorGradientBundle bundle, string propertyName, string name)
    {
        int tbint = (int)bundle.mode;

        SerializedProperty b = serializedObject.FindProperty(propertyName).FindPropertyRelative("showEditor");

		EditorGUILayout.BeginHorizontal();
		{
			if (b.boolValue)
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				bundle.mode = (GPUParticleSystem.ColorMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
			}
			else
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				EditorGUI.BeginDisabledGroup(true);
				{
					bundle.mode = (GPUParticleSystem.ColorMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
				}
				EditorGUI.EndDisabledGroup();
			}
		}
		EditorGUILayout.EndHorizontal();

        SerializedProperty p1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("color1");
        SerializedProperty p2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("color2");
        SerializedProperty g1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("gradient1");
        SerializedProperty g2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("gradient2");

        if (b.boolValue)
        {
            EditorGUILayout.BeginVertical();
            {

                switch (tbint)
                {
                    case 0:
                        EditorGUILayout.PropertyField(p1, new GUIContent("Color 1", "Color 1"));
                        break;

                    case 1:
                        EditorGUILayout.PropertyField(p2, new GUIContent("Color 2", "Color 1"));
                        EditorGUILayout.PropertyField(p1, new GUIContent("Color 1", "Color 2"));
                        break;

                    case 2:
                        EditorGUILayout.PropertyField(g1, new GUIContent("Gradient 1", "Gradient 1"));
                        break;

                    case 3:
                        EditorGUILayout.PropertyField(g2, new GUIContent("Gradient 1", "Gradient 1"));
                        EditorGUILayout.PropertyField(g1, new GUIContent("Gradient 2", "Gradient 2"));
                        break;
                }

            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawShaderCurveBundel(ShaderCurveBundle bundle, string propertyName, string name)
    {
        EditorGUI.BeginChangeCheck();
        SerializedProperty mode = serializedObject.FindProperty(propertyName).FindPropertyRelative("mode");
        SerializedProperty b = serializedObject.FindProperty(propertyName).FindPropertyRelative("showEditor");

        EditorGUILayout.BeginHorizontal();
        {
			if (b.boolValue)
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				bundle.mode = (GPUParticleSystem.CurveMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
			}
			else
			{
				if (GUILayout.Button(name, EditorStyles.toolbarButton))
					b.boolValue = !b.boolValue;

				EditorGUI.BeginDisabledGroup(true);
				{
					bundle.mode = (GPUParticleSystem.CurveMode)EditorGUILayout.EnumPopup(bundle.mode, EditorStyles.toolbarDropDown, GUILayout.MaxWidth(Screen.width / 4f));
				}
				EditorGUI.EndDisabledGroup();
			}
        }
        EditorGUILayout.EndHorizontal();

        if (b.boolValue)
        {
            EditorGUILayout.BeginVertical();
            {
                SerializedProperty mm = serializedObject.FindProperty(propertyName).FindPropertyRelative("minMax");
                SerializedProperty m = serializedObject.FindProperty(propertyName).FindPropertyRelative("multiplier");
                SerializedProperty s = serializedObject.FindProperty(propertyName).FindPropertyRelative("skew");
                SerializedProperty i = serializedObject.FindProperty(propertyName).FindPropertyRelative("invert");
                SerializedProperty c1 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve1");
                SerializedProperty c2 = serializedObject.FindProperty(propertyName).FindPropertyRelative("curve2");

                switch (mode.enumValueIndex)
                {
                    case 0:
                        EditorGUILayout.LabelField(name+" is deactivated.");
                        break;
                        
                    case 1:
                        m.floatValue = EditorGUILayout.FloatField(new GUIContent("Multiplier", "Y-axis"), m.floatValue);
                        s.floatValue = EditorGUILayout.Slider(new GUIContent("Skew", "Shape style setting"), s.floatValue, 0.2f, 16f);
                        i.boolValue = EditorGUILayout.Toggle(new GUIContent("Invert", "Invert the curve"), i.boolValue);
                        Rect rect1 = GUILayoutUtility.GetRect(250f, 250f);
                        EditorGUI.DrawPreviewTexture(rect1, Texture2D.whiteTexture, bundle.curvePreview, ScaleMode.ScaleToFit);
                        break;

                    case 2:
                        m.floatValue = EditorGUILayout.FloatField(new GUIContent("Multiplier", "Y-axis"), m.floatValue);
                        s.floatValue = EditorGUILayout.Slider(new GUIContent("Skew", "Shape style setting"), s.floatValue, 0.2f, 16f);
                        s.floatValue = Mathf.Clamp(s.floatValue, 0.2f, 16f);
                        i.boolValue = EditorGUILayout.Toggle(new GUIContent("Invert", "Invert the curve"), i.boolValue);
                        Rect rect2 = GUILayoutUtility.GetRect(250f, 250f);
                        EditorGUI.DrawPreviewTexture(rect2, Texture2D.whiteTexture, bundle.curvePreview, ScaleMode.ScaleToFit);
                        break;

                    case 3:
                        EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
                        EditorGUILayout.PropertyField(c1, new GUIContent("Curve", "Curve 1"));
                        break;

                    case 4:
                        EditorGUILayout.PropertyField(mm, new GUIContent("Min/Max", "Curve Range min/max"));
                        EditorGUILayout.PropertyField(c2, new GUIContent("Max", "Curve Range min/max"));
                        EditorGUILayout.PropertyField(c1, new GUIContent("Min", "Curve Range min/max"));
                        break;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    bundle.Apply();
                }         
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }

    }

    private void DrawEmitterOptions(GPUParticleSystem.EmitterShape shape)
    {
        SerializedProperty res = serializedObject.FindProperty("meshEmitterResolution");
        SerializedProperty mode = serializedObject.FindProperty("meshEmitterMode");

        switch (shape)
        {
            case GPUParticleSystem.EmitterShape.Point: 
                break;

            case GPUParticleSystem.EmitterShape.Edge:
                param1.floatValue = EditorGUILayout.FloatField("Length", param1.floatValue);
                break;

            case GPUParticleSystem.EmitterShape.Circle:
                param1.floatValue = EditorGUILayout.FloatField("Radius", param1.floatValue);

                EditorGUI.BeginChangeCheck();
                emitFromShell.boolValue = DrawOnOffToggle(emitFromShell.boolValue, "Emit from edge");
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.SetEmitFromShell(emitFromShell.boolValue);
                }
                break;

            case GPUParticleSystem.EmitterShape.Box:
                param1.floatValue = EditorGUILayout.FloatField("Width", param1.floatValue);
                param2.floatValue = EditorGUILayout.FloatField("Height", param2.floatValue);
                param3.floatValue = EditorGUILayout.FloatField("Depth", param3.floatValue);
                EditorGUI.BeginChangeCheck();
                emitFromShell.boolValue = DrawOnOffToggle(emitFromShell.boolValue, "Emit from edge");
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.SetEmitFromShell(emitFromShell.boolValue);
                }
                break;

            case GPUParticleSystem.EmitterShape.HemiSphere:
                param1.floatValue = EditorGUILayout.FloatField("Radius", param1.floatValue);
                EditorGUI.BeginChangeCheck();
                emitFromShell.boolValue = DrawOnOffToggle(emitFromShell.boolValue, "Emit from edge");
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.SetEmitFromShell(emitFromShell.boolValue);
                }
                break;

            case GPUParticleSystem.EmitterShape.Sphere:
                param1.floatValue = EditorGUILayout.FloatField("Radius", param1.floatValue);
                EditorGUI.BeginChangeCheck();
                emitFromShell.boolValue = DrawOnOffToggle(emitFromShell.boolValue, "Emit from edge");
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.SetEmitFromShell(emitFromShell.boolValue);
                }
                break;

            case GPUParticleSystem.EmitterShape.Cone:
                param1.floatValue = EditorGUILayout.FloatField("Radius 1", param1.floatValue);
                param2.floatValue = EditorGUILayout.FloatField("Angle", param2.floatValue);
                param3.floatValue = EditorGUILayout.FloatField("Length", param3.floatValue);
                //param4.floatValue = EditorGUILayout.FloatField("Radius 2", param4.floatValue);

                EditorGUI.BeginChangeCheck();
                emitFromShell.boolValue = DrawOnOffToggle(emitFromShell.boolValue, "Emit from edge");
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.SetEmitFromShell(emitFromShell.boolValue);
                }

                EditorGUI.BeginChangeCheck();
                emitFromBase.boolValue = DrawOnOffToggle(emitFromBase.boolValue, "Emit from base");
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.SetEmitFromBase(emitFromBase.boolValue);
                }
                break;

			case GPUParticleSystem.EmitterShape.Texture:
				EditorGUI.BeginChangeCheck();
				param1.floatValue = EditorGUILayout.FloatField("Width", param1.floatValue);
				param2.floatValue = EditorGUILayout.FloatField("Height", param2.floatValue);

				if (EditorGUI.EndChangeCheck())
				{
					particleSystem.UpdateStartSize();
				}
				break;

			case GPUParticleSystem.EmitterShape.Mesh:
				SerializedProperty mesh = serializedObject.FindProperty("meshEmitter");

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel(new GUIContent("Mesh", "Choose a mesh that will be used as an Emitter."));

					EditorGUI.BeginChangeCheck();
					mesh.objectReferenceValue = EditorGUILayout.ObjectField(mesh.objectReferenceValue, typeof(Mesh), false);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateEmitterTexture();
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel(new GUIContent("Generate from", "Chose from which data the mesh emitter will be created from."));

					EditorGUI.BeginChangeCheck();
					particleSystem.meshBakeType = (GPUParticleSystem.MeshBakeType)EditorGUILayout.EnumPopup(particleSystem.meshBakeType);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateEmitterTexture();
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Precision: " + (res.intValue * res.intValue).ToString());

					EditorGUI.BeginChangeCheck();
					res.intValue = EditorGUILayout.IntSlider(res.intValue, 8, 512);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateEmitterTexture();
					}
				}
				EditorGUILayout.EndHorizontal();

				break;

			case GPUParticleSystem.EmitterShape.MeshFilter:

				SerializedProperty meshFilter = serializedObject.FindProperty("meshFilterEmitter");

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel(new GUIContent("Mesh Filter", "Choose a mesh filter component that will be used as an Emitter."));

					EditorGUI.BeginChangeCheck();
					meshFilter.objectReferenceValue = EditorGUILayout.ObjectField(meshFilter.objectReferenceValue, typeof(MeshFilter), true);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateEmitterTexture();
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel(new GUIContent("Resolution", "The buffer size. Increase if mesh is big or many particles are needed."));

					EditorGUI.BeginChangeCheck();
					particleSystem.meshBakeType = (GPUParticleSystem.MeshBakeType)EditorGUILayout.EnumPopup(particleSystem.meshBakeType);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateEmitterTexture();
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Precision " + (res.intValue * res.intValue).ToString());

					EditorGUI.BeginChangeCheck();
					res.intValue = EditorGUILayout.IntSlider(res.intValue, 16, 512);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateEmitterTexture();
					}
				}
				EditorGUILayout.EndHorizontal();
				break;

			case GPUParticleSystem.EmitterShape.SkinnedMeshRenderer:

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(skinnedMeshEmitter);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.PrepareSkinnedMesh();
				}

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Precision " + (res.intValue * res.intValue).ToString());

					EditorGUI.BeginChangeCheck();
					skinnedMeshEmitterResolution.intValue = EditorGUILayout.IntSlider(skinnedMeshEmitterResolution.intValue, 16, 512);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateskinnedMeshEmitterPositionTexture();
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUI.BeginChangeCheck();
					layerMask.intValue = EditorGUILayout.IntField(new GUIContent("Layer", "The Skinned mesh emitter must be in a sepparate layer, shared only with other skinned mesh emitters."), layerMask.intValue);

					layerMask.intValue = Mathf.Clamp(layerMask.intValue, 0, 31);
					EditorGUILayout.LabelField("Layer: " + LayerMask.LayerToName(layerMask.intValue));

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateSkinnedMeshEmitterCameraLayer();
					}
				}
				EditorGUILayout.EndHorizontal();
				break;
        }
    }

    private void DrawParticleTypeOptions(GPUParticleSystem.ParticleType type)
    {
        SerializedProperty stretch = serializedObject.FindProperty("stretchMultiplier");
        SerializedProperty minMax = serializedObject.FindProperty("minMaxStretch");
        SerializedProperty meshParticle = serializedObject.FindProperty("meshParticle");
        //SerializedProperty res = serializedObject.FindProperty("meshParticleResolution");

        switch (type)
        {
            case GPUParticleSystem.ParticleType.StretchedBillboard:
                EditorGUI.BeginChangeCheck();
                stretch.floatValue = EditorGUILayout.FloatField("Stretch multiplier", stretch.floatValue);
				minMax.vector2Value = EditorGUILayout.Vector2Field("Min Max stretch", minMax.vector2Value);

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.UpdateStretchMultiplier();
                }
                break;
            case GPUParticleSystem.ParticleType.StretchedTail:
                EditorGUI.BeginChangeCheck();
                stretch.floatValue = EditorGUILayout.FloatField("Stretch multiplier", stretch.floatValue);
				minMax.vector2Value = EditorGUILayout.Vector2Field("Min Max stretch", minMax.vector2Value);
				if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    particleSystem.UpdateStretchMultiplier();
                }
                break;
				
            case GPUParticleSystem.ParticleType.Mesh:
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel(new GUIContent("Mesh", "Choose a mesh that will be used as a particle."));
                    EditorGUI.BeginChangeCheck();
                    meshParticle.objectReferenceValue = EditorGUILayout.ObjectField(meshParticle.objectReferenceValue, typeof(Mesh), false);
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                        particleSystem.ForceRecreateParticles();
                    }
                }
                EditorGUILayout.EndHorizontal();
                break;
				/*
            case GPUParticleSystem.ParticleType.AnimatedMesh:
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel(new GUIContent("Mesh", "Choose a mesh that will be used as a particle."));
                    meshParticle.objectReferenceValue = EditorGUILayout.ObjectField(meshParticle.objectReferenceValue, typeof(Mesh), false);
                }
                EditorGUILayout.EndHorizontal();
                break;
				*/
        }
    }

    private bool DrawOnOffToggle(bool OnOff, string name)
    {
        EditorGUILayout.BeginHorizontal();
        {
            //GUILayout.FlexibleSpace();
            if (OnOff)
            {
                EditorGUILayout.PrefixLabel(name);

                GUILayout.FlexibleSpace();

                GUI.color = new Color(0.5f,1f,0.5f,1f);
                if (GUILayout.Button("On", EditorStyles.toolbarButton, GUILayout.MaxWidth(35f)))
                {
                    OnOff = true;
                }
                GUI.color = Color.white;

                if (GUILayout.Button("Off", EditorStyles.toolbarButton, GUILayout.MaxWidth(35f)))
                {
                    OnOff = false;
                }                
            }
            else {
                EditorGUILayout.PrefixLabel(name);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("On", EditorStyles.toolbarButton, GUILayout.MaxWidth(35f)))
                {
                    OnOff = true;
                }

                GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
				if (GUILayout.Button("Off", EditorStyles.toolbarButton, GUILayout.MaxWidth(35f)))
                {
                    OnOff = false;
                }
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();

        return OnOff;
    }

	private bool DrawOnOffToggleBundle(bool OnOff, string name)
	{
		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.Label(name, EditorStyles.toolbarButton);

			if (OnOff)
			{
				GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
				if (GUILayout.Button("On", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
				{
					OnOff = true;
				}
				GUI.color = Color.white;

				if (GUILayout.Button("Off", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
				{
					OnOff = false;
				}
			}
			else
			{
				if (GUILayout.Button("On", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
				{
					OnOff = true;
				}

				GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
				if (GUILayout.Button("Off", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
				{
					OnOff = false;
				}
				GUI.color = Color.white;
			}
		}
		EditorGUILayout.EndHorizontal();
		return OnOff;
	}
}