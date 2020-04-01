using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;

[CustomEditor(typeof(GPUParticleSystem))]
public partial class GPUParticleSystemEditor : Editor
{
	GPUParticleSystem particleSystem;

	#region Properties
#pragma warning disable 414
	SerializedProperty playOnAwake;
	SerializedProperty loop;
	SerializedProperty effectLength;
	SerializedProperty maxParticles;
	SerializedProperty bufferWidth;
	SerializedProperty bufferHeight;
	SerializedProperty useFixedDeltaTime;
	SerializedProperty fixedDeltaTime;
	SerializedProperty timeScale;
	SerializedProperty seed;

	SerializedProperty param1;
	SerializedProperty param2;
	SerializedProperty param3;
	SerializedProperty param4;
	SerializedProperty randomness;
	SerializedProperty emitFromShell;
	SerializedProperty emitFromBase;
	SerializedProperty bursts;

	SerializedProperty useRotation;
	SerializedProperty useMaxVelocity;
	SerializedProperty useCircularForce;
	SerializedProperty circularForceCenter;
	SerializedProperty useInheritVelocity;

	SerializedProperty gravity;
	SerializedProperty inheritVelocity;
	SerializedProperty airResistance;

	SerializedProperty mainTexture;
	SerializedProperty motionVectors;
	SerializedProperty useZbuffer;
	SerializedProperty textureSheetMode;
	SerializedProperty textureSheetRandomIndex;
	SerializedProperty rows;
	SerializedProperty columns;

	SerializedProperty fgaFile;
	SerializedProperty vectorNoise;
	SerializedProperty vectorField;

	SerializedProperty collisionType;
	SerializedProperty planePositions;
	SerializedProperty planeNormals;
	SerializedProperty planes;
	SerializedProperty collisionDamping;
	SerializedProperty collisionCamera;
	SerializedProperty collisionCameraTransform;
	SerializedProperty depthCollisionDamping;
	SerializedProperty dampingRandomness;
	SerializedProperty depthCollisionDistance;

	SerializedProperty useMeshTarget;
	SerializedProperty useMeshFilter;
	SerializedProperty targetIsSameMeshAsEmitter;
	SerializedProperty meshTarget;
	SerializedProperty meshFilterTarget;
	SerializedProperty meshTargetResolution;
	SerializedProperty meshTargetBakeTyp;

	SerializedProperty skinnedMeshEmitter;
	SerializedProperty skinnedMeshEmitterResolution;
	SerializedProperty layerMask;

	SerializedProperty refractionNormals;
	SerializedProperty indexOfRefraction;

	SerializedProperty forwardVector;
	SerializedProperty aspectRatio;
	SerializedProperty offset;
	SerializedProperty motionVectorStrength;
	SerializedProperty castShadows;
	SerializedProperty receiveShadows;

	SerializedProperty followSpeed;

	//Enums
	SerializedProperty particleType;
	SerializedProperty blendMode;
	SerializedProperty simulationSpace;
	SerializedProperty emitterShape;
	SerializedProperty precision;
	SerializedProperty turbulenceType;
	SerializedProperty tightness;
	SerializedProperty lightMode;
	SerializedProperty metallic;
	SerializedProperty smoothness;
	SerializedProperty metallicSmoothness;
	SerializedProperty normalMap;
	SerializedProperty emissionMap;

	//Editor
	SerializedProperty GeneralsTab;
	SerializedProperty EmitterTab;
	SerializedProperty EmissionTab;
	SerializedProperty StartValuesTab;
	SerializedProperty LifetimeValuesTab;
	SerializedProperty ForcesTab;
	SerializedProperty TurbulenceTab;
	SerializedProperty AttractorsTab;
	SerializedProperty MeshTargetTab;
	SerializedProperty CollisionTab;
	SerializedProperty RenderingTab;
	SerializedProperty MaterialTab;
	SerializedProperty EditBounds;
	SerializedProperty ShowWSBounds;

	SerializedProperty extents;
	SerializedProperty center;

	SerializedProperty renderQueue;

	SerializedProperty attractors;

#pragma warning restore 414
	#endregion

	[MenuItem("GameObject/Effects/GPU Particle System", false, 2000)]
	public static void NewGPUParticleSystem()
	{
		GameObject g = new GameObject("GPU Particle System");
		g.AddComponent<GPUParticleSystem>();
		g.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
	}

