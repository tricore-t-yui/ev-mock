//////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////
//Velocities
//////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////
//World Space Sim
//////////////////////////////////////////////////////////////////////////////////////////

float4 GetNormalOnMeshWorld(sampler2D normals, float _Speed, float2 uv, float4x4 _EmitterMatrix)
{
	float3 Normal = normalize(tex2D(normals, float2(Random(uv), Random(uv+.1))));
	return mul(_EmitterMatrix, float4(Normal*_Speed,0));
}

float4 GetRandomVelocityBoxWorld(float2 uv, float _Speed, float4x4 _EmitterMatrix)
{
	float3 Position = float3( 0, 0, _Speed);
	return mul(_EmitterMatrix, float4(Position,0));
}

float4 GetRandomVelocityConeWorld(float2 uv, float radius1, float radius2, float length, float _StartSpeed, float4x4 _EmitterMatrix)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0)) * Random(uv+.2);
	float3 Position1 = StartPosition * radius1;
	float3 Position2 = StartPosition * radius2;
	Position2.z += length;

	Position1 = normalize(Position2 - Position1) * _StartSpeed;

	return mul(_EmitterMatrix, float4(Position1,0));
}

float4 GetRandomVelocityEdgeWorld(float2 uv, float _Speed, float4x4 _EmitterMatrix)
{
	float3 Position = float3( 0, _Speed, 0);
	return mul(_EmitterMatrix, float4(Position,0));
}

float4 GetRandomVelocityDiscSurfaceWorld(float2 uv, float _Speed, float4x4 _EmitterMatrix)
{
	float3 Position = normalize(float3(Random(uv) * 2 - 1, 0, Random(uv+.1) * 2 - 1)) * _Speed;
	return mul(_EmitterMatrix, float4(Position,0));
}

float4 GetRandomVelocityHemiSphereVolumeWorld(float2 uv, float _Speed, float4x4 _EmitterMatrix)
{
	float3 Position = float3( normalize( float3( Random(uv) * 2 - 1, Random(uv+.1), Random(uv+.2) * 2 - 1))) * _Speed;
	return mul(_EmitterMatrix, float4(Position,0));
}

//////////////////////////////////////////////////////////////////////////////////////////
//Local Space Sim
//////////////////////////////////////////////////////////////////////////////////////////

float4 GetNormalOnMeshLocal(sampler2D normals, float _Speed, float2 uv)
{
	float3 Normal = normalize(tex2D(normals, float2(Random(uv), Random(uv+.1))));
	return float4(Normal*_Speed,0);
}

float4 GetRandomVelocityBoxLocal(float2 uv, float _Speed)
{
	float3 Position = float3( 0, 0, _Speed);
	return float4(Position,0);
}

float4 GetRandomVelocityConeLocal(float2 uv, float radius1, float radius2, float length, float _StartSpeed)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0)) * Random(uv+.2);
	float3 Position1 = StartPosition * radius1;
	float3 Position2 = StartPosition * radius2;
	Position2.z += length;

	Position1 = normalize(Position2 - Position1) * _StartSpeed;

	return float4(Position1,0);
}

float4 GetRandomVelocityEdgeLocal(float2 uv, float _Speed)
{
	float3 Position = float3( 0, _Speed, 0);
	return float4(Position,0);
}

float4 GetRandomVelocityDiscSurfaceLocal(float2 uv, float _Speed)
{
	float3 Position = normalize(float3(Random(uv) * 2 - 1, 0, Random(uv+.1) * 2 - 1)) * _Speed;
	return float4(Position,0);
}

float4 GetRandomVelocityHemiSphereVolumeLocal(float2 uv, float _Speed)
{
	float3 Position = float3( normalize( float3( Random(uv) * 2 - 1, Random(uv+.1), Random(uv+.2) * 2 - 1)	)) * _Speed;
	return float4(Position,1);
}

//////////////////////////////////////////////////////////////////////////////////////////
//Both Sim Spaces
//////////////////////////////////////////////////////////////////////////////////////////

float4 GetRandomVelocitySphere(float2 uv, float _Speed)
{
	float3 Position = normalize(float3( float3( Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1))) * _Speed;
	return float4(Position,1);
}

