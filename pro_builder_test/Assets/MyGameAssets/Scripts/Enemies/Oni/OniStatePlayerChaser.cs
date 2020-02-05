using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// プレイヤーを追跡する
/// </summary>
public class OniStatePlayerChaser : StateMachineBehaviour
{
    // プレイヤー
    Transform player = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーを取得
        player = GameObject.Find("Player").transform;

        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 移動速度を設定
        navMesh.speed = animator.GetFloat("moveSpeed");

        // プレイヤーの位置を目標位置に設定
        navMesh.SetDestination(player.position);
    }
}
