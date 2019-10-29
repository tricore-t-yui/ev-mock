using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

/// <summary>
/// エネミーの徘徊（決められた複数の座標をリストの順番に移動し続ける）
/// </summary>
[ExecuteInEditMode]
public class EnemyWandering : StateMachineBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // 移動スピード
    [SerializeField]
    float moveSpeed = default;

    // 目標位置のリスト
    [SerializeField]
    List<Vector3> targetPositions = default;

    // 現在の目標位置のリスト番号
    int currentIndex = 0;

    /// <summary>
    /// ステート開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 移動スピードをセット
        navMeshAgent.speed = moveSpeed;
        // 一番最初の位置を設定
        navMeshAgent.SetDestination(GetNextTargetPos());
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
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
}
