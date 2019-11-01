using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 近くのチェックポイントに向かいながらプレイヤーを捜索
/// </summary>
public class EnemySearchAtCheckPoint : StateMachineBehaviour
{
    [SerializeField]
    EnemyParameterIdList enemyParameterIdList = default;

    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    [SerializeField]
    EnemyReturnPointList enemyReturnPointList = default;

    /// <summary>
    /// ステート開始
    /// <summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 一番近いポイントを取得
        ReturnPointData returnPointData = enemyReturnPointList.GetNearReturnPoint();
        // 目標位置をそこに設定
        navMeshAgent.SetDestination(returnPointData.position);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animatorStateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // チェックポイントまできたら捜索を諦めて徘徊に戻る
        if (navMeshAgent.remainingDistance < 0.5f)
        {
            enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerSearching, false);
        }
    }
}
