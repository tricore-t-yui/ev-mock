using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間の徘徊ルート
/// </summary>
[CreateAssetMenu]
public class MoveCheckPointList : ScriptableObject
{
    // 徘徊ルートのチェックポイント
    [SerializeField]
    List<Vector3> checkPoints = default;

    public Vector3 this[int index]
    {
        get
        {
            return checkPoints[index];
        }
    }

    public int Count
    {
        get
        {
            return checkPoints.Count;
        }
    }
}
