using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 隠れていない状態での攻撃処理
/// </summary>
public class OniStateAttackNormal : StateMachineBehaviour
{
    // ダメージ量
    [SerializeField]
    float damage = 0;

    // ダメージイベント
    PlayerDamageEvent damageEvent = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.Log("Attack");

        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動を止める
        navMesh.isStopped = true;

        // ダメージイベントのクラスを取得
        damageEvent = FindObjectOfType<PlayerDamageEvent>() ?? damageEvent;
        // プレイヤーのダメージイベントを呼ぶ
        damageEvent?.Invoke(animator.transform, damage);

    }
}
