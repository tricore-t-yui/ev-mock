using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// エネミーの状態を状況に応じて切り替える
/// </summary>
public class EnemyStateSwitcher : StateMachineBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // 徘徊に戻るポイントを管理したクラス
    [SerializeField]
    EnemyReturnPointList returnPointList = default;

    // エネミーの視界判定クラス
    [SerializeField]
    EnemyVisibility enemyVisibility = default;

    /// <summary>
    /// 更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーを見つけているかどうかのフラグ
        animator.SetBool("IsPlayerVisibility",enemyVisibility.IsPlayerDiscoverStay());

        // プレイヤーを見失ったら
        if (enemyVisibility.IsPlayerDiscoverExit())
        {
            // プレイヤーの捜索フラグをオンにする
            animator.SetBool("IsPlayerSearching",true);
        }

        // プレイヤーの捜索中
        if (animator.GetBool("IsPlayerSearching"))
        {
            // 捜索を諦めるポイントまできたら
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                // 捜索中のフラグをオフにする
                animator.SetBool("IsPlayerSearching", false);
            }
        }
    }
}
