using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 周りを見渡している状態
/// </summary>
public class EnemySurvey : StateMachineBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    /// <summary>
    /// ステート開始
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animatorStateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 敵を一時停止させる
        navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animatorStateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // あたりを見渡したら移動を再開させる
        navMeshAgent.isStopped = false;
    }
}
