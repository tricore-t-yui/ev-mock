using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public partial class GPUParticleSystem : MonoBehaviour
{
    public void OnEnable()
    {
#if UNITY_EDITOR
		CheckInstanceID();

		if (!Application.isPlaying)
        {
            EditorApplication.update += Update;
        }
#endif
		if (!shaderVariantCollection)
		{
			shaderVariantCollection = Resources.Load("Shaders/ShaderVariants") as ShaderVariantCollection;
		}

		if (shaderVariantCollection)
		{
			if (!shaderVariantCollection.isWarmedUp)
			{
				shaderVariantCollection.WarmUp();
			}
		}

		PrepareParticleData();
		MakeMeshes();
		Initialize();
		SetRenderQueue();

#if UNITY_EDITOR
		CollectShaderVariants();
#endif

		if (playOnAwake)
		{
			Play();
		}

		Shader.SetGlobalTexture("_GlobalPositions", particleData.positionBuffer_1);
	}

    private void OnDisable()
    {
		Stop();
		ClearParticleData();
		ClearMeshes();

#if UNITY_EDITOR
		if (!Application.isPlaying)
        {
            EditorApplication.update -= Update;
        }
#endif
    }

	private void OnDestroy()
	{
		if (skinnedMeshEmitterCam != null)
		{
			DestroyImmediate(skinnedMeshEmitterCam.gameObject);
		}
	}

#if UNITY_EDITOR
	private void CheckInstanceID()
	{
		if (customInstanceID == -1)
		{
			SetLightMode();
			SetRenderQueue();
			customInstanceID = this.GetInstanceID();
		}
		
		if (this.GetInstanceID() != customInstanceID)
		{
			particleMaterial = null;
			SetLightMode();
			SetRenderQueue();
			customInstanceID = this.GetInstanceID();
		}
	}
#endif

	private void Awake()
    {
		ReSeed();

        if (playOnAwake)
        {
            Play();
        }
    }
	
    private void Update()
    {
		UpdateCustomTime();
		UpdateProgress();
		UpdateTimeInMaterials();

		if (state == GPUParticleSystemState.Playing)
		{
			InternalEvaluateBurst();
			InternalEmit();
			InternalStep();
		}

#if UNITY_EDITOR
		if (CollisionTab)
		{
			DrawPlanes();
		}
#endif
	}

	private void UpdateCustomTime()
    {
		if (useFixedDeltaTime)
		{
			customDeltaTime = fixedDeltaTime;
		}
		else
		{
			customDeltaTime = (Time.realtimeSinceStartup - previousFrameTime) * timeScale;
			previousFrameTime = Time.realtimeSinceStartup;
		}

		if (state == GPUParticleSystemState.Playing)
		{
			customTime += customDeltaTime * timeScale;
		}
	}

	private void UpdateProgress()
	{
		if (state == GPUParticleSystemState.Playing)
		{
			progress = (customTime - effectStartTime) / effectLength;
			progress = Mathf.Clamp01(progress);

			if (progress >= 1.0f)
			{
				if (loop)
				{
					currentBursts = new List<GPUParticleSystemBurst>(bursts);
					effectStartTime = customTime;
					progress = 0.0f;
				}
				else
				{
					currentBursts = new List<GPUParticleSystemBurst>(bursts);
					state = GPUParticleSystemState.Stopped;
				}
			}

			
		}
	}

	public void SetRenderQueue()
	{
		particleMaterial.renderQueue = renderQueue;
	}

	#region API
	public void Play()
    {
		if (state == GPUParticleSystemState.Paused)
		{
			state = GPUParticleSystemState.Playing;
		}

		if (state == GPUParticleSystemState.Stopped || state == GPUParticleSystemState.Uninitialized)
		{
			currentBursts = new List<GPUParticleSystemBurst>(bursts);
			effectStartTime = customTime;
			progress = 0.0f;
			state = GPUParticleSystemState.Playing;
		}
	}

	public void Pause()
	{
		state = GPUParticleSystemState.Paused;
	}

	public void Stop()
	{
		progress = 0f;
		state = GPUParticleSystemState.Stopped;

		if (particleData != null)
		{
			particleData.Reset();
		}

		startID = 0f;
		endID = 0f;
	}

	public void Restart()
    {
        Stop();
        Play();
    }

    public void Emit(int numParticles)
    {
        EmitNumParticles(numParticles);
    }

    public void UpdateEmitter()
    {
        Vector4 emitterPosition = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1);

        if (simulationSpace == GPUParticleSystem.GPUSimulationSpace.Local)
        {
            emitterPosition =  Vector3.zero;
        }

        param4 = Mathf.Tan(param2 * Mathf.Deg2Rad) * param3;
        Vector4 emitterParameters = new Vector4(param1, param2, param3, param1 + param4);

        particleData.UpdateEmitterParameters(emitterPosition, emitterParameters);
    }

    public void ReSeed()
    {
        emissionRate.seed = Random.Range(0f,1f);
        startSpeed.seed = Random.Range(0f, 1f);
        startLifetime.seed = Random.Range(0f, 1f);
        startSize.seed = Random.Range(0f, 1f);
        startRotation.seed = Random.Range(0f, 1f);
        sizeOverLifetime.seed = Random.Range(0f, 1f);
        rotationOverLifetime.seed = Random.Range(0f, 1f);
        colorOverLifetime.seed = Random.Range(0f, 1f);
    }

	public void AddBurst(int min, int max, float time, float probability, bool playOnce)
	{
		GPUParticleSystemBurst b = new GPUParticleSystemBurst();
		b.minBurst = min;
		b.maxBurst = max;
		b.burstTime = time;
		b.burstProbability = probability;
		b.playOnce = playOnce;
		bursts.Add(b);
	}

	public void AddBurst()
	{
		GPUParticleSystemBurst b = new GPUParticleSystemBurst();
		bursts.Add(b);
	}

	public void AddAttractor()
	{
		if (attractors.Count >= 4)
			return;

		Attractor a = new Attractor(null, 1f, 1f);
		attractors.Add(a);
	}

	public void RemoveAttractor(int index)
	{
		attractors.RemoveAt(index);
	}

	public void RemoveLastAttractor()
	{
		if(attractors.Count > 0)
			attractors.RemoveAt(attractors.Count-1);
	}

	public void RemoveBurst(int index)
	{
		if (index < bursts.Count)
		{
			bursts.RemoveAt(index);
		}
	}

	public void RemoveLastBurst()
	{
		bursts.RemoveAt(bursts.Count - 1);
	}
	#endregion

