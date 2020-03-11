using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GPUParticleSystem
{
    #region Core
	public void PrepareParticleData()
	{
		maxParticles = bufferWidth * bufferHeight;

		if (precision == RenderTexturePrecision.Half)
		{
			particleData = new GPUParticleSystemBuffer(bufferWidth, bufferHeight, RenderTextureFormat.ARGBHalf);
		}
		else
		{
			particleData = new GPUParticleSystemBuffer(bufferWidth, bufferHeight, RenderTextureFormat.ARGBFloat);
		}

		SetLightMode();
		SetRenderQueue();
	}

	public void ClearParticleData()
	{
		RenderTexture.active = null;
		DestroyImmediate(particleData.newParticleBuffer);
		DestroyImmediate(particleData.metaBuffer_1);
		DestroyImmediate(particleData.metaBuffer_2);
		DestroyImmediate(particleData.positionBuffer_1);
		DestroyImmediate(particleData.positionBuffer_2);
		DestroyImmediate(particleData.velocityBuffer_1);
		DestroyImmediate(particleData.velocityBuffer_2);
		particleData = null;
	}

    private void InternalEmit()
    {
        if (startID >= maxParticles)
        {
			startID = 0f;
            endID = 0f;
        }

        if (emit)
        {
            float cEmission = GPUParticleSystemEvaluationHelper.EvaluateFloatCurveBundle(emissionRate, progress);
            endID += burstNum + cEmission * customDeltaTime;
			burstNum = 0;

			if (endID >= startID + 1)
            {
				particleData.Emit(startID, endID);
				startID = endID;
			}
            else
            {
                particleData.Emit(0f, 0f);
            }
        }
    }

	private void InternalEvaluateBurst()
	{
		float time = progress * effectLength;

		for (int i = 0; i < currentBursts.Count; i++)
		{
			if (time >= currentBursts[i].burstTime)
			{
				Emit(currentBursts[i].GetBurst());
				currentBursts.RemoveAt(i);
				i--;
			}
		}
	}

	private void InternalStep()
    {
        UpdateValues();
        particleData.StepMeta();
        particleData.StepVelocity();
		particleData.StepPosition();
    }

    private void Initialize()
    {
        SetMainTexture();
		SetMetallicSmoothnessValue();
		SetMetallicSmoothnessMapTexture();
		SetNormalMapTexture();
		SetEmissionMapTexture();
        SetMetaTexture();
        SetVelocityTexture();
        SetPositionTexture();
        SetUpDataTextures();
        SetParticleStretch();
        SetBlendMode();
		SetLightMode();
		SetZBuffer();
        UpdateVectorfieldFile();
        SetTurbulenceKeyword();
        UpdateTurbulenceTexture();
        SetLimitVelocity();
        SetCircularForce();
        UpdateColorOverLifeTime();
		SetCollisionType();
		UpdateEmitterTexture();
		UpdateMeshTargetTexture();
		UpdateMeshTargetParameters();

		if (emitterShape == EmitterShape.SkinnedMeshRenderer)
		{
			PrepareSkinnedMesh();
		}

		if (turbulenceType == TurbulenceType.VectorField)
        {
            UpdateVectorField();
            UpdateVectorFieldMatrix();
        }

		//Keywords only
		SetTextureSheetKeyword(textureSheetMode);
		UpdateMotionVectorTexture(motionVectors);
		SetRandomIndexKeyword(textureSheetRandomIndex);
		SetRotationKeyword();
        SetEmitterShapeKeyword();
        SetEmitFromShell(emitFromShell);
        SetEmitFromBase(emitFromBase);
		SetAttractorKeyword();
        SetParticleTypeKeyword(particleType);
        SetSimulationSpace();
		SetMeshTargetKeyword();
		SetShadowSettings();
		SetRefractionMapTexture();
		UpdateStretchMultiplier();
		UpdateIndexOfRefraction();

		if (collisionType == CollisionType.Depth)
		{
			UpdateCollisionCamera();
			UpdateDepthCollisionValues();
		}
	}

    private void UpdateValues()
    {
        UpdateStartSpeedAndLifetime();
        UpdateStartSizeAndRotation();
        UpdateEmitter();
        UpdateEmitterMatrix();
        UpdateSizeOverLifetime();
        UpdateRotationOverLifetime();
        UpdateOffset();
        UpdateColorIntensity();
        UpdateAmplitude();
        UpdateFrequency();
		UpdateTightness();
		UpdateOffset();
        UpdateAirResistance();
        UpdateGravity();
        UpdateCircularForceOverLifetime();
        UpdateCircularForceCenter();
		UpdateMaxVelocityOverLifetime();
		UpdateTextureSheetDimensions(rows, columns);
		UpdateCollisionPlanes();
		UpdateMotionVectorStrength();
		UpdateAspectRatio();
		UpdatePositionOffset();

		if (meshTarget != null)
		{
			UpdateMeshTargetParameters();
			UpdateMeshTargetMatrix();
		}

		if (useInheritVelocity)
		{
			UpdateInheritVelocity();
		}
		
		if (simulationSpace == GPUSimulationSpace.World)
        {
			UpdateWorldSpaceBoundingBox();
		}
		
        if (turbulenceType == TurbulenceType.VectorField)
        {
            UpdateVectorFieldMatrix();
        }

        if (gravity.mode == SimpleValueMode.Curve)
        {
            UpdateGravity();
        }

        if (airResistance.mode == SimpleValueMode.Curve)
        {
            UpdateAirResistance();
        }

		UpdateAttractors();
		UpdateForceOverLifetime();
		
		if(emitterShape == EmitterShape.SkinnedMeshRenderer)
		{
			RenderSkinnedMeshEmitterPositions();
		}

		if (collisionType == CollisionType.Depth)
		{
			UpdateCollisionMatrices();
		}

		//Debug.Log(skinnedMeshEmitterCam.cullingMask);
	}

    public void EmitNumParticles(int numParticles)
    {
		burstNum += numParticles;
    }
    #endregion

    #region ParticleMesh
    public void ForceRecreateParticles()
    {
		ClearMeshes();
		MakeMeshes();
		Initialize();
    }

    private void MakeMeshes()
    {
		ClearMeshes();
		SetRenderQueue();

		switch (particleType)
		{
			case ParticleType.Point:
				particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesPoint(bufferWidth, bufferHeight, particleMaterial);
				break;

			case ParticleType.Triangle:
				particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesTriangle(bufferWidth, bufferHeight, particleMaterial);
				break;

			case ParticleType.Mesh:
				if (meshParticle == null)
				{
					Debug.LogWarning("[GPUP] No particles created, assign a Mesh!");
				}
				else
				{
					particleMeshes = GPUParticleSystemMeshUtility.CreateMeshParticles(meshParticle, bufferWidth, bufferHeight, particleMaterial, true);
				}
				break;
			/*
			case ParticleType.AnimatedMesh:
				if (meshParticle == null)
				{
					Debug.LogWarning("[GPUP] No particles created, assign a Mesh!");
				}
				else
				{
					particleMeshes = GPUParticleSystemMeshUtility.CreateMeshParticles(meshParticle, bufferWidth, bufferHeight, particleMaterial, true);
				}
				break;
			*/
			case ParticleType.StretchedTail:
				particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesDoubleQuad(bufferWidth, bufferHeight, particleMaterial);
				break;

			default:
				particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesQuad(bufferWidth, bufferHeight, particleMaterial);
				break;
		}

		int count = particleMeshes.Length;
		particleMeshFilters = new MeshFilter[count];

		for (int i = 0; i < count; i++)
		{
			particleMeshFilters[i] = particleMeshes[i].GetComponent<MeshFilter>();
		}
    }

    private void ClearMeshes()
    {
        if (particleMeshes != null)
        {
            for (int i = 0; i < particleMeshes.Length; i++)
            {
                if (particleMeshes[i] != null)
                {
                    DestroyImmediate(particleMeshes[i]);
                }
            }
        }
		particleMeshes = null;

		if (sme != null)
		{
			DestroyImmediate(sme);
		}
	}

	public void SetShadowSettings()
	{
		if (lightMode == LightMode.Off)
		{
			for (int i = 0; i < particleMeshes.Length; i++)
			{
				MeshRenderer mr = particleMeshes[i].GetComponent<MeshRenderer>();
				mr.receiveShadows = false;
				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}
		}
		else
		{
			for (int i = 0; i < particleMeshes.Length; i++)
			{
				MeshRenderer mr = particleMeshes[i].GetComponent<MeshRenderer>();
				mr.receiveShadows = receiveShadows;
				mr.shadowCastingMode = castShadows;
			}
		}
		
	}
	#endregion

	#region MaterialManagement
	public void UpdatePositionOffset()
	{
		particleMaterial.SetVector("_PositionOffset", offset);
	}

	public void UpdateAspectRatio()
	{
		particleMaterial.SetFloat("_AspectRatio", aspectRatio);
	}

    private void UpdateTimeInMaterials()
    {
        particleMaterial.SetFloat("_CustomTime", customTime);
        particleData.UpdateTime(customTime, customDeltaTime);
    }

	public void SetMainTexture()
    {
        if (mainTexture != null)
        {
            particleMaterial.SetTexture("_MainTex", mainTexture);
            SetMainTextureKeyword(true);
        } else {
            SetMainTextureKeyword(false);
        }
    }

	public void SetMetallicSmoothnessMapTexture()
	{
		if (metallicSmoothness != null)
		{
			particleMaterial.SetTexture("_MetallicSmoothness", metallicSmoothness);
			SetMetallicSmoothnessKeyword(true);
		}
		else
		{
			SetMetallicSmoothnessKeyword(false);
		}
	}

	public void SetMetallicSmoothnessValue()
	{
		particleMaterial.SetFloat("_Metallic", metallic);
		particleMaterial.SetFloat("_Smoothness", smoothness);
	}

	public void SetNormalMapTexture()
	{
		if (normalMap != null)
		{
			particleMaterial.SetTexture("_BumpMap", normalMap);
			SetNormalKeyword(true);
		}
		else
		{
			SetNormalKeyword(false);
		}
	}

	public void SetRefractionMapTexture()
	{
		if (refractionNormals != null)
		{
			particleMaterial.SetTexture("_RefractionNormals", refractionNormals);
		}
	}

	public void SetEmissionMapTexture()
	{
		if (emissionMap != null)
		{
			particleMaterial.SetTexture("_Emission", emissionMap);
			SetEmissionKeyword(true);
		}
		else
		{
			SetEmissionKeyword(false);
		}
	}

	private void SetMetaTexture()
    {
        particleMaterial.SetTexture("_Meta", particleData.metaBuffer_1);
    }

    private void SetVelocityTexture()
    {
        particleMaterial.SetTexture("_Velocity", particleData.velocityBuffer_1);
    }

    private void SetPositionTexture()
    {
        particleMaterial.SetTexture("_Position", particleData.positionBuffer_1);
    }

    private void SetParticleStretch()
    {
        particleMaterial.SetFloat("_VelocityScale", stretchMultiplier);
    }
	#endregion

	#region ValueManagement
	public void UpdateDepthCollisionValues()
	{
		particleData.UpdateDepthCollisionParameters(depthCollisionDistance, depthCollisionDamping, dampingRandomness);
	}

	public void UpdateCollisionCamera()
	{
		if(collisionCamera != null)
		{
			if (collisionType == CollisionType.Depth)
			{
				collisionCamera.depthTextureMode = DepthTextureMode.DepthNormals;
				collisionCameraTransform = collisionCamera.transform;
			}
			else
			{
				collisionCamera.depthTextureMode = DepthTextureMode.None;
			}
		}
	}

	public void UpdateCollisionMatrices()
	{
		if (collisionCameraTransform == null || collisionCamera == null)
		{ 
			return;
		}

		MVP = GPUParticleSystemEvaluationHelper.CameraMVPMatrix(collisionCameraTransform, collisionCamera);

		Vector4 customZbufferParams = new Vector4(collisionCamera.nearClipPlane,
			collisionCamera.farClipPlane,
			0f,
			0f);

		Matrix4x4 viewMat = collisionCamera.worldToCameraMatrix;
		Matrix4x4 projMat = GL.GetGPUProjectionMatrix(collisionCamera.projectionMatrix, false);
		Matrix4x4 viewProjMat = (projMat * viewMat);
		Shader.SetGlobalMatrix("_ViewProjInv", viewProjMat.inverse);

		particleData.UpdateCollisionCamera(MVP, collisionCameraTransform.worldToLocalMatrix, collisionCamera.cameraToWorldMatrix, collisionCameraTransform.position, customZbufferParams);
	}

	public void UpdateWorldSpaceBoundingBox()
	{
		int count = particleMeshFilters.Length;

		for (int i = 0; i < count; i++)
		{
			if (particleMeshFilters[i] == null)
			{
				continue;
			}
			boundsBuffer = particleMeshFilters[i].sharedMesh.bounds;
			boundsBuffer.extents = extents;
			boundsBuffer.center = center + transform.position;
			particleMeshFilters[i].sharedMesh.bounds = boundsBuffer;
		}
	}

	public void SetupSkinnedMeshEmitterCamera()
	{
		if (skinnedMeshEmitterCam == null)
		{
			GameObject g = new GameObject("SkinnedMeshEmitterCamera");
			skinnedMeshEmitterCamTransform = g.transform;
			skinnedMeshEmitterCamTransform.position = transform.position - new Vector3(0f,0f,-10f);
			skinnedMeshEmitterCamTransform.SetParent(skinnedMeshEmitter.transform);
			skinnedMeshEmitterCamTransform.LookAt(transform.position);
			skinnedMeshEmitterCamTransform.gameObject.hideFlags = HideFlags.HideAndDontSave;
			skinnedMeshEmitterCam = g.AddComponent<Camera>();
			skinnedMeshEmitterCam.enabled = false;
			UpdateSkinnedMeshEmitterCameraLayer();
			skinnedMeshEmitterCam.clearFlags = CameraClearFlags.Color;
			skinnedMeshEmitterCam.backgroundColor = Color.black;
		}

		UpdateskinnedMeshEmitterPositionTexture();
	}

	public void DeactivateSkinnedMeshEmitter()
	{
		if (skinnedMeshEmitterCam != null)
		{
			DestroyImmediate(skinnedMeshEmitterCam.gameObject);
		}

		if (skinnedMeshEmitterPositionTexture != null)
		{
			DestroyImmediate(skinnedMeshEmitterPositionTexture);
		}

		if (skinnedMeshEmitterConvertedMesh != null)
		{
			DestroyImmediate(skinnedMeshEmitterConvertedMesh);
		}
	}

	public void UpdateSkinnedMeshEmitterCameraLayer()
	{
		if (skinnedMeshEmitterCam != null)
		{
			skinnedMeshEmitterCam.cullingMask = 1 << smeLayer;				//Nothing but...
			//skinnedMeshEmitterCam.cullingMask = ~(1 << smeLayer);			//Everything but...
		}
	}

	public void UpdateskinnedMeshEmitterPositionTexture()
	{
		if (skinnedMeshEmitterPositionTexture == null || skinnedMeshEmitterPositionTexture.width != skinnedMeshEmitterResolution || skinnedMeshEmitterPositionTexture.height != skinnedMeshEmitterResolution)
		{
			skinnedMeshEmitterPositionTexture = new RenderTexture(skinnedMeshEmitterResolution, skinnedMeshEmitterResolution, 16, precision == RenderTexturePrecision.Float ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf) ;//If render without depth -> Spams warning sometimes?!?!??!?!
			skinnedMeshEmitterPositionTexture.name = "SME Positions";
			skinnedMeshEmitterPositionTexture.useMipMap = false;
			skinnedMeshEmitterPositionTexture.filterMode = FilterMode.Point;
			skinnedMeshEmitterPositionTexture.wrapMode = TextureWrapMode.Clamp;
			skinnedMeshEmitterPositionTexture.Create();
		}

		if (skinnedMeshEmitterCam != null)
		{
			skinnedMeshEmitterCam.targetTexture = skinnedMeshEmitterPositionTexture;
		}
	}

	public void RenderSkinnedMeshEmitterPositions()
	{
		if (skinnedMeshEmitterSME == null)
		{
			return;
		}

		if (skinnedMeshEmitterCam == null)
		{
			SetupSkinnedMeshEmitterCamera();
		}
		
		skinnedMeshEmitterSME.enabled = true;
		skinnedMeshEmitterCam.Render();
		skinnedMeshEmitterSME.enabled = false;
	}

	public void PrepareSkinnedMesh()
	{
		if (skinnedMeshEmitter == null)
		{
			return;
		}

		SetupSkinnedMeshEmitterCamera();

		skinnedMeshEmitterConvertedMesh = GPUParticleSystemMeshUtility.ProcessSkinnedMesh(skinnedMeshEmitter.sharedMesh);
		particleData.UpdateSkinnedMeshEmitterTexture(skinnedMeshEmitterPositionTexture);

		//if (skinnedMeshEmitterConvertedMesh == null)
		//{
		//	skinnedMeshEmitterConvertedMesh = GPUParticleSystemMeshUtility.ProcessSkinnedMesh(skinnedMeshEmitter.sharedMesh);
		//}

		if (sme != null)
		{
			DestroyImmediate(sme);
		}
		
		sme = Instantiate(skinnedMeshEmitter.gameObject);
		skinnedMeshEmitterTransform = sme.transform;
		sme.name = "SME";
		skinnedMeshEmitterSME = sme.GetComponent<SkinnedMeshRenderer>();
		skinnedMeshEmitterSME.sharedMesh = skinnedMeshEmitterConvertedMesh;
		skinnedMeshEmitterSME.rootBone = skinnedMeshEmitter.rootBone;
		sme.hideFlags = HideFlags.DontSaveInEditor;
		sme.hideFlags = HideFlags.HideAndDontSave;
		sme.layer = smeLayer;

		if (skinnedMeshEmitterMaterial == null)
		{
			skinnedMeshEmitterMaterial = new Material(Shader.Find("Hidden/SkinnedMeshPosition"));
		}

		int matCount = skinnedMeshEmitterSME.sharedMaterials.Length;
		Material[] materials = new Material[matCount];

		for (int i = 0; i < matCount; i++)
		{
			materials[i] = skinnedMeshEmitterMaterial;
		}

		skinnedMeshEmitterSME.sharedMaterials = materials;
		skinnedMeshEmitterSME.enabled = false;
	}

	public void UpdateMeshTargetTexture()
	{
		if (!useMeshFilter)
		{
			if (meshTarget == null)
			{
				return;
			}

			meshTargetPositionTexture = GPUParticleSystemMeshUtility.MeshToTexture(meshTarget, targetBakeType, meshTargetResolution);
			particleData.UpdateMeshTargetTexture(meshTargetPositionTexture);
		}
		else
		{
			if (meshFilterTarget == null)
			{
				//Debug.Log("Please assign a mesh filter.");
				return;
			}

			meshTarget = meshFilterTarget.sharedMesh;

			if (meshTarget == null)
			{
				//Debug.Log("Mesh filter does not have a mesh.");
				return;
			}

			meshTargetPositionTexture = GPUParticleSystemMeshUtility.MeshToTexture(meshTarget, targetBakeType, meshTargetResolution);
			//meshTargetPositionTexture.name = "Mesh target positions";
			particleData.UpdateMeshTargetTexture(meshTargetPositionTexture);
		}
	}

	public void UpdateMeshTargetParameters()
	{
		particleData.UpdateMeshTargetValues(meshTargetStrength, onTarget, progress);
	}

	public void UpdateMeshTargetMatrix()
	{
		if (useMeshFilter)
		{
			if (meshFilterTarget != null)
			{
				particleData.UpdateMeshTargetMatrix(meshFilterTarget.transform.localToWorldMatrix);
			}
		}
		else
		{
			particleData.UpdateMeshTargetMatrix(transform.localToWorldMatrix);
		}
	}

	public void UpdateCollisionPlanes()
	{
		for (int i = 0; i < planes.Length; i++)
		{
			if (planes[i] != null)
			{
				planePositions[i] = new Vector4(planes[i].position.x, planes[i].position.y, planes[i].position.z, collisionDamping[i]);
				planeNormals[i] = new Vector4(planes[i].up.x, planes[i].up.y, planes[i].up.z, 1f);
			}
			else
			{
				planePositions[i] = new Vector4(0f, -Mathf.Infinity, 0f, 1f);
				planeNormals[i] = new Vector4(0f, 1f, 0f, 0f);
			}
		}
		particleData.UpdateCollisionPlanes(planePositions, planeNormals);
	}

	public void UpdateParticleMainTex(Texture2D mainTexture)
    {
        if (mainTexture != null)
        {
            this.mainTexture = mainTexture;
            particleMaterial.SetTexture("_MainTex", mainTexture);
            SetMainTextureKeyword(true);
            return;
        }

        if (this.mainTexture != null)
        {
            particleMaterial.SetTexture("_MainTex", this.mainTexture);
            SetMainTextureKeyword(true);
            return;
        }

        SetMainTextureKeyword(false);
    }

	public void UpdateMotionVectorStrength()
	{
		particleMaterial.SetFloat("_MotionVectorStrength", motionVectorStrength);
	}

	public void UpdateMotionVectorTexture(Texture2D motionVectors)
	{
		if (motionVectors != null)
		{
			this.motionVectors = motionVectors;
			particleMaterial.SetTexture("_MotionVectors", motionVectors);
			return;
		}

		if (this.motionVectors != null)
		{
			particleMaterial.SetTexture("_MotionVectors", this.motionVectors);
			return;
		}
	}

    public void UpdateStartSpeedAndLifetime()
    {
        particleData.UpdateStartLifetimeSpeed(startLifetime, startSpeed, progress);
    }

    public void UpdateStartSizeAndRotation()
    {
        particleData.UpdateStartSizeRotation(startSize, startRotation, progress);
    }

    public void UpdateEmitterParameters()
    {
        param4 = Mathf.Tan(param2 * Mathf.Deg2Rad) * param3;
        Vector4 emitterParameters = new Vector4(param1, param2, param3, param1 + param4);

        if (simulationSpace == GPUSimulationSpace.Local)
        {
            particleData.UpdateEmitterParameters(transform.position, emitterParameters);
        } else {
            particleData.UpdateEmitterParameters(transform.position, Vector3.zero);
        }
    }

    public void UpdateEmitterMatrix()
    {
        switch (emitterShape)
        {
            case EmitterShape.MeshFilter:
                if (meshFilterEmitter != null)
                {
                    particleData.UpdateEmitterMatrix(meshFilterEmitter.transform.localToWorldMatrix);
                }
                break;
            case EmitterShape.SkinnedMeshRenderer:
                if (skinnedMeshEmitterTransform != null)
                {
                    particleData.UpdateEmitterMatrix(skinnedMeshEmitterTransform.localToWorldMatrix);
                }
                break;
            default:
                particleData.UpdateEmitterMatrix(transform.localToWorldMatrix);
                break;
        }
    }

    public void UpdateParticleColorIntensity()
    {
        particleMaterial.SetFloat("_ColorIntensity", colorOverLifetime.intensity);
    }

    public void UpdateStretchMultiplier()
    {
        particleMaterial.SetFloat("_VelocityScale", stretchMultiplier);
        particleMaterial.SetVector("_MinMaxStretch", minMaxStretch);
    }

    public void UpdateColorOverLifeTime()
    {
        if (colorTexture == null)
        {
            SetUpDataTextures();
        }

        switch (colorOverLifetime.mode)
        {
            case ColorMode.Color:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.color1, colorTexture);
                break;

            case ColorMode.RandomTwoColors:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.color1, colorOverLifetime.color2, colorTexture);
                break;

            case ColorMode.Gradient:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.gradient1, colorTexture);
                break;

            case ColorMode.RandomTwoGradients:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.gradient1, colorOverLifetime.gradient2, colorTexture);
                break;
        }

        particleMaterial.SetTexture("_ColorOverLifetime", colorTexture);
    }

    public void UpdateColorIntensity()
    {
        particleMaterial.SetFloat("_ColorIntensity", GPUParticleSystemEvaluationHelper.EvaluateSingleFloatCurveBundle(colorIntensityOverLifetime, progress));
    }

    public void UpdateMaxVelocityOverLifetime()
    {
        particleData.UpdateMaxVelocityOverLifetime(maxVelocity, progress);
    }

    public void UpdateCircularForceOverLifetime()
    {
        particleData.UpdateCircularForceOverLifetime(circularForce, progress);
    }

    private void UpdateCircularForceCenter()
    {
        if (!useCircularForce)
            return;

        if (circularForceCenter == null)
        {
            GameObject g = new GameObject("Center of circular force");
			circularForceCenter = g.transform;
        }

        particleData.UpdateCircularForceCenterPosition(circularForceCenter.position + safetyVector);
    }

    public void UpdateSizeOverLifetime()
    {
        SetSizeOverLifetimeKeyword(sizeOverLifetime.mode);

        switch (sizeOverLifetime.mode)
        {
            case CurveMode.Off:
                break;

            case CurveMode.Linear:
                sizeOverLifetime.skew = Mathf.Clamp(sizeOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_SizeMultiplier", sizeOverLifetime.multiplier);
                particleMaterial.SetFloat("_SizeOverLifetimeSkew", sizeOverLifetime.skew);

                if (sizeOverLifetime.invert)
                {
                    particleMaterial.SetFloat("_Invert", 1f);
                }
                else {
                    particleMaterial.SetFloat("_Invert", 0f);
                }
                break;

            case CurveMode.Smooth:
                sizeOverLifetime.skew = Mathf.Clamp(sizeOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_SizeMultiplier", sizeOverLifetime.multiplier);
                particleMaterial.SetFloat("_SizeOverLifetimeSkew", sizeOverLifetime.skew);

                if (sizeOverLifetime.invert)
                {
                    particleMaterial.SetFloat("_Invert", 1f);
                }
                else
                {
                    particleMaterial.SetFloat("_Invert", 0f);
                }
                break;

            case CurveMode.Curve:
                sizeOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(sizeOverLifetime.curve1);
                particleMaterial.SetVectorArray("_SizeOverLifetimeBezierC1", sizeOverLifetime.bezier1);
                particleMaterial.SetInt("_SOLNumSegments", sizeOverLifetime.curve1.keys.Length - 1);
                break;

            case CurveMode.RandomTwoCurves:
                sizeOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(sizeOverLifetime.curve1);
                sizeOverLifetime.bezier2 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(sizeOverLifetime.curve2);
                particleMaterial.SetVectorArray("_SizeOverLifetimeBezierC1", sizeOverLifetime.bezier1);
                particleMaterial.SetVectorArray("_SizeOverLifetimeBezierC2", sizeOverLifetime.bezier2);
                particleMaterial.SetInt("_SOLNumSegments", sizeOverLifetime.curve1.keys.Length - 1);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    public void UpdateRotationOverLifetime()
    {
        SetRotationOverLifetimeKeyword(rotationOverLifetime.mode);

        switch (rotationOverLifetime.mode)
        {
            case CurveMode.Off:
                break;

            case CurveMode.Linear:
                rotationOverLifetime.skew = Mathf.Clamp(rotationOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_RotationMultiplier", rotationOverLifetime.multiplier);
                particleMaterial.SetFloat("_RotationOverLifetimeSkew", rotationOverLifetime.skew);
                break;

            case CurveMode.Smooth:
                rotationOverLifetime.skew = Mathf.Clamp(rotationOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_RotationMultiplier", rotationOverLifetime.multiplier);
                particleMaterial.SetFloat("_RotationOverLifetimeSkew", rotationOverLifetime.skew);
                break;

            case CurveMode.Curve:
                rotationOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(rotationOverLifetime.curve1);
                particleMaterial.SetVectorArray("_RotationOverLifetimeBezierC1", rotationOverLifetime.bezier1);
                particleMaterial.SetInt("_ROLNumSegments", rotationOverLifetime.curve1.keys.Length - 1);
                break;

            case CurveMode.RandomTwoCurves:
                rotationOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(rotationOverLifetime.curve1);
                rotationOverLifetime.bezier2 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(rotationOverLifetime.curve2);
                particleMaterial.SetVectorArray("_RotationOverLifetimeBezierC1", rotationOverLifetime.bezier1);
                particleMaterial.SetVectorArray("_RotationOverLifetimeBezierC2", rotationOverLifetime.bezier2);
                particleMaterial.SetInt("_ROLNumSegments", rotationOverLifetime.curve1.keys.Length - 1);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    public void UpdateGravity()
    {
        switch (gravity.mode)
        {
            case SimpleValueMode.Value:
                particleData.UpdateGravity(gravity.value);
                break;

            case SimpleValueMode.Curve:
                particleData.UpdateGravity(gravity.curve.Evaluate(progress));
                break;
        }
    }

    public void UpdateAirResistance()
    {
        particleData.UpdateAirResistance(GPUParticleSystemEvaluationHelper.EvaluateSingleFloatCurveBundle(airResistance, progress));
    }

    public void UpdateTurbulenceTexture()
    {
        particleData.UpdateTurbulenceTexture(vectorNoise);
    }

    public void UpdateVectorField()
    {
        particleData.UpdateVectorField(vectorField);
    }

    public void UpdateAmplitude()
    {
        particleData.UpdateAmplitude(turbulenceAmplitude, progress);
    }

    public void UpdateFrequency()
    {
        particleData.UpdateFrequency(turbulenceFrequency, progress);
    }

	public void UpdateTightness()
	{
		particleData.UpdateTightness(Tightness);
	}

	public void UpdateOffset()
    {
        particleData.UpdateOffset(turbulenceOffset, progress);
    }

    public void UpdateVectorfieldFile()
    {
        if (fgaFile != null)
            vectorField = GPUParticleSystemEvaluationHelper.DeserializeVectorField(fgaFile);
    }

    public void UpdateVectorFieldMatrix()
    {
        if (vectorFieldObject == null)
        {
            GameObject g = new GameObject("Vector field transform");
            vectorFieldObject = g.transform;
        }

        if (vectorFieldObject != null)
        {
			if (simulationSpace == GPUSimulationSpace.Local)
			{
				vectorFieldObject.position = transform.position;
			}
			else {
				vectorFieldObject.position = Vector3.zero;
			}
            particleData.UpdateVectorFieldMatrix(vectorFieldObject, turbulenceRotation, turbulenceFrequency, progress, customDeltaTime);
        }
    }

	public void UpdateEmitterTexture()
	{
		switch (emitterShape)
		{
			case EmitterShape.Mesh:
				if (meshEmitter == null)
				{
					Debug.Log("Please assign a mesh.");
					return;
				}
				
				GPUParticleSystemMeshUtility.MeshToPositionNormals(out meshEmitterPositionTexture, out meshEmitterNormalTexture, meshEmitter, meshBakeType, meshEmitterResolution);
				particleData.UpdateMeshEmitterTexture(meshEmitterPositionTexture, meshEmitterNormalTexture);
				break;

			case EmitterShape.MeshFilter:
				if (meshFilterEmitter == null)
				{
					Debug.Log("Please assign a mesh filter.");
					return;
				}

				meshEmitter = meshFilterEmitter.sharedMesh;

				if (meshEmitter == null)
				{
					Debug.Log("Mesh filter does not have a mesh.");
					return;
				}
				GPUParticleSystemMeshUtility.MeshToPositionNormals(out meshEmitterPositionTexture, out meshEmitterNormalTexture, meshEmitter, meshBakeType, meshEmitterResolution);
				particleData.UpdateMeshEmitterTexture(meshEmitterPositionTexture, meshEmitterNormalTexture);
				break;

				case EmitterShape.SkinnedMeshRenderer:
				UpdateskinnedMeshEmitterPositionTexture();
				particleData.UpdateSkinnedMeshEmitterTexture(skinnedMeshEmitterPositionTexture);
				break;
		}
	}

	private void UpdateInheritVelocity()
	{
		emitterVelocity = Vector3.Normalize(transform.position - previousEmitterPosition) * GPUParticleSystemEvaluationHelper.EvaluateSingleFloatCurveBundle(inheritVelocityMultiplyer, progress) * customDeltaTime;
		previousEmitterPosition = transform.position;

		particleData.UpdateInheritVelocity(emitterVelocity);
	}

	private void UpdateAttractors()
	{
		if (attractors.Count > 0)
			particleData.UpdateAttractors(attractors, transform.position);
	}

	public void UpdateForceOverLifetime()
	{
		particleData.UpdateForceOverLifetime(forceOverLifetime, progress);
	}

	public void UpdateTextureSheetDimensions(int rows, int columns)
	{
		particleMaterial.SetInt("_Rows", rows);
		particleMaterial.SetInt("_Columns", columns);
	}

	public void UpdateIndexOfRefraction()
	{
		particleMaterial.SetFloat("_IndexOfRefraction", indexOfRefraction);
	}
	#endregion

	#region KeyWordManagement
	public void SetMeshTargetKeyword()
	{
		if (!useMeshTarget)
		{
			particleData.MeshTarget(false);
		}
		else
		{
			particleData.MeshTarget(true);
		}
	}

	public void SetCollisionType()
	{
		particleData.SetCollisionType(collisionType);
	}

	public void SetMainTextureKeyword(bool active)
    {
        if (active)
        {
            particleMaterial.EnableKeyword("MAINTEX");
        }
        else {
            particleMaterial.DisableKeyword("MAINTEX");
        }
    }

	public void SetMetallicSmoothnessKeyword(bool active)
	{
		if (active)
		{
			particleMaterial.EnableKeyword("METALLIC_SMOOTHNESS");
		}
		else
		{
			particleMaterial.DisableKeyword("METALLIC_SMOOTHNESS");
		}
	}

	public void SetNormalKeyword(bool active)
	{
		if (active)
		{
			particleMaterial.EnableKeyword("NORMAL_MAP");
		}
		else
		{
			particleMaterial.DisableKeyword("NORMAL_MAP");
		}
	}

	public void SetEmissionKeyword(bool active)
	{
		if (active)
		{
			particleMaterial.EnableKeyword("EMISSION_MAP");
		}
		else
		{
			particleMaterial.DisableKeyword("EMISSION_MAP");
		}
	}

	public void SetTurbulenceKeyword()
    {
        if (turbulenceType != TurbulenceType.VectorField)
        {
            if (vectorFieldObject != null)
            {
                DestroyImmediate(vectorFieldObject.gameObject);
            }
        }
        else
        {
            if (vectorFieldObject == null)
            {
                GameObject g = new GameObject("Vector field transform");
                vectorFieldObject = g.transform;
            }
        }
        particleData.Turbulence(turbulenceType);
    }

    public void SetRotationKeyword()
    {
        if (useRotation)
        {
            particleMaterial.EnableKeyword("ROTATION");
        }
        else
        {
            particleMaterial.DisableKeyword("ROTATION");
        }
    }

    public void SetRotationOverLifetimeKeyword(bool active)
    {
        if (active)
        {
            particleMaterial.EnableKeyword("ROTATION");
        }
        else
        {
            particleMaterial.DisableKeyword("ROTATION");
        }
    }

    public void SetParticleTypeKeyword(ParticleType type)
    {
        particleMaterial.DisableKeyword("POINT");
        particleMaterial.DisableKeyword("TRIANGLE");
        particleMaterial.DisableKeyword("BILLBOARD");
        particleMaterial.DisableKeyword("H_BILLBORD");
        particleMaterial.DisableKeyword("V_BILLBOARD");
        particleMaterial.DisableKeyword("TS_BILLBOARD");
        particleMaterial.DisableKeyword("S_BILLBOARD");
        particleMaterial.DisableKeyword("MESH");
        particleMaterial.DisableKeyword("ANIMATED_MESH");

        switch (type)
        {
            case ParticleType.Point:
                particleMaterial.EnableKeyword("POINT");
                break;

            case ParticleType.Triangle:
                particleMaterial.EnableKeyword("TRIANGLE");
                break;

            case ParticleType.Billboard:
                particleMaterial.EnableKeyword("BILLBOARD");
                break;

            case ParticleType.HorizontalBillboard:
                particleMaterial.EnableKeyword("H_BILLBORD");
                break;

            case ParticleType.VerticalBillboard:
                particleMaterial.EnableKeyword("V_BILLBOARD");
                break;

            case ParticleType.StretchedTail:
                particleMaterial.EnableKeyword("TS_BILLBOARD");
                break;

            case ParticleType.StretchedBillboard:
                particleMaterial.EnableKeyword("S_BILLBOARD");
                break;
				
            case ParticleType.Mesh:
                particleMaterial.EnableKeyword("MESH");
                break;

			//case ParticleType.AnimatedMesh:
			//    particleMaterial.EnableKeyword("ANIMATED_MESH");
			//    break;
		}
	}

    public void SetTextureSheetKeyword(TextureSheetMode mode)
    {
        switch (mode)
        {
            case TextureSheetMode.Off:
                particleMaterial.DisableKeyword("TEXTURESHEET");
				particleMaterial.DisableKeyword("TEXTURESHEET_BLENDED");
				particleMaterial.DisableKeyword("TEXTURESHEET_MOTIONVECTORS");
                break;

			case TextureSheetMode.TextureSheet:
				particleMaterial.DisableKeyword("TEXTURESHEET_BLENDED");
				particleMaterial.DisableKeyword("TEXTURESHEET_MOTIONVECTORS");
				particleMaterial.EnableKeyword("TEXTURESHEET");
				break;

			case TextureSheetMode.TextureSheetBlended:
				particleMaterial.DisableKeyword("TEXTURESHEET");
				particleMaterial.DisableKeyword("TEXTURESHEET_MOTIONVECTORS");
				particleMaterial.EnableKeyword("TEXTURESHEET_BLENDED");
				break;

			case TextureSheetMode.TextureSheetMotionVectors:
				particleMaterial.DisableKeyword("TEXTURESHEET");
				particleMaterial.DisableKeyword("TEXTURESHEET_BLENDED");
				particleMaterial.EnableKeyword("TEXTURESHEET_MOTIONVECTORS");
                break;
        }
    }

    public void SetRandomIndexKeyword(bool active)
    {
        if (active)
        {
            particleMaterial.EnableKeyword("RANDOMINDEX");
        }
        else
        {
            particleMaterial.DisableKeyword("RANDOMINDEX");
        }
    }

    public void SetSizeOverLifetimeKeyword(CurveMode mode)
    {
        particleMaterial.DisableKeyword("LINEAR_SIZE");
        particleMaterial.DisableKeyword("SMOOTH_SIZE");
        particleMaterial.DisableKeyword("CURVE_SIZE");
        particleMaterial.DisableKeyword("RANDOM2CURVES_SIZE");

        switch (mode)
        {
            case CurveMode.Linear:
                particleMaterial.EnableKeyword("LINEAR_SIZE");
                break;

            case CurveMode.Smooth:
                particleMaterial.EnableKeyword("SMOOTH_SIZE");
                break;

            case CurveMode.Curve:
                particleMaterial.EnableKeyword("CURVE_SIZE");
                break;

            case CurveMode.RandomTwoCurves:
                particleMaterial.EnableKeyword("RANDOM2CURVES_SIZE");
                break;
        }
    }

    public void SetRotationOverLifetimeKeyword(CurveMode mode)
    {
        particleMaterial.DisableKeyword("LINEAR_ROTATION");
        particleMaterial.DisableKeyword("SMOOTH_ROTATION");
        particleMaterial.DisableKeyword("CURVE_ROTATION");
        particleMaterial.DisableKeyword("RANDOM2CURVES_ROTATION");
        switch (mode)
        {
            case CurveMode.Linear:
                particleMaterial.EnableKeyword("LINEAR_ROTATION");
                break;

            case CurveMode.Smooth:
                particleMaterial.EnableKeyword("SMOOTH_ROTATION");
                break;

            case CurveMode.Curve:
                particleMaterial.EnableKeyword("CURVE_ROTATION");
                break;

            case CurveMode.RandomTwoCurves:
                particleMaterial.EnableKeyword("RANDOM2CURVES_ROTATION");
                break;
        }
    }

    public void SetSimulationSpace()
    {
        particleData.SimSpace(simulationSpace);

		if (simulationSpace == GPUSimulationSpace.Local)
		{
			if (particleMeshes == null)
			{
				return;
			}

			foreach (GameObject g in particleMeshes)
            {
                if (g != null)
                {
                    g.transform.parent = transform;
                    g.transform.localPosition = Vector3.zero;
                    g.transform.localRotation = Quaternion.identity;
                    g.transform.localScale = Vector3.one;

					int count = particleMeshFilters.Length;

					for (int i = 0; i < count; i++)
					{
						boundsBuffer.extents = extents;
						boundsBuffer.center = center;
						particleMeshFilters[i].sharedMesh.bounds = boundsBuffer;
					}
                }
            }
        }
        else
        {
			if (particleMeshes == null)
			{
				return;
			}

			foreach (GameObject g in particleMeshes)
            {
                if (g != null)
                {
                    g.transform.parent = null;
                    g.transform.position = Vector3.zero;
                    g.transform.localRotation = Quaternion.identity;
                    g.transform.localScale = Vector3.one;
                }
            }
        }
    }

    public void SetEmitterShapeKeyword()
    {
		if (emitterShape == EmitterShape.Texture)
		{
			particleMaterial.EnableKeyword("TEXTUREEMITTER");
			UpdateStartSize();
		}
		else
		{
			particleMaterial.DisableKeyword("TEXTUREEMITTER");
		}

		particleData.EmitterShape(emitterShape);
    }

	public void UpdateStartSize()
	{
		startSize.value1 = (param1 / bufferWidth) * 2f;
		startSize.value2 = (param1 / bufferHeight) * 2f;
	}

	public void SetLimitVelocity()
    {
        particleData.LimitVelocity(useMaxVelocity);
    }

    public void SetCircularForce()
    {
        particleData.CircularForce(useCircularForce);
    }

	public void SetLightMode()
	{
		if (particleMaterial == null)
		{
			switch (lightMode)
			{
				case LightMode.Off:
					particleMaterial = new Material(Shader.Find("GPUParticles/GPUParticles"));
					break;

				case LightMode.Standard:
					particleMaterial = new Material(Shader.Find("GPUParticles/ParticleStandard"));
					break;

				case LightMode.Refraction:
					particleMaterial = new Material(Shader.Find("GPUParticles/GPUParticlesRefraction"));
					break;
			}
		}
		else
		{
			switch (lightMode)
			{
				case LightMode.Off:
					particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
					break;

				case LightMode.Standard:
					particleMaterial.shader = Shader.Find("GPUParticles/ParticleStandard");
					break;

				case LightMode.Refraction:
					particleMaterial.shader = Shader.Find("GPUParticles/GPUParticlesRefraction");
					break;
			}
		}
		
	}

    public void SetBlendMode()
    {
        switch (blendMode)
        {
            case GPUParticleBlendMode.Alpha:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Additive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Screen:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Premultiplied:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Subtractive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Multiply:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Opaque:
				particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
				particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.Zero);
				break;

            case GPUParticleBlendMode.CutOff:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionStandard");
                break;
        }
    }

    public void SetBlendMode(GPUParticleBlendMode blendMode)
    {
        this.blendMode = blendMode;

        switch (blendMode)
        {
            case GPUParticleBlendMode.Alpha:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Additive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Screen:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Premultiplied:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Subtractive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Multiply:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Opaque:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionOpaque");
                break;

            case GPUParticleBlendMode.CutOff:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionStandard");
                break;
        }
    }

    public void SetZBuffer()
    {
        if (useZbuffer)
        {
            particleMaterial.SetInt("_ZWrite", 1);
        }
        else
        {
            particleMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void SetZBuffer(bool active)
    {
        if (active)
        {
            particleMaterial.SetInt("_ZWrite", 1);
        }
        else
        {
            particleMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void SetEmitFromShell(bool active)
    {
        particleData.EmitFromShell(active);
    }

    public void SetEmitFromBase(bool active)
    {
        particleData.EmitFromBase(active);
    }

	public void SetAttractorKeyword()
	{
		if (attractors.Count > 0)
		{
			particleData.Attractors(true);
		}
		else {
			particleData.Attractors(false);
		}
	}
    #endregion

    #region TextureManagment
    public void SetUpDataTextures()
    {
        colorTexture = new Texture2D(particleColorPrecision, particleColorPrecision, TextureFormat.RGBAHalf, false);
        colorTexture.Apply(false);
    }
	#endregion

}