float4 GetRandomVelocity(float2 uv, float _Speed)
{
	float3 Position = normalize(float3( float3( Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1))) * _Speed;
	return float4(Position,1);
}


//////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////
//POSITION
//////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////
//World Space Sim
//////////////////////////////////////////////////////////////////////////////////////////

float4 GetPointOnPlaneWorld(float2 uv, float4 _EmitterParam, float4x4 _EmitterMatrix)
{
	float4 Position;
	Position.x = ((uv.x * 2) - 1) * _EmitterParam.x;
	Position.y = ((uv.y * 2) - 1) * _EmitterParam.y;
	Position.z = 0;
	return mul(_EmitterMatrix, float4(Position.xyz, 1));
}

float4 GetPointOnMeshWorld(sampler2D MeshPositions, float2 uv, float4x4 _EmitterMatrix)
{
	float3 Position = tex2D(MeshPositions, float2(Random(uv), Random(uv+.1)));
	return  mul(_EmitterMatrix, float4(Position,1));
}

float4 GetRandomPointHemiSphereVolumeWorld(float2 uv, float radius, float4x4 _EmitterMatrix)
{
	float3 Position = float3( normalize( float3( Random(uv) * 2 - 1, Random(uv+.1), Random(uv+.2) * 2 - 1)	)) * radius;
	return mul(_EmitterMatrix, float4(Position * Random(uv+.3),1));
}

float4 GetRandomPointHemiSphereSurfaceWorld(float2 uv, float radius, float4x4 _EmitterMatrix)
{
	float3 Position = normalize( float3( float3( Random(uv) * 2 - 1, Random(uv+.1), Random(uv+.2) * 2 - 1)	)) * radius;
	return mul(_EmitterMatrix, float4(Position,1));
}

float4 GetRandomPointBoxWorldVolume(float2 uv, float3 size, float4x4 _EmitterMatrix)
{
	float3 Position = float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1);
	Position *= size;
	return mul(_EmitterMatrix, float4(Position,1));
}

float4 GetRandomPointOnEdgeWorld(float2 uv, float length, float4x4 _EmitterMatrix)
{
	float3 Position = float3( Random(uv) * 2 - 1, 0, 0) * (length / 2);
	return mul(_EmitterMatrix, float4(Position,1));
}

//Cone:
//Volume
float4 GetRandomPointConeWorld(float2 uv, float radius1, float radius2, float length, float4x4 _EmitterMatrix)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0)) * Random(uv+.2);
	float3 Position1 = StartPosition * radius1;
	float3 Position2 = StartPosition * radius2;
	Position2.z += length;

	Position1 = lerp(Position1, Position2, Random(uv+.3));
	return mul(_EmitterMatrix, float4(Position1,1));
}

//Base
float4 GetRandomPointConeWorldBase(float2 uv, float radius1, float4x4 _EmitterMatrix)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0)) * Random(uv+.2);
	float3 Position1 = StartPosition * radius1;
	return mul(_EmitterMatrix, float4(Position1,1));
}

//Edge
float4 GetRandomPointConeWorldEdge(float2 uv, float radius1, float radius2, float length, float4x4 _EmitterMatrix)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0));
	float3 Position1 = StartPosition * radius1;
	float3 Position2 = StartPosition * radius2;
	Position2.z += length;

	Position1 = lerp(Position1, Position2, Random(uv+.3));

	return mul(_EmitterMatrix, float4(Position1,1));
}

//Base Edge
float4 GetRandomPointConeWorldBaseEdge(float2 uv, float radius1, float4x4 _EmitterMatrix)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0));
	float3 Position1 = StartPosition * radius1;
	return mul(_EmitterMatrix, float4(Position1,1));
}

float4 GetRandomPointOnDiscSurfaceWorld(float2 uv, float radius, float4x4 _EmitterMatrix)
{
	float3 Position = normalize(float3(Random(uv) * 2 - 1, 0, Random(uv+.1) * 2 - 1)) * Random(uv+.2) * radius;
	return mul(_EmitterMatrix, float4(Position,1));
}

