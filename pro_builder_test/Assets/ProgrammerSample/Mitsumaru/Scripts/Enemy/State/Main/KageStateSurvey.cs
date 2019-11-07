using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KageStateSurvey : MonoBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    [SerializeField]
    EnemyParameterIdList enemyParameterIdList = default;

    /// <summary>
    /// ステート開始
    /// </summary>
    public void StateEnter()
    {
        // 敵を一時停止させる
        navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public void StateUpdate()
    {
       
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public void StateExit()
    {
        // あたりを見渡したら移動を再開させる
        navMeshAgent.isStopped = false;
    }
}
