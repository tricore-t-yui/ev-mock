using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// プレイヤーの追跡を行うエネミー
/// </summary>
public class EnemyChaser : StateMachineBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // プレイヤー
    [SerializeField]
    Transform player = default;

    // 移動スピード
    [SerializeField]
    float moveSpeed = default;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 移動スピードをセット
        navMeshAgent.speed = moveSpeed;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 追跡対象の位置をセット
        navMeshAgent.SetDestination(player.position);
    }
}