#if UNITY_EDITOR
	public void CollectShaderVariants()
	{
		ShaderVariantCollection.ShaderVariant velocity = new ShaderVariantCollection.ShaderVariant();
		velocity.shader = Shader.Find("GPUParticles/Internal/Velocity");
		velocity.passType = UnityEngine.Rendering.PassType.Normal;
		velocity.keywords = particleData.GetVelocityKeywords();

		if (!shaderVariantCollection.Contains(velocity))
		{
			shaderVariantCollection.Add(velocity);
		}

		ShaderVariantCollection.ShaderVariant position = new ShaderVariantCollection.ShaderVariant();
		position.shader = Shader.Find("GPUParticles/Internal/Position");
		position.passType = UnityEngine.Rendering.PassType.Normal;
		position.keywords = particleData.GetPositionKeywords();

		if (!shaderVariantCollection.Contains(position))
		{
			shaderVariantCollection.Add(position);
		}

		ShaderVariantCollection.ShaderVariant particle = new ShaderVariantCollection.ShaderVariant();
		particle.shader = Shader.Find("GPUParticles/GPUParticles");
		particle.passType = UnityEngine.Rendering.PassType.Normal;
		particle.keywords = particleMaterial.shaderKeywords;

		if (!shaderVariantCollection.Contains(particle))
		{
			shaderVariantCollection.Add(particle);
		}
		/*
		ShaderVariantCollection.ShaderVariant litParticle = new ShaderVariantCollection.ShaderVariant();
		litParticle.shader = Shader.Find("GPUParticles/ParticleStandard");
		litParticle.passType = UnityEngine.Rendering.PassType.Normal;
		litParticle.keywords = particleMaterial.shaderKeywords;

		if (!shaderVariantCollection.Contains(litParticle))
		{
			shaderVariantCollection.Add(litParticle);
		}
		*/
	}

	private void CreateDebugQuads()
	{
		GameObject g = GameObject.CreatePrimitive(PrimitiveType.Quad);
		g.name = "New ptk";
		g.transform.position = Vector3.zero;
		Material m = new Material(Shader.Find("Unlit/Texture"));
		m.SetTexture("_MainTex", particleData.newParticleBuffer);
		g.GetComponent<MeshRenderer>().material = m;

		g = GameObject.CreatePrimitive(PrimitiveType.Quad);
		g.name = "Pos";
		g.transform.position = new Vector3(1f, 0f, 0f);
		m = new Material(Shader.Find("Unlit/Texture"));
		m.SetTexture("_MainTex", particleData.positionBuffer_1);
		g.GetComponent<MeshRenderer>().material = m;

		g = GameObject.CreatePrimitive(PrimitiveType.Quad);
		g.name = "Vel";
		g.transform.position = new Vector3(2f, 0f, 0f);
		m = new Material(Shader.Find("Unlit/Texture"));
		m.SetTexture("_MainTex", particleData.velocityBuffer_1);
		g.GetComponent<MeshRenderer>().material = m;

		g = GameObject.CreatePrimitive(PrimitiveType.Quad);
		g.name = "Meta";
		g.transform.position = new Vector3(3f, 0f, 0f);
		m = new Material(Shader.Find("Unlit/Texture"));
		m.SetTexture("_MainTex", particleData.metaBuffer_1);
		g.GetComponent<MeshRenderer>().material = m;
	}

	private void DrawPlanes()
	{
		for (int i = 0; i < planePositions.Length; i++)
		{
			if (planes[i] == null)
				return;

			Quaternion rotation = planes[i].rotation;
			Vector3 position = new Vector3(planePositions[i].x, planePositions[i].y, planePositions[i].z);

			for (int j = 0; j < 12; j++)
			{
				float t = (Time.time % 1f) + j;
				float intenisty = 1f - (float)j / 12f;

				Vector3 tl1 = new Vector3(1f, 0f, 1f) * t;
				Vector3 tr1 = new Vector3(-1f, 0f, 1f) * t;
				Vector3 bl1 = new Vector3(1f, 0f, -1f) * t;
				Vector3 br1 = new Vector3(-1f, 0f, -1f) * t;

				Vector3 tl2 = new Vector3(1f, 0.02f, 1f) * t;
				Vector3 tr2 = new Vector3(-1f, 0.02f, 1f) * t;
				Vector3 bl2 = new Vector3(1f, 0.02f, -1f) * t;
				Vector3 br2 = new Vector3(-1f, 0.02f, -1f) * t;

				tl1 = rotation * tl1;
				tr1 = rotation * tr1;
				bl1 = rotation * bl1;
				br1 = rotation * br1;

				tl2 = rotation * tl2;
				tr2 = rotation * tr2;
				bl2 = rotation * bl2;
				br2 = rotation * br2;

				Vector3 pos1 = position + tl1;
				Vector3 pos2 = position + tr1;
				Vector3 pos3 = position + bl1;
				Vector3 pos4 = position + br1;

				Vector3 pos5 = position + tl2;
				Vector3 pos6 = position + tr2;
				Vector3 pos7 = position + bl2;
				Vector3 pos8 = position + br2;

				Debug.DrawLine(pos1, pos2, new Color(1f, 0f, 0f, intenisty));
				Debug.DrawLine(pos2, pos4, new Color(1f, 0f, 0f, intenisty));
				Debug.DrawLine(pos4, pos3, new Color(1f, 0f, 0f, intenisty));
				Debug.DrawLine(pos3, pos1, new Color(1f, 0f, 0f, intenisty));

				Debug.DrawLine(pos5, pos6, new Color(0f, 1f, 0f, intenisty));
				Debug.DrawLine(pos6, pos8, new Color(0f, 1f, 0f, intenisty));
				Debug.DrawLine(pos8, pos7, new Color(0f, 1f, 0f, intenisty));
				Debug.DrawLine(pos7, pos5, new Color(0f, 1f, 0f, intenisty));
			}
		}
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "Icon_big.png", true);
	}

	public Bounds GetWSBounds()
	{
		if (particleMeshes == null)
		{
			return new Bounds();
		}

		if (particleMeshes.Length <= 0)
		{
			return new Bounds();
		}

		return particleMeshes[0].GetComponent<Renderer>().bounds;
	}
#endif
}
