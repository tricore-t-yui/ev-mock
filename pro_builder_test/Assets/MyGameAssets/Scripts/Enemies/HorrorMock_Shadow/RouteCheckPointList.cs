using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RouteCheckPointList : ScriptableObject,IEnumerable
{
    [SerializeField]
    List<Vector2> checkPoints = default;
    
    public int Count { get { return checkPoints.Count; } }
    public Vector2 this[int index] { get { return checkPoints[index]; } }

    public IEnumerator GetEnumerator()
    {
        return checkPoints.GetEnumerator();
    }
}
