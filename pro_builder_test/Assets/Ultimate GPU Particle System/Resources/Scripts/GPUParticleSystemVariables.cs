using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GPUParticleSystem
{
    #region Constants
    private const int particleColorPrecision = 256;
	private Vector3 safetyVector = new Vector3(0.01f,0f,0f);
    #endregion

    #region Enums
    public enum MeshBakeType {
        Vertex, Edge, Triangle
    };

    public enum GPUParticleBlendMode
    {
        Alpha, Additive, Screen, Premultiplied, Subtractive, Multiply, CutOff, Opaque
    };

    public enum CurveMode
    {
        Off, Linear, Smooth, Curve, RandomTwoCurves
    };

    public enum GPUParticleSystemState {
        Uninitialized, Paused, Stopped, Playing
    };

	public enum EmitterShape
	{
		Point, Edge, Circle, Box, HemiSphere, Sphere, Cone, Texture, Mesh, MeshFilter, SkinnedMeshRenderer
	};

	public enum ParticleType {
        Point, Triangle, Billboard, HorizontalBillboard, VerticalBillboard, StretchedTail, StretchedBillboard, Mesh, Trails
    };

    public enum TurbulenceType
    {
        Off, Texture, VectorField
    };

    public enum GPUSimulationSpace
    {
        Local, World
    };

    public enum ValueMode {
        Value, RandomTwoValues, Curve, RandomTwoCurves
    };

	public enum ColorMode
	{
		Color, RandomTwoColors, Gradient, RandomTwoGradients
	};

	public enum SimpleValueMode
    {
        Value, Curve
    };

    public enum TextureSheetMode
    {
        Off, TextureSheet, TextureSheetBlended, TextureSheetMotionVectors
    };
    
    public enum RenderTexturePrecision
    {
        Half, Float
    };

	public enum CollisionType
	{
		Off, Planes, Depth
	};

	public enum LightMode
	{
		Off, Standard, Refraction
	};
	#endregion

	#region Core
	[SerializeField]private int customInstanceID = -1;
	public GPUParticleSystemBuffer particleData;
    public Material particleMaterial;
	public int renderQueue = 3000;
    private GameObject[] particleMeshes;
	private MeshFilter[] particleMeshFilters;
	public int bufferWidth = 256;
    public int bufferHeight = 256;
	[System.NonSerialized] private float customTime = 0f;
	[System.NonSerialized] private float customDeltaTime = 0f;
	[System.NonSerialized] private float previousFrameTime = 0f;
    public RenderTexturePrecision precision = RenderTexturePrecision.Float;
    public bool useFixedDeltaTime = false;
    public float fixedDeltaTime = 0.016666666f;
	private int burstNum = 0;
	public ShaderVariantCollection shaderVariantCollection;
	public int seed = 123456789;
    #endregion

    #region Spawning
    private float startID = 0f;
    private float endID = 0f;
	#endregion

	#region General
	[System.NonSerialized] public GPUParticleSystemState state = GPUParticleSystemState.Uninitialized;
    public bool playOnAwake = true;
    public bool loop = true;
    public bool emit = true;
	[System.NonSerialized] private float effectStartTime = 0f;
    public float effectLength = 2f;
    [System.NonSerialized] public float progress = 0f;
	public float timeScale = 1.0f;
	//public SingleFloatCurveBundle timeScale = new SingleFloatCurveBundle(1.0f);
	#endregion

    #region Emitter
    public EmitterShape emitterShape = EmitterShape.Cone;
                                                //Cone      //Box       //Sphere    //Circle    //Edge      //HemiSphere	//Texture
    public float param1 = 1f;                   //Radius1   //X Size    //Radius    //Radius    //Length    //Radius		//Size x
    public float param2 = 25f;                  //Angle     //Y Size														//Size y
    public float param3 = 5f;                   //Length    //Z Size
    public float param4 = 1f;                   //Radius2
    public float randomness = 0f;
    public bool emitFromShell = false;
    public bool emitFromBase = true;            //Only Cone
	public GPUSimulationSpace simulationSpace = GPUSimulationSpace.World;
	public List<GPUParticleSystemBurst> bursts = new List<GPUParticleSystemBurst>();
	[System.NonSerialized]private List<GPUParticleSystemBurst> currentBursts = new List<GPUParticleSystemBurst>();
	private int burst;

	//Mesh Emitter
	public Mesh meshEmitter;
	//public Bounds worldBounds;
	//public Bounds bounds;
	public Vector3 center = new Vector3(0.0f, 0.0f, 12.0f);
	public Vector3 extents = new Vector3(8.0f, 8.0f, 12.0f);
	private Bounds boundsBuffer;
	public MeshFilter meshFilterEmitter;
	public int meshEmitterResolution = 16;
	public MeshBakeType meshBakeType = MeshBakeType.Vertex;
	public Texture2D meshEmitterPositionTexture;
	public Texture2D meshEmitterNormalTexture;

#pragma warning disable 414
	private float meshEmitterSize = 1f;
#pragma warning restore 414

	//Skinned mesh emitter
	public int smeLayer = 8;
	public int skinnedMeshEmitterResolution = 128;
	public SkinnedMeshRenderer skinnedMeshEmitter;
	private SkinnedMeshRenderer skinnedMeshEmitterSME;
	private GameObject sme;
	public Transform skinnedMeshEmitterTransform;
	public Camera skinnedMeshEmitterCam;
	public Transform skinnedMeshEmitterCamTransform;
	private Material skinnedMeshEmitterMaterial;
	private RenderTexture skinnedMeshEmitterPositionTexture;
	public Mesh skinnedMeshEmitterConvertedMesh;
	#endregion

	#region Emission
	public FloatCurveBundle emissionRate = new FloatCurveBundle(2500f, 2500f);
    public int maxParticles = 65536;
    #endregion

    #region StartValues
    public FloatCurveBundle startLifetime = new FloatCurveBundle(5f, 5f);
    public FloatCurveBundle startSize = new FloatCurveBundle(.1f, .22f);
	public FloatCurveBundle startSpeed = new FloatCurveBundle(5f, 5f);
	public FloatCurveBundle startRotation = new FloatCurveBundle(0f, 0f);
	public bool useRotation = false;
	#endregion

	#region LifetimeValues
	public ShaderCurveBundle sizeOverLifetime = new ShaderCurveBundle();
    public ShaderCurveBundle rotationOverLifetime = new ShaderCurveBundle();
	public ColorGradientBundle colorOverLifetime = new ColorGradientBundle();
	public SingleFloatCurveBundle colorIntensityOverLifetime = new SingleFloatCurveBundle(1f);
	public SingleFloatCurveBundle maxVelocity = new SingleFloatCurveBundle(25f);
	public bool useMaxVelocity = false;
    private Texture2D colorTexture;
    #endregion

    #region ParticleShape
    public ParticleType particleType = ParticleType.Triangle;
    #endregion

    #region ParticleMesh
    public Mesh meshParticle;
	#endregion

	#region Trails
	public float followSpeed = 15f;
	#endregion

	#region Forces
	public SingleFloatCurveBundle gravity = new SingleFloatCurveBundle(0f);
	private Vector3 previousEmitterPosition = Vector3.zero;
	private Vector3 emitterVelocity = Vector3.zero;
	public SingleFloatCurveBundle inheritVelocityMultiplyer = new SingleFloatCurveBundle(0f);
    public bool useInheritVelocity = false;
	public SingleFloatCurveBundle airResistance = new SingleFloatCurveBundle(0f);
	public Vector3CurveBundle forceOverLifetime = new Vector3CurveBundle();
    public bool useCircularForce = false;
	public Vector3CurveBundle circularForce = new Vector3CurveBundle();
    public Transform circularForceCenter;
    #endregion

    #region Turbulence
    public TurbulenceType turbulenceType = TurbulenceType.Off;
    public TextAsset fgaFile;
    public Texture3D vectorField;
    public Texture2D vectorNoise;
	public float Tightness = 1f;
	public Vector3CurveBundle turbulenceAmplitude = new Vector3CurveBundle();
    public Vector3CurveBundle turbulenceFrequency = new Vector3CurveBundle();
	public Vector3CurveBundle turbulenceOffset = new Vector3CurveBundle();
	public Vector3CurveBundle turbulenceRotation = new Vector3CurveBundle();
	[SerializeField] private Transform vectorFieldObject;
	#endregion

	#region Attractors
	public List<Attractor> attractors = new List<Attractor>();
	public bool useMeshTarget = false;
	public bool useMeshFilter = false;
	public bool targetIsSameMeshAsEmitter = false;
	public SingleFloatCurveBundle meshTargetStrength = new SingleFloatCurveBundle(1f);
	public SingleFloatCurveBundle onTarget = new SingleFloatCurveBundle(0f);
	public Mesh meshTarget;
	public MeshFilter meshFilterTarget;
	public int meshTargetResolution = 16;
	public MeshBakeType targetBakeType = MeshBakeType.Vertex;
	public Texture2D meshTargetPositionTexture;
#pragma warning disable 414
	private float meshTargetSize = 1f;
#pragma warning restore 414
	#endregion

	#region Collision
	public CollisionType collisionType;
	public Vector4[] planePositions = new Vector4[6];   //XYZ Position; W = Velocity loss
	public Vector4[] planeNormals = new Vector4[6];
	public Transform[] planes = new Transform[6];
	public float[] collisionDamping = new float[6];
	public Camera collisionCamera;
	public Transform collisionCameraTransform;
	private Matrix4x4 MVP;
	public float depthCollisionDamping = 1.0f;
	public float dampingRandomness = 1.0f;
	public float depthCollisionDistance = 0.1f;
	#endregion

	#region Rendering
	public float aspectRatio = 1f;
	public Vector3 forwardVector = new Vector3(1f,0f,0f);
	public Vector3 offset = Vector3.zero;
	public Texture2D mainTexture;
	public Texture2D motionVectors;
	public TextureSheetMode textureSheetMode = TextureSheetMode.Off;
	public float motionVectorStrength = 0.05f;
    public bool textureSheetRandomIndex = false;
	public int rows = 6;
	public int columns = 6;
    public GPUParticleBlendMode blendMode = GPUParticleBlendMode.Additive;
    public bool useZbuffer = false;
    public float stretchMultiplier = .1f;
	public Vector2 minMaxStretch = new Vector2(0.1f, 1.0f);
	public LightMode lightMode = LightMode.Off;
	public float metallic = 1.0f;
	public float smoothness = 0.0f;
	public Texture2D metallicSmoothness;
	public Texture2D normalMap;
	public Texture2D emissionMap;
	public UnityEngine.Rendering.ShadowCastingMode castShadows = UnityEngine.Rendering.ShadowCastingMode.Off;
	public bool receiveShadows = false;
	public Texture2D refractionNormals;
	public float indexOfRefraction = 1f;
	#endregion

	#region Editor
#if UNITY_EDITOR
	public string savePath = "";
	public bool GeneralsTab = false;
	public bool EmitterTab = true;
	public bool EmissionTab = true;
	public bool StartValuesTab = false;
	public bool LifetimeValuesTab = false;
	public bool ForcesTab = false;
	public bool TurbulenceTab = false;
	public bool AttractorsTab = false;
	public bool MeshTargetTab = false;
	public bool CollisionTab = false;
	public bool RenderingTab = false;
	public bool MaterialTab = false;
	public bool EditBounds = false;
	public bool ShowWSBounds = false;
	private Quaternion tempRotation;
	private int previousLayer = 0;
	private bool firstTimeSwitchedToTrails = false;

	public void StartEditBB()
	{
		tempRotation = transform.rotation;
		transform.rotation = Quaternion.identity;
	}

	public void EndEditBB()
	{
		transform.rotation = tempRotation;
	}

	private void CheckIfLayerChanged()
	{
		if (previousLayer != gameObject.layer)
		{
			previousLayer = gameObject.layer;
			SetLayer();
		}
	}
#endif
	#endregion

	#region Debug
	public void DebugOn()
	{
		foreach (GameObject g in Object.FindObjectsOfType(typeof(GameObject)))
		{
			g.hideFlags = HideFlags.None;
		}
	}

	public void DebugOff()
	{
		for (int i = 0; i < particleMeshes.Length; i++)
		{
			particleMeshes[i].hideFlags = HideFlags.HideInHierarchy;
		}
	}
	#endregion

}
