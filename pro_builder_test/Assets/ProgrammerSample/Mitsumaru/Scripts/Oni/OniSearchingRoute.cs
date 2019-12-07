using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OniSearchingRoute : ScriptableObject
{
    [SerializeField]
    List<Vector3> checkPoints = default;
    public IReadOnlyList<Vector3> CheckPoints => checkPoints;
}
