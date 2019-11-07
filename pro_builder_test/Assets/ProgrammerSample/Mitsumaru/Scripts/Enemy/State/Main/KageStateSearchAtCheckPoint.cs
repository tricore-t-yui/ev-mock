using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KageStateSearchAtCheckPoint : MonoBehaviour
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
    public void StateEnter()
    {
        // 一番近いポイントを取得
        ReturnPointData returnPointData = enemyReturnPointList.GetNearReturnPoint();
        // 目標位置をそこに設定
        navMeshAgent.SetDestination(returnPointData.position);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public void StateUpdate()
    {
        // チェックポイントまできたら捜索を諦めて徘徊に戻る
        if (navMeshAgent.remainingDistance < 0.5f)
        {
            enemyParameterIdList.SetBool(EnemyParameterIdList.ParameterType.IsPlayerSearching, false);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public void StateExit()
    {

    }
}
