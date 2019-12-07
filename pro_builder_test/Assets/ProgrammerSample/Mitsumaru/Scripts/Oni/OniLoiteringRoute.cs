using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鬼の徘徊ルート
/// </summary>
[CreateAssetMenu]
public class OniLoiteringRoute : ScriptableObject
{
    // ルートのチェックポイントのリスト
    [SerializeField]
    List<Vector3> checkPoints = default;
    public IReadOnlyList<Vector3> CheckPoints => checkPoints;
}
