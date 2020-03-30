using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// デバッグ用に敵の移動ルートを表示
/// </summary>
public class EnemyRouteDrawer : MonoBehaviour
{
#if UNITY_EDITOR
    // 目標位置のリスト
    [SerializeField]
    List<MoveCheckPointList> targetPositions = default;

    [SerializeField]
    NavMeshAgent navMeshAgent = default;

#if UNITY_EDITOR
    /// <summary>
    /// デバッグ用表示
    /// </summary>
    void OnDrawGizmos()
    {
        // それぞれの目標位置を線で結ぶ
        foreach(MoveCheckPointList checkPoint in targetPositions)
        {
            DrawLineToTargetPos(checkPoint);
        }
    }
#endif

    /// <summary>
    /// 現在と次の目標位置を線でつなぐ
    /// </summary>
    void DrawLineToTargetPos(MoveCheckPointList checkPoint)
    {
        for (int i = 0; i < checkPoint.Count; i++)
        {
            // リストの末尾にきた場合は、末尾と先頭でつなぐ
            if (i == checkPoint.Count - 1)
            {
                // 線を表示
                Debug.DrawLine(checkPoint[i], checkPoint[0], Color.red);
            }
            else
            {
                // 線を表示
                Debug.DrawLine(checkPoint[i], checkPoint[i + 1], Color.red);
            }
        }
    }
#endif
}
