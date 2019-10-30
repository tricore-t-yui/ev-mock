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
    List<Vector3> targetPositions = default;

    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    /// <summary>
    /// デバッグ用表示
    /// </summary>
    void OnDrawGizmos()
    {
        // それぞれの目標位置を線で結ぶ
        for (int i = 0; i < targetPositions.Count; i++)
        {
            DrawLineToTargetPos(i);
        }

        Gizmos.DrawSphere(navMeshAgent.destination, 1);
    }

    /// <summary>
    /// 現在と次の目標位置を線でつなぐ
    /// </summary>
    /// <param name="nextIndex">次の目標位置のリストの番号</param>
    void DrawLineToTargetPos(int nextIndex)
    {
        Color lineColor;
        // 最初の線だけ黄色にする
        if (nextIndex == 0)
        {
            lineColor = Color.yellow;
        }
        // それ以外は赤にする
        else
        {
            lineColor = Color.red;
        }

        // リストの末尾にきた場合は、末尾と先頭でつなぐ
        if (nextIndex == targetPositions.Count - 1)
        {
            // 線を表示
            Debug.DrawLine(targetPositions[nextIndex], targetPositions[0], lineColor);
        }
        else
        {
            // 線を表示
            Debug.DrawLine(targetPositions[nextIndex], targetPositions[nextIndex + 1], lineColor);
        }
    }
#endif
}