	private void OnEnable()
	{
		playOnAwake = serializedObject.FindProperty("playOnAwake");
		loop = serializedObject.FindProperty("loop");
		effectLength = serializedObject.FindProperty("effectLength");
		maxParticles = serializedObject.FindProperty("maxParticles");
		bufferWidth = serializedObject.FindProperty("bufferWidth");
		bufferHeight = serializedObject.FindProperty("bufferHeight");
		useFixedDeltaTime = serializedObject.FindProperty("useFixedDeltaTime");
		fixedDeltaTime = serializedObject.FindProperty("fixedDeltaTime");
		timeScale = serializedObject.FindProperty("timeScale");
		seed = serializedObject.FindProperty("seed");

		param1 = serializedObject.FindProperty("param1");
		param2 = serializedObject.FindProperty("param2");
		param3 = serializedObject.FindProperty("param3");
		param4 = serializedObject.FindProperty("param4");
		randomness = serializedObject.FindProperty("randomness");
		emitFromShell = serializedObject.FindProperty("emitFromShell");
		emitFromBase = serializedObject.FindProperty("emitFromBase");
		bursts = serializedObject.FindProperty("bursts");

		useRotation = serializedObject.FindProperty("useRotation");
		useMaxVelocity = serializedObject.FindProperty("useMaxVelocity");
		useCircularForce = serializedObject.FindProperty("useCircularForce");
		circularForceCenter = serializedObject.FindProperty("circularForceCenter");
		useInheritVelocity = serializedObject.FindProperty("useInheritVelocity");

		gravity = serializedObject.FindProperty("gravity");
		inheritVelocity = serializedObject.FindProperty("inheritVelocity");
		airResistance = serializedObject.FindProperty("airResistance");

		mainTexture = serializedObject.FindProperty("mainTexture");
		useZbuffer = serializedObject.FindProperty("useZbuffer");
		motionVectors = serializedObject.FindProperty("motionVectors");
		textureSheetMode = serializedObject.FindProperty("textureSheetMode");
		textureSheetRandomIndex = serializedObject.FindProperty("textureSheetRandomIndex");
		rows = serializedObject.FindProperty("rows");
		columns = serializedObject.FindProperty("columns");

		vectorNoise = serializedObject.FindProperty("vectorNoise");
		fgaFile = serializedObject.FindProperty("fgaFile");
		vectorField = serializedObject.FindProperty("vectorField");

		collisionType = serializedObject.FindProperty("collisionType");
		planePositions = serializedObject.FindProperty("planePositions");
		planeNormals = serializedObject.FindProperty("planeNormals");
		planes = serializedObject.FindProperty("planes");
		collisionDamping = serializedObject.FindProperty("collisionDamping");
		collisionCamera = serializedObject.FindProperty("collisionCamera");
		collisionCameraTransform = serializedObject.FindProperty("collisionCameraTransform");
		depthCollisionDamping = serializedObject.FindProperty("depthCollisionDamping");
		dampingRandomness = serializedObject.FindProperty("dampingRandomness");
		depthCollisionDistance = serializedObject.FindProperty("depthCollisionDistance");

		GeneralsTab = serializedObject.FindProperty("GeneralsTab");
		EmitterTab = serializedObject.FindProperty("EmitterTab");
		EmissionTab = serializedObject.FindProperty("EmissionTab");
		StartValuesTab = serializedObject.FindProperty("StartValuesTab");
		LifetimeValuesTab = serializedObject.FindProperty("LifetimeValuesTab");
		ForcesTab = serializedObject.FindProperty("ForcesTab");
		TurbulenceTab = serializedObject.FindProperty("TurbulenceTab");
		tightness = serializedObject.FindProperty("Tightness");
		CollisionTab = serializedObject.FindProperty("CollisionTab");
		AttractorsTab = serializedObject.FindProperty("AttractorsTab");
		MeshTargetTab = serializedObject.FindProperty("MeshTargetTab");
		RenderingTab = serializedObject.FindProperty("RenderingTab");
		MaterialTab = serializedObject.FindProperty("MaterialTab");
		EditBounds = serializedObject.FindProperty("EditBounds");
		ShowWSBounds = serializedObject.FindProperty("ShowWSBounds");

		extents = serializedObject.FindProperty("extents");
		center = serializedObject.FindProperty("center");

		particleType = serializedObject.FindProperty("particleType");
		blendMode = serializedObject.FindProperty("blendMode");
		simulationSpace = serializedObject.FindProperty("simulationSpace");
		emitterShape = serializedObject.FindProperty("emitterShape");
		precision = serializedObject.FindProperty("precision");
		turbulenceType = serializedObject.FindProperty("turbulenceType");
		lightMode = serializedObject.FindProperty("lightMode");
		metallic = serializedObject.FindProperty("metallic");
		smoothness = serializedObject.FindProperty("smoothness");
		metallicSmoothness = serializedObject.FindProperty("metallicSmoothness");
		normalMap = serializedObject.FindProperty("normalMap");
		emissionMap = serializedObject.FindProperty("emissionMap");

		forwardVector = serializedObject.FindProperty("forwardVector");
		aspectRatio = serializedObject.FindProperty("aspectRatio");
		offset = serializedObject.FindProperty("offset");
		motionVectorStrength = serializedObject.FindProperty("motionVectorStrength");

		attractors = serializedObject.FindProperty("attractors");
		useMeshTarget = serializedObject.FindProperty("useMeshTarget");
		useMeshFilter = serializedObject.FindProperty("useMeshFilter");
		targetIsSameMeshAsEmitter = serializedObject.FindProperty("targetIsSameMeshAsEmitter");
		meshTarget = serializedObject.FindProperty("meshTarget");
		meshFilterTarget = serializedObject.FindProperty("meshFilterTarget");
		meshTargetResolution = serializedObject.FindProperty("meshTargetResolution");
		meshTargetBakeTyp = serializedObject.FindProperty("targetBakeType");

		skinnedMeshEmitter = serializedObject.FindProperty("skinnedMeshEmitter");
		skinnedMeshEmitterResolution = serializedObject.FindProperty("skinnedMeshEmitterResolution");
		layerMask = serializedObject.FindProperty("smeLayer");

		refractionNormals = serializedObject.FindProperty("refractionNormals");
		indexOfRefraction = serializedObject.FindProperty("indexOfRefraction");

		castShadows = serializedObject.FindProperty("castShadows");
		receiveShadows = serializedObject.FindProperty("receiveShadows");
		renderQueue = serializedObject.FindProperty("renderQueue");

		followSpeed = serializedObject.FindProperty("followSpeed");

		image = new Texture2D(1, 1);
		image.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Ultimate GPU Particle System/Resources/Logo.png"));
		image.Apply();
	}

