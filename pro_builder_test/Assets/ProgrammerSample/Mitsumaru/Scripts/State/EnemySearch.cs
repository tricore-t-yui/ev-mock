using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// プレイヤーを見失ったあと探す
/// </summary>
public class EnemySearch : StateMachineBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    [SerializeField]
    EnemyReturnPointList enemyReturnPointList = default;

    /// <summary>
    /// ステート開始
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 一番近いポイントを取得
        ReturnPointData returnPointData = enemyReturnPointList.GetNearReturnPoint();
        // 目標位置をそこに設定
        navMeshAgent.SetDestination(returnPointData.position);
    }
}