float4 GetRandomPointOnDiscEdgeWorld(float2 uv, float radius, float4x4 _EmitterMatrix)
{
	float3 Position = normalize(float3(Random(uv) * 2 - 1, 0, Random(uv+.1) * 2 - 1)) * radius;
	return mul(_EmitterMatrix, float4(Position,1));
}

//////////////////////////////////////////////////////////////////////////////////////////
//Local Space Sim
//////////////////////////////////////////////////////////////////////////////////////////

float4 GetPointOnMeshLocal(sampler2D MeshPositions, float2 uv)
{
	float3 Position = tex2D(MeshPositions, float2(Random(uv), Random(uv+.1)));
	return float4(Position,1);
}

float4 GetRandomPointHemiSphereVolumeLocal(float2 uv, float radius)
{
	float3 Position = float3( normalize( float3( Random(uv) * 2 - 1, Random(uv+.1), Random(uv+.2) * 2 - 1)	)) * radius;
	return float4(Position *  Random(uv+.3),1);
}

float4 GetRandomPointHemiSphereSurfaceLocal(float2 uv, float radius)
{
	float3 Position = normalize( float3( float3( Random(uv) * 2 - 1, Random(uv+.1), Random(uv+.2) * 2 - 1)	)) * radius;
	return float4(Position,1);
}

float4 GetRandomPointBoxLocal(float2 uv, float3 size)
{
	float3 Position = float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1);
	Position *= size;
	return float4(Position,1);
}

float4 GetRandomPointOnEdgeLocal(float2 uv, float length)
{
	float3 Position = float3( Random(uv) * 2 - 1, 0, 0) * (length / 2);
	return float4(Position,1);
}

//Cone:
//Volume
float4 GetRandomPointConeLocal(float2 uv, float radius1, float radius2, float length)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0)) * Random(uv+.2);
	float3 Position1 = StartPosition * radius1;
	float3 Position2 = StartPosition * radius2;
	Position2.z += length;

	Position1 = lerp(Position1, Position2, Random(uv+.3));

	return float4(Position1,1);
}

//Edge
float4 GetRandomPointConeLocalEdge(float2 uv, float radius1, float radius2, float length)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0));
	float3 Position1 = StartPosition * radius1;
	float3 Position2 = StartPosition * radius2;
	Position2.z += length;

	Position1 = lerp(Position1, Position2, Random(uv+.3));

	return float4(Position1,1);
}

//Base
float4 GetRandomPointConeLocalBase(float2 uv, float radius1, float length)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0)) * Random(uv+.2);
	float3 Position1 = StartPosition * radius1;
	return float4(Position1,1);
}

float4 GetRandomPointConeLocalBaseEdge(float2 uv, float radius1, float length)
{
	float3 StartPosition = normalize(float3(Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, 0));
	float3 Position1 = StartPosition * radius1;
	return float4(Position1,1);
}

float4 GetRandomPointOnDiscSurfaceLocal(float2 uv, float radius)
{
	float3 Position = normalize(float3(Random(uv) * 2 - 1, 0, Random(uv+.1) * 2 - 1)) * Random(uv+.2) * radius;
	return float4(Position,1);
}

float4 GetRandomPointOnDiscEdgeLocal(float2 uv, float radius)
{
	float3 Position = normalize(float3(Random(uv) * 2 - 1, 0, Random(uv+.1) * 2 - 1)) * radius;
	return float4(Position,1);
}

//////////////////////////////////////////////////////////////////////////////////////////
//Both Sim Spaces
//////////////////////////////////////////////////////////////////////////////////////////

float4 GetRandomPositionSphereVolume(float2 uv, float radius)
{
	float3 Position = float3( normalize( float3( Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1)	)) * radius;
	return float4(Position * Random(uv+.3),1);
}

float4 GetRandomPositionSphere(float2 uv, float radius)
{
	float3 Position = float3( normalize( float3( Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1)	)) * radius;
	return float4(Position,1);
}

float3 GetRandomVectorPoint(float2 uv)
{
	float4 Vector = float4(	normalize(	float3( Random(uv) * 2 - 1, Random(uv+.1) * 2 - 1, Random(uv+.2) * 2 - 1)	), 1);
	return Vector.xyz;
}