using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPUParticleSystemMeshUtility
{
	#region LocalBuffer
#pragma warning disable 414
	private static Vector3[] Vertices = new Vector3[0];
    private static Vector3[] Normals = new Vector3[0];
	private static Vector4[] Tangents = new Vector4[0];
	private static int[] Triangles = new int[0];
	private static Vector2[] UVs = new Vector2[0];
	private static Vector2[] PosUV = new Vector2[0];
	private static Vector2[] AnimUV = new Vector2[0];

	public static float ignoreSize = 0f;//0.0002f;
    private static int numTriangles;
    private static Vector3[] vertexBuffer = new Vector3[0];
    private static Vector3[] normalBuffer = new Vector3[0];
	private static int[] triangleBuffer = new int[0];
	private static Vector2[] uV1Buffer = new Vector2[0];
	private static Vector2[] uV2Buffer = new Vector2[0];
	private static float[] weightsBuffer = new float[0];
	private static float Buffer = 0;
    private static Vector3 vectorBuffer;
    private static float sizeBuffer;
	private static Color[] positionArray = new Color[0];
	private static Color[] normalArray = new Color[0];
	//private static Texture2D positionsBuffer;
#pragma warning restore 414
	#endregion

	public static GameObject[] CreateParticlesPoint(int _Width, int _Height, Material _Mat)
    {
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt(((float)NumParticles) / 65535);

        GameObject[] MeshHolders = new GameObject[NumMeshes];

        int CurrentVertNum = 65535;
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            if (n == NumMeshes - 1)
                CurrentVertNum = NumParticles % 65535;

            Vertices = new Vector3[CurrentVertNum];
            Triangles = new int[CurrentVertNum];
            PosUV = new Vector2[CurrentVertNum];

            //Create Vertices
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new Vector3(0f, 0f, 0f);
                Triangles[i] = i;
            }

            //Create UVs
            for (int i = 0; i < PosUV.Length; i++, index++)
            {
				float uCoord = ((float)index % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height) / _Height;

				PosUV[i] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
            }

            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.SetIndices(Triangles, MeshTopology.Points, 0);
            mesh.uv = PosUV;
			g.AddComponent<MeshFilter>().mesh = mesh;
            g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
			g.hideFlags = HideFlags.HideAndDontSave;
			MeshHolders[n] = g;
        }

        return MeshHolders;
    }

	public static GameObject[] CreateParticlesTriangle(int _Width, int _Height, Material _Mat)
	{
		int NumParticles = _Width * _Height;
		int NumMeshes = Mathf.CeilToInt((NumParticles * 3f) / 65535);	// 3 verts + 1 tri per particle devided by max vert count

		GameObject[] MeshHolders = new GameObject[NumMeshes];

		int CurrentVertCount = 21845;									// Max vert count devided by 3
		Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
		int index = 0;

		for (int n = 0; n < NumMeshes; n++)
		{
			if (n == NumMeshes - 1)
				CurrentVertCount = NumParticles % 21845;

			Vertices = new Vector3[CurrentVertCount * 3];
			Normals = new Vector3[CurrentVertCount * 3];
			Triangles = new int[CurrentVertCount * 3];
			PosUV = new Vector2[CurrentVertCount * 3];
			UVs = new Vector2[CurrentVertCount * 3];

			//Create Vertices
			for (int i = 0; i < Vertices.Length; i += 3)
			{
				Vertices[i] = new Vector3(0f, 0f, 0f);
				Vertices[i + 1] = new Vector3(0f, 0f, 0f);
				Vertices[i + 2] = new Vector3(0f, 0f, 0f);

				Normals[i] = new Vector3(0f, 0f, -1f);
				Normals[i + 1] = new Vector3(0f, 0f, -1f);
				Normals[i + 2] = new Vector3(0f, 0f, -1f);
			}

			int triIndex = 0;

			for (int i = 0; i < Vertices.Length; i += 3)
			{
				Triangles[triIndex] = i;
				triIndex++;
				Triangles[triIndex] = i + 2;
				triIndex++;
				Triangles[triIndex] = i + 1;
				triIndex++;
			}

			for (int i = 0; i < CurrentVertCount; i++, index++)
			{
				UVs[i * 3] = new Vector2(0f, 0f);
				UVs[i * 3 + 1] = new Vector2(1f, 0f);
				UVs[i * 3 + 2] = new Vector2(.5f, 1f);

				float uCoord = ((float)index%(float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height ) / _Height;

				PosUV[i * 3] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 3 + 1] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 3 + 2] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
			}

			GameObject g = new GameObject("ParticleHolder_" + n.ToString());
			Mesh mesh = new Mesh();
			mesh.name = "Particle Mesh";
			mesh.vertices = Vertices;
			mesh.triangles = Triangles;
			mesh.uv = PosUV;
			mesh.uv2 = UVs;
			mesh.normals = Normals;
			g.AddComponent<MeshFilter>().mesh = mesh;
			g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
			g.hideFlags = HideFlags.HideAndDontSave;
			MeshHolders[n] = g;
		}

		return MeshHolders;
	}

	public static GameObject[] CreateParticlesQuad(int _Width, int _Height, Material _Mat)
	{
		int NumParticles = _Width * _Height;
		int NumMeshes = Mathf.CeilToInt(((float)NumParticles) / 16250);

		GameObject[] MeshHolders = new GameObject[NumMeshes];

		int CurrentVertNum = 16250;
		Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
		int index = 0;

		for (int n = 0; n < NumMeshes; n++)
		{
			if (n == NumMeshes - 1)
				CurrentVertNum = NumParticles % 16250;

			Vertices = new Vector3[CurrentVertNum * 4];
			Normals = new Vector3[CurrentVertNum * 4];
			Tangents = new Vector4[CurrentVertNum * 4];
			Triangles = new int[CurrentVertNum * 6];
			PosUV = new Vector2[CurrentVertNum * 4];
			UVs = new Vector2[CurrentVertNum * 4];

			//Create Vertices
			for (int i = 0; i < Vertices.Length; i += 4)
			{
				Vertices[i] = new Vector3(0f, 0f, 0f);
				Vertices[i + 1] = new Vector3(0f, 0f, 0f);
				Vertices[i + 2] = new Vector3(0f, 0f, 0f);
				Vertices[i + 3] = new Vector3(0f, 0f, 0f);

				Normals[i] = new Vector3(0f, 0f, -1f);
				Normals[i + 1] = new Vector3(0f, 0f, -1f);
				Normals[i + 2] = new Vector3(0f, 0f, -1f);
				Normals[i + 3] = new Vector3(0f, 0f, -1f);

				Tangents[i] = new Vector3(1f, 0f, 0f);
				Tangents[i + 1] = new Vector3(1f, 0f, 0f);
				Tangents[i + 2] = new Vector3(1f, 0f, 0f);
				Tangents[i + 3] = new Vector3(1f, 0f, 0f);
			}

			int triIndex = 0;

			for (int i = 0; i < Vertices.Length; i += 4)
			{
				Triangles[triIndex] = i;
				triIndex++;
				Triangles[triIndex] = i + 3;
				triIndex++;
				Triangles[triIndex] = i + 1;
				triIndex++;

				Triangles[triIndex] = i + 1;
				triIndex++;
				Triangles[triIndex] = i + 3;
				triIndex++;
				Triangles[triIndex] = i + 2;
				triIndex++;
			}

			//Create UVs
			for (int i = 0; i < CurrentVertNum; i++, index++)
			{
				UVs[i * 4] = new Vector2(0f, 0f);
				UVs[i * 4 + 1] = new Vector2(1f, 0f);
				UVs[i * 4 + 2] = new Vector2(1f, 1f);
				UVs[i * 4 + 3] = new Vector2(0f, 1f);

				float uCoord = ((float)index % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height) / _Height;

				PosUV[i * 4] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 4 + 1] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 4 + 2] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 4 + 3] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
			}

			GameObject g = new GameObject("ParticleHolder_" + n.ToString());
			Mesh mesh = new Mesh();
			mesh.vertices = Vertices;
			mesh.triangles = Triangles;
			mesh.uv = PosUV;
			mesh.uv2 = UVs;
			mesh.normals = Normals;
			mesh.tangents = Tangents;
			g.AddComponent<MeshFilter>().mesh = mesh;
			g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
			g.hideFlags = HideFlags.HideAndDontSave;
			MeshHolders[n] = g;
		}

		return MeshHolders;
	}

	public static GameObject[] CreateParticlesDoubleQuad(int _Width, int _Height, Material _Mat)
    {
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt(((float)NumParticles) / 13100);

        GameObject[] MeshHolders = new GameObject[NumMeshes];

        int CurrentVertNum = 13100;
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            if (n == NumMeshes - 1)
                CurrentVertNum = NumParticles % 13100;

            Vertices = new Vector3[CurrentVertNum * 5];
            Normals = new Vector3[CurrentVertNum * 5];
            Triangles = new int[CurrentVertNum * 9];
            PosUV = new Vector2[CurrentVertNum * 5];
            UVs = new Vector2[CurrentVertNum * 5];

            //Create Vertices
            for (int i = 0; i < Vertices.Length; i += 5)
            {
                Vertices[i] = new Vector3(1f, 0f, 0f);
                Vertices[i + 1] = new Vector3(0f, 0f, 0f);
                Vertices[i + 2] = new Vector3(0f, 0f, 0f);
                Vertices[i + 3] = new Vector3(0f, 0f, 0f);
                Vertices[i + 4] = new Vector3(0f, 0f, 0f);

                Normals[i] = new Vector3(0f, 0f, -1f);
                Normals[i + 1] = new Vector3(0f, 0f, -1f);
                Normals[i + 2] = new Vector3(0f, 0f, -1f);
                Normals[i + 3] = new Vector3(0f, 0f, -1f);
                Normals[i + 4] = new Vector3(0f, 0f, -1f);
            }

            int triIndex = 0;

            for (int i = 0; i < Vertices.Length; i += 5)
            {
                Triangles[triIndex] = i;
                triIndex++;
                Triangles[triIndex] = i + 1;
                triIndex++;
                Triangles[triIndex] = i + 2;
                triIndex++;

                Triangles[triIndex] = i + 1;
                triIndex++;
                Triangles[triIndex] = i + 3;
                triIndex++;
                Triangles[triIndex] = i + 2;
                triIndex++;

                Triangles[triIndex] = i + 2;
                triIndex++;
                Triangles[triIndex] = i + 3;
                triIndex++;
                Triangles[triIndex] = i + 4;
                triIndex++;
            }

            //Create UVs
            for (int i = 0; i < CurrentVertNum; i++, index++)
            {
                UVs[i * 5] = new Vector2(0f, 0.5f);
                UVs[i * 5 + 1] = new Vector2(0.5f, 1f);
                UVs[i * 5 + 2] = new Vector2(.5f, 0f);
                UVs[i * 5 + 3] = new Vector2(1f, 1f);
                UVs[i * 5 + 4] = new Vector2(1f, 0f);

				float uCoord = ((float)index % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height) / _Height;

				PosUV[i * 5] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 1] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 2] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 3] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 4] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
            }

            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.uv = PosUV;
            mesh.uv2 = UVs;
            mesh.normals = Normals;
			g.AddComponent<MeshFilter>().mesh = mesh;
            g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
            MeshHolders[n] = g;
        }
		
        foreach (GameObject g in MeshHolders)
        {
            g.hideFlags = HideFlags.HideAndDontSave;
        }
		
        return MeshHolders;
    }

    public static GameObject[] CreateMeshParticles(Mesh _Mesh, int _Width, int _Height, Material _Mat, bool _2ndUV)
    {
        int oMeshVertNum = _Mesh.vertexCount;
        int oMeshTriNum = _Mesh.triangles.Length;
        int oMeshNormNum = _Mesh.normals.Length;
        int oMeshTanNum = _Mesh.tangents.Length;
        int ParticlesPerMesh = Mathf.FloorToInt(65000f / (float)oMeshVertNum);
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt((float)NumParticles / (float)ParticlesPerMesh);

        GameObject[] MeshHolders = new GameObject[NumMeshes];
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        float AnimHalfTexelOffset = (1f / (float)oMeshVertNum) / 2f;

        //Original Mesh
        Vector3[] oVertices = _Mesh.vertices;
        Vector4[] oTangents = _Mesh.tangents;
        Vector3[] oNormals = _Mesh.normals;
        int[] oTriangles = _Mesh.triangles;
        Vector2[] oUV1 = _Mesh.uv;

        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh m = new Mesh();

            int nVertNum = oMeshVertNum * ParticlesPerMesh;
            int nTriNum = oMeshTriNum * ParticlesPerMesh;
            int nNormNum = oMeshNormNum * ParticlesPerMesh;
            int nTangentsNum = oMeshTanNum * ParticlesPerMesh;
            Vertices = new Vector3[nVertNum];
            Normals = new Vector3[nNormNum];
            Tangents = new Vector4[nTangentsNum];
            int[] nTriangles = new int[nTriNum];
            UVs = new Vector2[nVertNum];
            PosUV = new Vector2[nVertNum];
            //AnimUV = new Vector2[nVertNum];

            int vIndex = 0;

            for (int v = 0; v < nVertNum; v++)
            {
                Vertices[v] = oVertices[vIndex];

                if (vIndex >= oMeshVertNum - 1)
                {
                    vIndex = 0;
                }
                else
                {
                    vIndex++;
                }
            }

            int nIndex = 0;

            for (int v = 0; v < nNormNum; v++)
            {
                Normals[v] = oNormals[nIndex];

                if (nIndex >= oMeshNormNum - 1)
                {
                    nIndex = 0;
                }
                else
                {
                    nIndex++;
                }
            }

            int tanIndex = 0;

            for (int v = 0; v < nTangentsNum; v++)
            {
                Tangents[v] = oTangents[tanIndex];

                if (tanIndex >= oMeshTanNum - 1)
                {
                    tanIndex = 0;
                }
                else
                {
                    tanIndex++;
                }
            }

            int tIndex = 0;

            for (int t = 0; t < nTriNum; t++)
            {
                nTriangles[t] = oTriangles[tIndex] + (oMeshVertNum * Mathf.FloorToInt((float)t / (float)oMeshTriNum));

                if (tIndex >= oMeshTriNum - 1)
                {
                    tIndex = 0;
                }
                else
                {
                    tIndex++;
                }
            }

            int uvIndex = 0;

            //Create UVs
            for (int u = 0; u < nVertNum; u++)
            {
				if (u < oUV1.Length)
				{
					UVs[u] = oUV1[uvIndex];
				}

                int uIndex = Mathf.FloorToInt((float)index / (float)oMeshVertNum);
                index++;

				float uCoord = ((float)uIndex % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)uIndex / (float)_Width)) % _Height) / _Height;

				PosUV[u] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                //AnimUV[u] = new Vector2(0f, (float)u / (float)oMeshVertNum) + new Vector2(0f, AnimHalfTexelOffset);

                if (uvIndex >= oMeshVertNum - 1)
                {
                    uvIndex = 0;
                }
                else
                {
                    uvIndex++;
                }
            }
			
            m.vertices = Vertices;
			m.triangles = nTriangles;
			m.normals = Normals;
			m.tangents = Tangents;
            m.uv = PosUV;
            m.uv2 = UVs;
			g.AddComponent<MeshFilter>().mesh = m;
            g.AddComponent<MeshRenderer>().material = _Mat;
            MeshHolders[n] = g;
        }
		
        foreach (GameObject g in MeshHolders)
        {
            g.hideFlags = HideFlags.HideAndDontSave;
        }
		
        return MeshHolders;
    }

    public static Vector4[] AnimationCurveToBezier(AnimationCurve Curve)
    {
        Curve = CheckAnimationCurve(Curve);

        List<Vector4> BezierPoints = new List<Vector4>();

        int numKeys = Curve.keys.Length;

        if (numKeys > 0)
        {
            Keyframe[] keys = Curve.keys;

            for (int i = 0; i < numKeys - 1; i++)
            {
                Vector4 P1C1 = Vector4.zero;
                Vector4 P2C2 = Vector4.zero;

                CurveToBezier(keys[i], keys[i + 1], out P1C1, out P2C2);

                BezierPoints.Add(P1C1);
                BezierPoints.Add(P2C2);
            }
        }

        while (BezierPoints.Count < 10)
        {
            BezierPoints.Add(Vector4.zero);
        }

        return BezierPoints.ToArray();
    }

    public static AnimationCurve CheckAnimationCurve(AnimationCurve Curve)
    {
        int numKeys = Curve.keys.Length;

        if (numKeys > 6)
        {
            Debug.LogWarning("[UPS] Due to Shader optimizations, no more than 6 keyframes are allowed!");
            Keyframe[] newKewFrames = new Keyframe[6];

            for (int i = 0; i < 6; i++)
            {
                newKewFrames[i] = Curve.keys[i];
            }
            Curve.keys = newKewFrames;
        }
        return Curve;
    }

    public static void CurveToBezier(Keyframe k0, Keyframe k1, out Vector4 P1C1, out Vector4 P2C2)
    {

        float tgIn = k1.inTangent;          //inTangent
        float tgOut = k0.outTangent;        //outTangent

        P1C1.x = k0.time;
        P1C1.y = k0.value;
        P2C2.x = k1.time;
        P2C2.y = k1.value;

        float tangLengthX = Mathf.Abs(P1C1.x - P2C2.x) * 0.333333f;
        float tangLengthY = tangLengthX;
        Vector2 c1 = new Vector2(P1C1.x, P1C1.y);
        Vector2 c2 = new Vector2(P2C2.x, P2C2.y);

        c1.x += tangLengthX;
        c1.y += tangLengthY * tgOut;
        c2.x -= tangLengthX;
        c2.y -= tangLengthY * tgIn;

        P1C1.z = c1.x;
        P1C1.w = c1.y;
        P2C2.z = c2.x;
        P2C2.w = c2.y;
    }

	public static Texture2D MeshToTexture(Mesh mesh, GPUParticleSystem.MeshBakeType bakeType, int size)
	{
		int ArraySize = size * size;

		Texture2D positions = new Texture2D(size, size, TextureFormat.RGBAHalf, false);
		positions.filterMode = FilterMode.Point;

		if (ArraySize != positionArray.Length)
		{
			positionArray = new Color[ArraySize];
		}

		vertexBuffer = mesh.vertices;
		triangleBuffer = mesh.triangles;
		weightsBuffer = new float[mesh.triangles.Length / 3];

		CreateTextureColours(bakeType, ref vertexBuffer, ref triangleBuffer, ref weightsBuffer, ArraySize);

		positions.SetPixels(positionArray);
		positions.Apply(false);
		return positions;
	}

	public static void MeshToPositionNormals(out Texture2D positions, out Texture2D normals, Mesh mesh, GPUParticleSystem.MeshBakeType bakeType, int size)
	{
		int ArraySize = size * size;

		positions = new Texture2D(size, size, TextureFormat.RGBAHalf, false);
		positions.filterMode = FilterMode.Point;

		normals = new Texture2D(size, size, TextureFormat.RGBAHalf, false);
		normals.filterMode = FilterMode.Point;

		if (ArraySize != positionArray.Length)
		{
			positionArray = new Color[ArraySize];
			normalArray = new Color[ArraySize];
		}

		vertexBuffer = mesh.vertices;
		normalBuffer = mesh.normals;
		triangleBuffer = mesh.triangles;
		weightsBuffer = new float[mesh.triangles.Length / 3];

		CreateTextureColours(bakeType, ref vertexBuffer, ref normalBuffer, ref triangleBuffer, ref weightsBuffer, ArraySize);

		positions.SetPixels(positionArray);
		positions.Apply(false);
		normals.SetPixels(normalArray);
		normals.Apply(false);
	}

	private static void CreateTextureColours(GPUParticleSystem.MeshBakeType geometryType, ref Vector3[] vertices, ref Vector3[] normals, ref int[] tris, ref float[] weights, int arraySize)
	{
		switch (geometryType)
		{
			case GPUParticleSystem.MeshBakeType.Vertex:
				for (int i = 0; i < arraySize; i++)
				{
					int randomIndex = Random.Range(0, vertices.Length);
					Vector3 position = vertices[randomIndex];
					positionArray[i] = new Color(position.x, position.y, position.z, 1.0f);
					Vector3 normal = normals[randomIndex];
					normalArray[i] = new Color(normal.x, normal.y, normal.z, 1.0f);
				}
				break;

			case GPUParticleSystem.MeshBakeType.Edge:
				for (int i = 0; i < arraySize; i++)
				{
					int startIndex = Random.Range(0, tris.Length / 3) * 3;
					int startTriangle = Random.Range(0, 3);
					int endTriangle = 0;

					if (startTriangle != 2)
					{
						endTriangle = Random.Range(1, 3);
					}

					Vector3 position1 = vertices[tris[startIndex + startTriangle]];
					Vector3 position2 = vertices[tris[startIndex + endTriangle]];

					Vector3 normal1 = normals[tris[startIndex + startTriangle]];
					Vector3 normal2 = normals[tris[startIndex + endTriangle]];

					//Vector3 Pos = Vector3.Lerp(Pos1, Pos2, Rnd.Next());
					float blend = Random.Range(0f, 1f);
					Vector3 position = Vector3.Lerp(position1, position2, blend);
					Vector3 normal = Vector3.Lerp(normal1, normal2, blend);
					positionArray[i] = new Color(position.x, position.y, position.z, 1.0f);
					normalArray[i] = new Color(normal.x, normal.y, normal.z, 1.0f);
				}
				break;

			case GPUParticleSystem.MeshBakeType.Triangle:

				float wholeMeshArea = 0.0f;
				int triangleArea = 0;

				//Calculate Triangle and Whole Triangle Size
				CalculateTriangles(ref vertices, ref tris, ref weights, ref wholeMeshArea, ref triangleArea);

				//Normalize
				Normalize(ref weights, ref wholeMeshArea);

				//Generate Random Point
				GenerateRandomPoint(ref vertices, ref normals, ref tris, ref weights, arraySize);
				break;
		}
	}

	private static void CreateTextureColours(GPUParticleSystem.MeshBakeType geometryType, ref Vector3[] vertices, ref int[] tris, ref float[] weights, int arraySize)
	{
		switch (geometryType)
		{
			case GPUParticleSystem.MeshBakeType.Vertex:
				for (int i = 0; i < arraySize; i++)
				{
					int randomIndex = Random.Range(0, vertices.Length);
					Vector3 position = vertices[randomIndex];
					positionArray[i] = new Color(position.x, position.y, position.z, 1.0f);
				}
				break;

			case GPUParticleSystem.MeshBakeType.Edge:
				for (int i = 0; i < arraySize; i++)
				{
					int startIndex = Random.Range(0, tris.Length / 3) * 3;
					int startTriangle = Random.Range(0, 3);
					int endTriangle = 0;

					if (startTriangle != 2)
					{
						endTriangle = Random.Range(1, 3);
					}

					Vector3 position1 = vertices[tris[startIndex + startTriangle]];
					Vector3 position2 = vertices[tris[startIndex + endTriangle]];

					//Vector3 Pos = Vector3.Lerp(Pos1, Pos2, Rnd.Next());
					float blend = Random.Range(0f, 1f);
					Vector3 position = Vector3.Lerp(position1, position2, blend);
					positionArray[i] = new Color(position.x, position.y, position.z, 1.0f);
				}
				break;

			case GPUParticleSystem.MeshBakeType.Triangle:

				float wholeMeshArea = 0.0f;
				int triangleArea = 0;

				//Calculate Triangle and Whole Triangle Size
				CalculateTriangles(ref vertices, ref tris, ref weights, ref wholeMeshArea, ref triangleArea);

				//Normalize
				Normalize(ref weights, ref wholeMeshArea);

				//Generate Random Point
				GenerateRandomPoint(ref vertices, ref tris, ref weights, arraySize);
				break;
		}
	}

	private static void CalculateTriangles(ref Vector3[] vertices, ref int[] tris, ref float[] weights, ref float wholeMeshArea, ref int tri)
	{
		int triangleLength = tris.Length;
		for (int j = 0; j < triangleLength; j += 3)
		{
			Vector3 A = vertices[tris[j]];
			Vector3 B = vertices[tris[j + 1]];
			Vector3 C = vertices[tris[j + 2]];
			Vector3 V = Vector3.Cross(A - B, A - C);
			float area = V.magnitude * 0.5f;
			weights[tri] = area;
			++tri;
			wholeMeshArea += area;
		}
	}

	private static void Normalize(ref float[] Weights, ref float WholeMeshArea)
	{
		int weightsLength = Weights.Length;
		for (int j = 0; j < weightsLength; ++j)
		{
			Weights[j] /= WholeMeshArea;
		}
	}

	private static void GenerateRandomPoint(ref Vector3[] Vertices, ref Vector3[] normals, ref int[] Tris, ref float[] Weights, int ArraySize)
	{
		int weightLength = Weights.Length;
		for (int i = 0; i < ArraySize; ++i)
		{
			float RandomTriangle = Random.Range(0f, 1f);
			float acc = 0.0f;
			int TriangleIndex = 0;
			GetWeights(ref Weights, ref weightLength, ref RandomTriangle, ref acc, ref TriangleIndex);
			Vector3 position = GenerateRandomPosition(ref Vertices, ref Tris, TriangleIndex);
			Vector3 normal = GenerateRandomNormal(ref normals, ref Tris, TriangleIndex);
			AssignPosition(i, position);
			AssignNormal(i, normal);
		}
	}

	private static void GenerateRandomPoint(ref Vector3[] Vertices, ref int[] Tris, ref float[] Weights, int ArraySize)
	{
		int weightLength = Weights.Length;
		for (int i = 0; i < ArraySize; ++i)
		{
			float RandomTriangle = Random.Range(0f, 1f);
			float acc = 0.0f;
			int TriangleIndex = 0;
			GetWeights(ref Weights, ref weightLength, ref RandomTriangle, ref acc, ref TriangleIndex);
			Vector3 position = GenerateRandomPosition(ref Vertices, ref Tris, TriangleIndex);
			AssignPosition(i, position);
		}
	}

	private static Vector3 GenerateRandomPosition(ref Vector3[] Vertices, ref int[] Tris, int TriangleIndex)
	{
		Vector3 position1 = Vertices[Tris[TriangleIndex]];
		Vector3 position2 = Vertices[Tris[TriangleIndex + 1]];
		Vector3 position3 = Vertices[Tris[TriangleIndex + 2]];

		Vector3 position = GetRandomPointOnTriangle(position1, position2, position3);
		return position;
	}

	private static Vector3 GenerateRandomNormal(ref Vector3[] normals, ref int[] Tris, int TriangleIndex)
	{
		Vector3 normal1 = normals[Tris[TriangleIndex]];
		Vector3 normal2 = normals[Tris[TriangleIndex + 1]];
		Vector3 normal3 = normals[Tris[TriangleIndex + 2]];

		Vector3 normal = GetNormalOfTriangle(normal1, normal2, normal3);
		return normal;
	}

	private static void AssignPosition(int i, Vector3 position)
	{
		Color col = positionArray[i];
		col.r = position.x;
		col.g = position.y;
		col.b = position.z;
		positionArray[i] = col;
	}

	private static void AssignNormal(int i, Vector3 normal)
	{
		Color col = normalArray[i];
		col.r = normal.x;
		col.g = normal.y;
		col.b = normal.z;
		normalArray[i] = col;
	}

	private static void GetWeights(ref float[] Weights, ref int weightLength, ref float RandomTriangle, ref float acc, ref int TriangleIndex)
	{
		for (int j = 0; j < weightLength; ++j)
		{
			acc += Weights[j];

			if (acc >= RandomTriangle)
			{
				TriangleIndex = j * 3;
				break;
			}
		}
	}

	private static Vector3 GetRandomPointOnTriangle(Vector3 P1, Vector3 P2, Vector3 P3)
	{
		float r1 = Random.Range(0f, 1f);
		float r2 = Random.Range(0f, 1f);

		float Sqrtr1 = Mathf.Sqrt(r1);
		float negSqrtr1 = 1f - Mathf.Sqrt(r1);

		return new Vector3((negSqrtr1) * P1.x + (Sqrtr1 * (1f - r2)) * P2.x + (Sqrtr1 * r2) * P3.x,
			(negSqrtr1) * P1.y + (Sqrtr1 * (1f - r2)) * P2.y + (Sqrtr1 * r2) * P3.y,
			(negSqrtr1) * P1.z + (Sqrtr1 * (1f - r2)) * P2.z + (Sqrtr1 * r2) * P3.z
			);
	}

	private static Vector3 GetNormalOfTriangle(Vector3 n1, Vector3 n2, Vector3 n3)
	{
		return Vector3.Normalize((n1 + n2 + n3) / 3f);
	}

	//Skinned Mesh Emitter
	//Splits all triangles (unique vertices, normals etc.) and adds 2nd UV set.
	public static Mesh ProcessSkinnedMesh(Mesh mesh)
	{
		Mesh newMesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uv0 = new List<Vector2>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector4> tangents = new List<Vector4>();
		List<BoneWeight> boneWeights = new List<BoneWeight>();
		List<Matrix4x4> bindPoses = new List<Matrix4x4>();

		List<Vector3> newVertices = new List<Vector3>();
		List<Vector2> newUv0 = new List<Vector2>();
		List<Vector2> newUv1 = new List<Vector2>();
		List<Vector3> newNormals = new List<Vector3>();
		List<Vector4> newTangents = new List<Vector4>();
		List<BoneWeight> newBoneWeights = new List<BoneWeight>();

		mesh.GetVertices(vertices);
		mesh.GetUVs(0, uv0);
		mesh.GetNormals(normals);
		mesh.GetTangents(tangents);
		mesh.GetBoneWeights(boneWeights);
		mesh.GetBindposes(bindPoses);

		int subMeshesCount = mesh.subMeshCount;
		List<List<int>> newSubmeshTriangles = new List<List<int>>();

		int newIndex = 0;

		for (int s = 0; s < subMeshesCount; s++)
		{
			List<int> triangles = new List<int>();
			mesh.GetIndices(triangles, s);

			List<int> nTriangles = new List<int>();

			for (int t = 0; t < triangles.Count; t++)
			{
				int currentIndex = triangles[t];

				newVertices.Add(vertices[currentIndex]);
				newUv0.Add(uv0[currentIndex]);
				newNormals.Add(normals[currentIndex]);
				newTangents.Add(tangents[currentIndex]);
				newBoneWeights.Add(boneWeights[currentIndex]);

				nTriangles.Add(newIndex);
				newIndex++;
			}

			newSubmeshTriangles.Add(nTriangles);
		}

		float[][] weights = new float[subMeshesCount][];

		for (int i = 0; i < newSubmeshTriangles.Count; i++)
		{
			weights[i] = new float[Mathf.FloorToInt(newSubmeshTriangles[i].Count / 3f)];
		}

		WeightTriangles(newVertices, newSubmeshTriangles, ref weights);
		newUv1 = DistributeUV(weights);

		newMesh.name = mesh.name + " sme";

		if (newVertices.Count > 65000)
		{
			newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			//Debug.Log("Using 32bit index buffers. Vertex count: "+ newVertices.Count);
		}
		else {
			newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
			//Debug.Log("Using 16bit index buffers. Vertex count: " + newVertices.Count);
		}

		newMesh.SetVertices(newVertices);
		newMesh.subMeshCount = mesh.subMeshCount;

		for (int s = 0; s < newSubmeshTriangles.Count; s++)
		{
			newMesh.SetTriangles(newSubmeshTriangles[s].ToArray(), s);
		}

		newMesh.SetUVs(0, newUv0);
		newMesh.SetUVs(1, newUv1);
		newMesh.SetNormals(newNormals);
		newMesh.SetTangents(newTangents);
		newMesh.boneWeights = newBoneWeights.ToArray();
		newMesh.bindposes = bindPoses.ToArray();

		//Debug.Log("Converted mesh");
		return newMesh;
	}

	private static List<Vector2> DistributeUV(float[][] weights)
	{
		List<Vector2> uv = new List<Vector2>();

		float tLeft = 0.0f;
		float bLeft = 0.0f;

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j += 2)
			{
				uv.Add(new Vector2(bLeft, 0f));
				uv.Add(new Vector2(tLeft, 1f));
				tLeft += weights[i][j] * 2f;
				uv.Add(new Vector2(tLeft, 1f));

				if (j + 1 >= weights[i].Length)
					continue;

				uv.Add(new Vector2(bLeft, 0f));
				uv.Add(new Vector2(tLeft, 1f));
				bLeft += weights[i][j + 1] * 2f;
				uv.Add(new Vector2(bLeft, 0f));
			}
		}

		return uv;
	}

	private static void WeightTriangles(List<Vector3> verts, List<List<int>> tris, ref float[][] weights)
	{
		float totalSize = 0.0f;

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				totalSize += weights[i][j] = TriangleSize(verts[tris[i][j * 3]], verts[tris[i][j * 3 + 1]], verts[tris[i][j * 3 + 2]]);
			}
		}

		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				weights[i][j] /= totalSize;
			}
		}
	}

	public static float TriangleSize(Vector3 V1, Vector3 V2, Vector3 V3)
	{
		Vector3 vectorBuffer = Vector3.Cross(V1 - V2, V1 - V3);
		return vectorBuffer.magnitude * 0.5f;
	}
}
