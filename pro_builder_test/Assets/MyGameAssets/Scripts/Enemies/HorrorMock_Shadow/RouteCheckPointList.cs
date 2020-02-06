using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RouteCheckPointList : ScriptableObject
{
    [SerializeField]
    List<Vector2> checkPoints = default;
    public List<Vector2> CheckPoints => checkPoints;
}
