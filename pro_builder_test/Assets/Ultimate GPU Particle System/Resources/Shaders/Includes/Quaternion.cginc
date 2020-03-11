//////////////////////////////////////////////////////////////////////////////////////////
//Quaternion math
//////////////////////////////////////////////////////////////////////////////////////////

float4 QuaternionAngleAxis(float3 axis, float angle)
{ 
  float4 qr;
  float half_angle = (angle * 0.5);
  qr.x = axis.x * sin(half_angle);
  qr.y = axis.y * sin(half_angle);
  qr.z = axis.z * sin(half_angle);
  qr.w = cos(half_angle);
  return qr;
}

float3 RotateVertex(float3 position, float3 axis, float angle)
{ 
  float4 q = QuaternionAngleAxis(axis, angle);
  float3 v = position.xyz;
  return v + 2.0 * cross(q.xyz, cross(q.xyz, v) + q.w * v);
}