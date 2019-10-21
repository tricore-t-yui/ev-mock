using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// エネミーの通常移動（決められた複数の座標をリストの順番に移動し続ける）
/// </summary>
public class EnemyMover : MonoBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // 目標位置のリスト
    [SerializeField]
    List<Vector3> targetPositions = default;

    // 移動スピード
    [SerializeField]
    float moveSpeed = default;

    // 現在の目標位置のリスト番号
    int currentIndex = 0;

    /// <summary>
    /// 開始
    /// </summary>
    void OnEnable()
    {
        // 移動スピードをセット
        navMeshAgent.speed = moveSpeed;
        // 一番最初の位置を設定
        navMeshAgent.SetDestination(GetNextTargetPos());
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 移動が完了したら次の目標位置を設定する
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            // 次の目標位置を取得
            Vector3 nextTargetPos = GetNextTargetPos();
            // 目標位置をセット
            navMeshAgent.SetDestination(nextTargetPos);
        }
    }

    /// <summary>
    /// 次の目標位置を取得
    /// </summary>
    /// <returns>次の目標位置</returns>
    Vector3 GetNextTargetPos()
    {
        // リストの末尾まできていたら先頭に戻る
        if (currentIndex == targetPositions.Count - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        // 次の目標位置を返す
        return targetPositions[currentIndex];
    }

    /// <summary>
    /// Sceneビューのみに表示
    /// </summary>
    void OnDrawGizmos()
    {
        // それぞれの目標位置を線で結ぶ
        for (int i = 0; i < targetPositions.Count; i++)
        {
            DrawLineToTargetPos(i);
        }
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
            Debug.DrawLine(targetPositions[nextIndex], targetPositions[0],lineColor);
        }
        else
        {
            // 線を表示
            Debug.DrawLine(targetPositions[nextIndex], targetPositions[nextIndex + 1],lineColor);
        }
    }
}