	private Texture2D image;

	public override void OnInspectorGUI()
	{
		particleSystem = (GPUParticleSystem)target;

		if (particleSystem.gameObject.scene.name == null)
		{
			EditorGUILayout.HelpBox("Can't edit prefabs at this point!", MessageType.Info);
			return;
		}

		GUILayout.Space(4f);
		Rect r = GUILayoutUtility.GetRect(Screen.width, (Screen.width / 600f) * 110f);
		r.width = Mathf.Clamp(r.width, 60, 600);
		r.height = Mathf.Clamp(r.height, 11, 110);
		EditorGUI.DrawPreviewTexture(r, image);

		#region GeneralsTab
		if (GUILayout.Button("General", EditorStyles.toolbarDropDown))
		{
			GeneralsTab.boolValue = !GeneralsTab.boolValue;
		}

		if (GeneralsTab.boolValue == true)
		{
			GUILayout.Space(-4f);
			EditorGUILayout.BeginVertical("Box");
			{
				effectLength.floatValue = EditorGUILayout.FloatField("Effect length", effectLength.floatValue);
				loop.boolValue = DrawOnOffToggle(loop.boolValue, "Loop");
				playOnAwake.boolValue = DrawOnOffToggle(playOnAwake.boolValue, "Play on awake");

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(simulationSpace);

				if (EditorGUI.EndChangeCheck())
				{
					if (EditBounds.boolValue)
					{
						EditBounds.boolValue = false;
						particleSystem.EndEditBB();
					}

					serializedObject.ApplyModifiedProperties();
					particleSystem.SetSimulationSpace();
					particleSystem.PrepareParticleData();
					particleSystem.ForceRecreateParticles();
				}
				GUILayout.Space(15f);

				EditorGUI.BeginChangeCheck();
				if (particleSystem.particleType == GPUParticleSystem.ParticleType.Trails)
				{
					bufferHeight.intValue = EditorGUILayout.DelayedIntField("Num Trails", bufferHeight.intValue);
					bufferWidth.intValue = EditorGUILayout.DelayedIntField("Num Segments", bufferWidth.intValue);
				}
				else
				{
					bufferWidth.intValue = EditorGUILayout.DelayedIntField("Buffer Width", bufferWidth.intValue);
					bufferHeight.intValue = EditorGUILayout.DelayedIntField("Buffer Height", bufferHeight.intValue);
					EditorGUILayout.LabelField("Max Particles:", maxParticles.intValue.ToString());
				}

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.PrepareParticleData();
					particleSystem.ForceRecreateParticles();
				}

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(precision);

				if (EditorGUI.EndChangeCheck())
				{
					particleSystem.ForceRecreateParticles();
				}

				useFixedDeltaTime.boolValue = DrawOnOffToggle(useFixedDeltaTime.boolValue, "Use fixed delta time");
				EditorGUI.BeginDisabledGroup(!useFixedDeltaTime.boolValue);
				{
					fixedDeltaTime.floatValue = EditorGUILayout.FloatField("Fixed delta time", fixedDeltaTime.floatValue);
				}
				EditorGUI.EndDisabledGroup();

				timeScale.floatValue = EditorGUILayout.FloatField("Time Scale", timeScale.floatValue);

				EditorGUI.BeginChangeCheck();
				seed.intValue = EditorGUILayout.DelayedIntField("Seed", seed.intValue);
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.ResetSeed();
				}

				EditorGUILayout.BeginVertical("Box");
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField("Axis aligned bounds", EditorStyles.boldLabel);

						if (EditBounds.boolValue)
						{
							if (GUILayout.Button("Apply", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 6f)))
							{
								EditBounds.boolValue = !EditBounds.boolValue;
								serializedObject.ApplyModifiedProperties();
								particleSystem.PrepareParticleData();
								particleSystem.ForceRecreateParticles();
								particleSystem.EndEditBB();
							}
						}
						else
						{
							if (GUILayout.Button("Edit", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 6f)))
							{
								EditBounds.boolValue = !EditBounds.boolValue;
								ShowWSBounds.boolValue = false;
								particleSystem.StartEditBB();
							}
						}
					}
					EditorGUILayout.EndHorizontal();

					if (EditBounds.boolValue)
					{
						center.vector3Value = EditorGUILayout.Vector3Field("Center", center.vector3Value);
						extents.vector3Value = EditorGUILayout.Vector3Field("Extends", extents.vector3Value);
					}
					else
					{
						EditorGUILayout.BeginHorizontal();
						{
							EditorGUILayout.LabelField("World space bounds");
							if (!ShowWSBounds.boolValue)
							{
								if (GUILayout.Button("Show", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 6f)))
								{
									ShowWSBounds.boolValue = !ShowWSBounds.boolValue;
								}
							}
							else
							{
								if (GUILayout.Button("Hide", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 6f)))
								{
									ShowWSBounds.boolValue = !ShowWSBounds.boolValue;
								}
							}
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}
		#endregion

		#region EmitterTab
		if (GUILayout.Button("Emitter", EditorStyles.toolbarDropDown))
		{
			EmitterTab.boolValue = !EmitterTab.boolValue;
		}

		if (EmitterTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(emitterShape);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetEmitterShapeKeyword();

					if (particleSystem.emitterShape == GPUParticleSystem.EmitterShape.SkinnedMeshRenderer)
					{
						particleSystem.SetupSkinnedMeshEmitterCamera();
					}
					else
					{
						particleSystem.DeactivateSkinnedMeshEmitter();
					}

				}

				DrawEmitterOptions(particleSystem.emitterShape);
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region EmissionTab
		if (GUILayout.Button("Emission", EditorStyles.toolbarDropDown))
		{
			EmissionTab.boolValue = !EmissionTab.boolValue;
		}

		if (EmissionTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawFloatCurveBundel(particleSystem.emissionRate, "emissionRate", "Rate");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}

				EditorGUILayout.BeginVertical("Box");
				{
					EditorGUILayout.BeginHorizontal("Toolbar");
					{
						EditorGUILayout.LabelField("Time", GUILayout.MaxWidth(Screen.width / 8f));
						EditorGUILayout.LabelField("Min", GUILayout.MaxWidth(Screen.width / 8f));
						EditorGUILayout.LabelField("Max", GUILayout.MaxWidth(Screen.width / 8f));
						EditorGUILayout.LabelField("Probability", GUILayout.MaxWidth(Screen.width / 4f));
						GUILayout.FlexibleSpace();
						EditorGUILayout.LabelField("Once", GUILayout.MaxWidth(Screen.width / 8f));
					}
					EditorGUILayout.EndHorizontal();

					for (int i = 0; i < bursts.arraySize; i++)
					{
						EditorGUILayout.BeginHorizontal();
						{
							bursts.GetArrayElementAtIndex(i).FindPropertyRelative("burstTime").floatValue = Mathf.Clamp(EditorGUILayout.FloatField(bursts.GetArrayElementAtIndex(i).FindPropertyRelative("burstTime").floatValue, GUILayout.MaxWidth(Screen.width / 8f)), 0f, particleSystem.effectLength);
							bursts.GetArrayElementAtIndex(i).FindPropertyRelative("minBurst").intValue = EditorGUILayout.IntField(bursts.GetArrayElementAtIndex(i).FindPropertyRelative("minBurst").intValue, GUILayout.MaxWidth(Screen.width / 8f));
							bursts.GetArrayElementAtIndex(i).FindPropertyRelative("maxBurst").intValue = EditorGUILayout.IntField(bursts.GetArrayElementAtIndex(i).FindPropertyRelative("maxBurst").intValue, GUILayout.MaxWidth(Screen.width / 8f));
							bursts.GetArrayElementAtIndex(i).FindPropertyRelative("burstProbability").floatValue = EditorGUILayout.Slider(bursts.GetArrayElementAtIndex(i).FindPropertyRelative("burstProbability").floatValue, 0f, 1f, GUILayout.MaxWidth(Screen.width / 2f));
							//GUILayout.FlexibleSpace();
							//bursts.GetArrayElementAtIndex(i).FindPropertyRelative("playOnce").boolValue = EditorGUILayout.Toggle(bursts.GetArrayElementAtIndex(i).FindPropertyRelative("playOnce").boolValue, GUILayout.MaxWidth(Screen.width / 8f));
						}
						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.BeginHorizontal();
					{
						if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
						{
							particleSystem.AddBurst();
						}

						if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
						{
							particleSystem.RemoveLastBurst();
						}
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region StartValuesTab
		if (GUILayout.Button("Start values", EditorStyles.toolbarDropDown))
		{
			StartValuesTab.boolValue = !StartValuesTab.boolValue;
		}

		if (StartValuesTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawFloatCurveBundel(particleSystem.startSpeed, "startSpeed", "Start speed");
				DrawFloatCurveBundel(particleSystem.startLifetime, "startLifetime", "Start lifetime");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateStartSpeedAndLifetime();
				}

				EditorGUI.BeginChangeCheck();
				DrawFloatCurveBundel(particleSystem.startSize, "startSize", "Start size");

				GUILayout.Space(5f);

				EditorGUILayout.BeginVertical();
				{
					EditorGUI.BeginChangeCheck();
					useRotation.boolValue = DrawOnOffToggleBundle(useRotation.boolValue, "Enable Rotation");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.SetRotationKeyword();
					}

					if (useRotation.boolValue)
					{
						DrawFloatCurveBundel(particleSystem.startRotation, "startRotation", "Start rotation");
					}

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateStartSizeAndRotation();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region LifetimeValuesTab
		if (GUILayout.Button("Lifetime values", EditorStyles.toolbarDropDown))
		{
			LifetimeValuesTab.boolValue = !LifetimeValuesTab.boolValue;
		}

		if (LifetimeValuesTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawShaderCurveBundel(particleSystem.sizeOverLifetime, "sizeOverLifetime", "Size over lifetime");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateSizeOverLifetime();
				}

				EditorGUI.BeginDisabledGroup(!useRotation.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawShaderCurveBundel(particleSystem.rotationOverLifetime, "rotationOverLifetime", "Rotation over lifetime");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateRotationOverLifetime();
					}
				}
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginChangeCheck();
				DrawColorGradientBundel(particleSystem.colorOverLifetime, "colorOverLifetime", "Color over lifetime");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateColorOverLifeTime();
				}

				EditorGUI.BeginChangeCheck();
				DrawSingleFloatCurveBundel(particleSystem.colorIntensityOverLifetime, "colorIntensityOverLifetime", "Color intensity over lifetime");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateColorIntensity();
				}

				GUILayout.Space(5f);

				EditorGUI.BeginChangeCheck();
				useMaxVelocity.boolValue = DrawOnOffToggleBundle(useMaxVelocity.boolValue, "Enable max velocity");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetLimitVelocity();
				}

				EditorGUI.BeginDisabledGroup(!useMaxVelocity.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawSingleFloatCurveBundel(particleSystem.maxVelocity, "maxVelocity", "Max velocity");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateMaxVelocityOverLifetime();
					}
				}
				EditorGUI.EndDisabledGroup();

			}
			EditorGUILayout.EndHorizontal();
		}
		#endregion

		#region Forces
		if (GUILayout.Button("Forces", EditorStyles.toolbarDropDown))
		{
			ForcesTab.boolValue = !ForcesTab.boolValue;
		}

		if (ForcesTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawSingleFloatCurveBundel(particleSystem.gravity, "gravity", "Gravity");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateGravity();
				}

				EditorGUI.BeginChangeCheck();
				DrawSingleFloatCurveBundel(particleSystem.airResistance, "airResistance", "Air resistance (drag)");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateAirResistance();
				}

				EditorGUI.BeginChangeCheck();
				DrawVector3CurveBundel(particleSystem.forceOverLifetime, "forceOverLifetime", "Force over lifetime");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateForceOverLifetime();
				}

				GUILayout.Space(5f);

				EditorGUI.BeginChangeCheck();
				useInheritVelocity.boolValue = DrawOnOffToggleBundle(useInheritVelocity.boolValue, "Enable inherit velocity");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}

				EditorGUI.BeginDisabledGroup(!useInheritVelocity.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawSingleFloatCurveBundel(particleSystem.inheritVelocityMultiplyer, "inheritVelocityMultiplyer", "Inherit velocity");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
					}
				}
				EditorGUI.EndDisabledGroup();

				GUILayout.Space(5f);

				EditorGUI.BeginChangeCheck();
				useCircularForce.boolValue = DrawOnOffToggleBundle(useCircularForce.boolValue, "Enable circular force");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetCircularForce();
				}

				EditorGUI.BeginDisabledGroup(!useCircularForce.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.circularForce, "circularForce", "Circular force");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateCircularForceOverLifetime();
					}

					circularForceCenter.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Center", "Assign a transform or use default."), circularForceCenter.objectReferenceValue, typeof(Transform), true);
				}
				EditorGUI.EndDisabledGroup();

			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Turbulence
		if (GUILayout.Button("Turbulence", EditorStyles.toolbarDropDown))
		{
			TurbulenceTab.boolValue = !TurbulenceTab.boolValue;
		}

		if (TurbulenceTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(turbulenceType);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetTurbulenceKeyword();

					if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.Texture)
					{
						particleSystem.UpdateTurbulenceTexture();
					}

					if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.VectorField)
					{
						particleSystem.UpdateVectorfieldFile();
						particleSystem.UpdateVectorField();
					}
				}

				if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.Texture)
				{
					EditorGUI.BeginChangeCheck();
					vectorNoise.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Noise (RGBA)", "Assign a texture to enable."), vectorNoise.objectReferenceValue, typeof(Texture2D), false);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateTurbulenceTexture();
					}
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.turbulenceAmplitude, "turbulenceAmplitude", "Amplitude");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateAmplitude();
					}
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.turbulenceFrequency, "turbulenceFrequency", "Frequency");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateFrequency();
					}
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.turbulenceOffset, "turbulenceOffset", "Offset (speed)");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateOffset();
					}
				}

				if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.VectorField)
				{
					if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ||
						EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS ||
						EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
					{
						EditorGUILayout.HelpBox("Vector field turbulence is not supported on your current build target.", MessageType.Warning);
					}

					EditorGUI.BeginChangeCheck();
					fgaFile.objectReferenceValue = EditorGUILayout.ObjectField("Vectorfield file (fga)", fgaFile.objectReferenceValue, typeof(TextAsset), false);
					if (EditorGUI.EndChangeCheck())
					{
						particleSystem.UpdateVectorfieldFile();
						particleSystem.UpdateVectorField();
					}

					EditorGUI.BeginDisabledGroup(true);
					{
						EditorGUI.BeginChangeCheck();
						vectorField.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Noise (RGBA)", "Assign a texture to enable."), vectorField.objectReferenceValue, typeof(Texture3D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateVectorField();
						}
					}
					EditorGUI.EndDisabledGroup();

					EditorGUILayout.BeginVertical("Box");
					{
						EditorGUI.BeginChangeCheck();
						DrawVector3CurveBundel(particleSystem.turbulenceAmplitude, "turbulenceAmplitude", "Amplitude");
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateAmplitude();
						}

						EditorGUI.BeginChangeCheck();
						DrawVector3CurveBundel(particleSystem.turbulenceFrequency, "turbulenceFrequency", "Frequency");
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateFrequency();
						}

						EditorGUI.BeginChangeCheck();
						DrawVector3CurveBundel(particleSystem.turbulenceRotation, "turbulenceRotation", "Rotation");
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateVectorFieldMatrix();
						}

						EditorGUI.BeginChangeCheck();
						tightness.floatValue = EditorGUILayout.Slider("Tightness", tightness.floatValue, 0f, 1f);
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateTightness();
						}
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Attractors
		if (GUILayout.Button("Attractors", EditorStyles.toolbarDropDown))
		{
			AttractorsTab.boolValue = !AttractorsTab.boolValue;
		}

		if (AttractorsTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(Screen.width / 4f));
					EditorGUILayout.LabelField("Strength", GUILayout.MaxWidth(Screen.width / 4f));
					EditorGUILayout.LabelField("Attenuation", GUILayout.MaxWidth(Screen.width / 4f));
				}
				EditorGUILayout.EndHorizontal();

				for (int i = 0; i < attractors.arraySize; i++)
				{
					EditorGUILayout.BeginHorizontal();
					{
						GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
						if (GUILayout.Button("x", EditorStyles.toolbarButton))
						{
							particleSystem.RemoveAttractor(i);
						}
						GUI.color = new Color(0.5f, 1f, 0.5f, 1f);

						if (GUILayout.Button("+ =>", EditorStyles.toolbarButton))
						{
							GameObject g = new GameObject("Attractor " + i.ToString());
							attractors.GetArrayElementAtIndex(i).FindPropertyRelative("attractor").objectReferenceValue = g.transform;
							serializedObject.ApplyModifiedProperties();
						}
						GUI.color = Color.white;

						attractors.GetArrayElementAtIndex(i).FindPropertyRelative("attractor").objectReferenceValue = EditorGUILayout.ObjectField(attractors.GetArrayElementAtIndex(i).FindPropertyRelative("attractor").objectReferenceValue, typeof(Transform), true) as Transform;
						attractors.GetArrayElementAtIndex(i).FindPropertyRelative("strength").floatValue = EditorGUILayout.FloatField(attractors.GetArrayElementAtIndex(i).FindPropertyRelative("strength").floatValue);
						attractors.GetArrayElementAtIndex(i).FindPropertyRelative("attenuation").floatValue = EditorGUILayout.FloatField(attractors.GetArrayElementAtIndex(i).FindPropertyRelative("attenuation").floatValue);
					}
					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
					{
						particleSystem.AddAttractor();
						particleSystem.SetAttractorKeyword();
					}

					if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
					{
						particleSystem.RemoveLastAttractor();
						particleSystem.SetAttractorKeyword();
					}

					GUILayout.FlexibleSpace();
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region MeshTarget
		if (GUILayout.Button("Mesh Target", EditorStyles.toolbarDropDown))
		{
			MeshTargetTab.boolValue = !MeshTargetTab.boolValue;
		}

		if (MeshTargetTab.boolValue)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				useMeshTarget.boolValue = DrawOnOffToggle(useMeshTarget.boolValue, "Use Mesh Target");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetMeshTargetKeyword();
				}

				if (useMeshTarget.boolValue)
				{
					useMeshFilter.boolValue = DrawOnOffToggle(useMeshFilter.boolValue, "Use Mesh Filter");

					if (particleSystem.emitterShape == GPUParticleSystem.EmitterShape.Mesh || particleSystem.emitterShape == GPUParticleSystem.EmitterShape.Mesh)
					{
						targetIsSameMeshAsEmitter.boolValue = DrawOnOffToggle(targetIsSameMeshAsEmitter.boolValue, "Use Mesh Emitter");
					}

					if (!useMeshFilter.boolValue)
					{
						EditorGUI.BeginChangeCheck();

						EditorGUILayout.BeginHorizontal();
						{
							EditorGUILayout.PrefixLabel("Mesh");
							meshTarget.objectReferenceValue = EditorGUILayout.ObjectField(meshTarget.objectReferenceValue, typeof(Mesh), false);
						}
						EditorGUILayout.EndHorizontal();

						EditorGUI.BeginDisabledGroup(targetIsSameMeshAsEmitter.boolValue);
						{
							EditorGUILayout.PropertyField(meshTargetBakeTyp);

							EditorGUILayout.BeginHorizontal();
							{
								EditorGUILayout.PrefixLabel("Resolution");
								meshTargetResolution.intValue = EditorGUILayout.IntSlider(meshTargetResolution.intValue, 8, 256);
							}
							EditorGUILayout.EndHorizontal();
						}
						EditorGUI.EndDisabledGroup();

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateMeshTargetTexture();
						}

						EditorGUI.BeginChangeCheck();
						DrawSingleFloatCurveBundel(particleSystem.meshTargetStrength, "meshTargetStrength", "Strength");
						DrawSingleFloatCurveBundel(particleSystem.onTarget, "onTarget", "On Target");

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateMeshTargetParameters();
						}
					}
					else
					{
						EditorGUI.BeginChangeCheck();

						EditorGUILayout.BeginHorizontal();
						{
							EditorGUILayout.PrefixLabel("Mesh Filter");
							meshFilterTarget.objectReferenceValue = EditorGUILayout.ObjectField(meshFilterTarget.objectReferenceValue, typeof(MeshFilter), true);
						}
						EditorGUILayout.EndHorizontal();

						EditorGUI.BeginDisabledGroup(targetIsSameMeshAsEmitter.boolValue);
						{
							EditorGUILayout.PropertyField(meshTargetBakeTyp);

							EditorGUILayout.BeginHorizontal();
							{
								EditorGUILayout.PrefixLabel("Resolution");
								meshTargetResolution.intValue = EditorGUILayout.IntSlider(meshTargetResolution.intValue, 8, 256);
							}
							EditorGUILayout.EndHorizontal();
						}
						EditorGUI.EndDisabledGroup();

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateMeshTargetTexture();
						}

						EditorGUI.BeginChangeCheck();
						DrawSingleFloatCurveBundel(particleSystem.meshTargetStrength, "meshTargetStrength", "Strength");
						DrawSingleFloatCurveBundel(particleSystem.onTarget, "onTarget", "On Target");

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateMeshTargetParameters();
						}
					}

					if (GUILayout.Button("Update"))
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateMeshTargetTexture();
					}
				}

			}
			EditorGUILayout.EndVertical();
		}

		#endregion

		#region Collision
		if (GUILayout.Button("Collision", EditorStyles.toolbarDropDown))
		{
			CollisionTab.boolValue = !CollisionTab.boolValue;
		}

		if (CollisionTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(collisionType);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetCollisionType();
				}

				if (collisionType.enumValueIndex == (int)GPUParticleSystem.CollisionType.Planes)
				{
					for (int i = 0; i < planePositions.arraySize; i++)
					{
						if (planes.GetArrayElementAtIndex(i).objectReferenceValue == null)
						{
							EditorGUILayout.BeginHorizontal();
							{
								if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width / 8f)))
								{
									GameObject g = new GameObject("Collision Plane");
									planes.GetArrayElementAtIndex(i).objectReferenceValue = g.transform;
									collisionDamping.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.Slider(collisionDamping.GetArrayElementAtIndex(i).floatValue, 0.0f, 1.0f);
									serializedObject.ApplyModifiedProperties();
								}
								planes.GetArrayElementAtIndex(i).objectReferenceValue = EditorGUILayout.ObjectField(planes.GetArrayElementAtIndex(i).objectReferenceValue, typeof(Transform), true);
							}
							EditorGUILayout.EndVertical();

							break;
						}
						else
						{
							EditorGUILayout.BeginHorizontal();
							{
								GUI.color = Color.red;
								if (GUILayout.Button("x", EditorStyles.toolbarButton, GUILayout.MaxWidth(20f)))
								{
									DestroyImmediate(((Transform)planes.GetArrayElementAtIndex(i).objectReferenceValue).gameObject);
								}
								GUI.color = Color.white;

								planes.GetArrayElementAtIndex(i).objectReferenceValue = EditorGUILayout.ObjectField(planes.GetArrayElementAtIndex(i).objectReferenceValue, typeof(Transform), true);
								collisionDamping.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.Slider(collisionDamping.GetArrayElementAtIndex(i).floatValue, 0.0f, 1.0f);
							}
							EditorGUILayout.EndVertical();
						}
					}
				}

				if (collisionType.enumValueIndex == (int)GPUParticleSystem.CollisionType.Depth)
				{
					EditorGUI.BeginChangeCheck();
					collisionCamera.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Camera", "Pick a camer that is used to compute collision from."), collisionCamera.objectReferenceValue, typeof(Camera), true);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						Camera c = collisionCamera.objectReferenceValue as Camera;
						particleSystem.UpdateCollisionCamera();		
					}

					EditorGUI.BeginChangeCheck();

					depthCollisionDamping.floatValue = EditorGUILayout.Slider("Damping", depthCollisionDamping.floatValue, 0f, 1f);
					dampingRandomness.floatValue = EditorGUILayout.Slider("Damping randomness", dampingRandomness.floatValue, 0f, 1f);

					GUILayout.Space(5f);
					depthCollisionDistance.floatValue = EditorGUILayout.Slider("Collision Distance", depthCollisionDistance.floatValue, 0.01f, 1f);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateDepthCollisionValues();
					}

					EditorGUILayout.HelpBox("In order to work correctly, the game view camera used to generate the collision data must be rendering and Scene lighting must be disabled in the scene view.", MessageType.Warning);
				}
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Rendering
		if (GUILayout.Button("Rendering", EditorStyles.toolbarDropDown))
		{
			RenderingTab.boolValue = !RenderingTab.boolValue;
		}

		if (RenderingTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(particleType);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.ForceRecreateParticles();
					particleSystem.UpdateParticleTypeKeywords();
					particleSystem.SetParticleTypeKeyword(particleSystem.particleType);
					particleSystem.ClearParticleData();
					particleSystem.PrepareParticleData();
					particleSystem.ForceRecreateParticles();
				}

				DrawParticleTypeOptions(particleSystem.particleType);

				if (particleSystem.particleType == GPUParticleSystem.ParticleType.Mesh)
				{
					forwardVector.vector3Value = EditorGUILayout.Vector3Field(new GUIContent("Forward Vector", "Defines which axis of the mesh is facing forward"), forwardVector.vector3Value);
				}
				else
				{
					EditorGUI.BeginChangeCheck();
					offset.vector3Value = EditorGUILayout.Vector3Field("Offset", offset.vector3Value);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdatePositionOffset();
					}
				}
				

				GUILayout.Space(15f);
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Material
		if (GUILayout.Button("Material options", EditorStyles.toolbarDropDown))
		{
			MaterialTab.boolValue = !MaterialTab.boolValue;
		}

		if (MaterialTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(lightMode);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetLightMode();
				}

				switch (lightMode.enumValueIndex)
				{
					case (int)GPUParticleSystem.LightMode.Off:
						EditorGUI.BeginChangeCheck();
						useZbuffer.boolValue = DrawOnOffToggle(useZbuffer.boolValue, "Z-Buffer");

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetZBuffer();
						}

						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(blendMode);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetBlendMode();
						}

						EditorGUI.BeginChangeCheck();
						mainTexture.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Main Texture", "Assign a texture to enable."), mainTexture.objectReferenceValue, typeof(Texture2D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetMainTexture();
						}

						EditorGUI.BeginChangeCheck();
						aspectRatio.floatValue = EditorGUILayout.FloatField("Aspect Ratio", aspectRatio.floatValue);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateAspectRatio();
						}

						EditorGUILayout.BeginVertical("Box");
						{
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.PropertyField(textureSheetMode);

							if (EditorGUI.EndChangeCheck())
							{
								serializedObject.ApplyModifiedProperties();
								particleSystem.SetTextureSheetKeyword((GPUParticleSystem.TextureSheetMode)textureSheetMode.enumValueIndex);
							}

							if (textureSheetMode.enumValueIndex != 0)
							{
								EditorGUI.BeginChangeCheck();
								textureSheetRandomIndex.boolValue = DrawOnOffToggle(textureSheetRandomIndex.boolValue, "Random frame");
								if (EditorGUI.EndChangeCheck())
								{
									serializedObject.ApplyModifiedProperties();
									particleSystem.SetRandomIndexKeyword(textureSheetRandomIndex.boolValue);
								}

								EditorGUI.BeginChangeCheck();
								rows.intValue = EditorGUILayout.IntField("Rows", rows.intValue);
								columns.intValue = EditorGUILayout.IntField("Columns", columns.intValue);
								if (EditorGUI.EndChangeCheck())
								{
									serializedObject.ApplyModifiedProperties();
									particleSystem.UpdateTextureSheetDimensions(rows.intValue, columns.intValue);
								}

								if (textureSheetMode.enumValueIndex == 3)
								{
									EditorGUI.BeginChangeCheck();
									motionVectors.objectReferenceValue = EditorGUILayout.ObjectField("Motion Vector Texture", motionVectors.objectReferenceValue, typeof(Texture2D), false);

									if (EditorGUI.EndChangeCheck())
									{
										serializedObject.ApplyModifiedProperties();
										particleSystem.UpdateMotionVectorTexture(null);
									}

									EditorGUI.BeginChangeCheck();
									motionVectorStrength.floatValue = EditorGUILayout.FloatField("Motion Vector Strength", motionVectorStrength.floatValue);

									if (EditorGUI.EndChangeCheck())
									{
										serializedObject.ApplyModifiedProperties();
										particleSystem.UpdateMotionVectorStrength();
									}
								}
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUI.BeginChangeCheck();
						renderQueue.intValue = EditorGUILayout.IntField("Render Queue", renderQueue.intValue);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetRenderQueue();
						}
						break;

					case (int)GPUParticleSystem.LightMode.Standard:
						EditorGUI.BeginChangeCheck();
						mainTexture.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Main Texture", "Assign a texture to enable."), mainTexture.objectReferenceValue, typeof(Texture2D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetMainTexture();
						}

						EditorGUI.BeginChangeCheck();
						aspectRatio.floatValue = EditorGUILayout.FloatField("Aspect Ratio", aspectRatio.floatValue);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateAspectRatio();
						}

						EditorGUI.BeginChangeCheck();
						metallic.floatValue = EditorGUILayout.Slider("Metallic", metallic.floatValue, 0f, 1f);
						smoothness.floatValue = EditorGUILayout.Slider("Smoothness", smoothness.floatValue, 0f, 1f);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetMetallicSmoothnessValue();
						}

						EditorGUI.BeginChangeCheck();
						metallicSmoothness.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Metallic(R) and Smoothness(A)", "Assign a texture to enable."), metallicSmoothness.objectReferenceValue, typeof(Texture2D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetMetallicSmoothnessMapTexture();
						}

						EditorGUI.BeginChangeCheck();
						normalMap.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Normal Map", "Assign a texture to enable."), normalMap.objectReferenceValue, typeof(Texture2D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetNormalMapTexture();
						}

						EditorGUI.BeginChangeCheck();
						emissionMap.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Emission Map", "Assign a texture to enable."), emissionMap.objectReferenceValue, typeof(Texture2D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetEmissionMapTexture();
						}

						EditorGUI.BeginChangeCheck();
						renderQueue.intValue = EditorGUILayout.IntField("Render Queue", renderQueue.intValue);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetRenderQueue();
						}

						EditorGUI.BeginChangeCheck();
						receiveShadows.boolValue = DrawOnOffToggle(receiveShadows.boolValue, "Receive Shadows");
						EditorGUILayout.PropertyField(castShadows);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetShadowSettings();
						}

						break;

					case (int)GPUParticleSystem.LightMode.Refraction:

						EditorGUI.BeginChangeCheck();
						useZbuffer.boolValue = DrawOnOffToggle(useZbuffer.boolValue, "Z-Buffer");

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetZBuffer();
						}
						
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(blendMode);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetBlendMode();
						}

						//EditorGUI.BeginChangeCheck();
						//mainTexture.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Main Texture", "Assign a texture to enable."), mainTexture.objectReferenceValue, typeof(Texture2D), false);

						//if (EditorGUI.EndChangeCheck())
						//{
						//	serializedObject.ApplyModifiedProperties();
						//	particleSystem.SetMainTexture();
						//}

						EditorGUI.BeginChangeCheck();
						refractionNormals.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Refraction map", "Assign a texture to enable."), refractionNormals.objectReferenceValue, typeof(Texture2D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetRefractionMapTexture();
						}

						EditorGUI.BeginChangeCheck();
						indexOfRefraction.floatValue = EditorGUILayout.Slider("Index of refraction", indexOfRefraction.floatValue, 0f, 2f);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateIndexOfRefraction();
						}

						EditorGUI.BeginChangeCheck();
						renderQueue.intValue = EditorGUILayout.IntField("Render Queue", renderQueue.intValue);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.SetRenderQueue();
						}

						break;
				}
				
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		GUILayout.Space(10f);

		serializedObject.ApplyModifiedProperties();
    }
}
