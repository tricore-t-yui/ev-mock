using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート管理：攻撃
/// </summary>
public class KageStateAttack : StateMachineBehaviour
{
    // ダメージ量
    [SerializeField]
    float damage = 0;

    // ダメージイベント
    PlayerDamageEvent damageEvent = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;
    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Debug.Log("Attack");

        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        navMesh.isStopped = true;

        // ダメージイベントのクラスを取得
        damageEvent = FindObjectOfType<PlayerDamageEvent>() ?? damageEvent;
        // プレイヤーのダメージイベントを呼ぶ
        damageEvent?.Invoke(animator.transform,damage);
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ここで攻撃フラグをオフにする
        // NOTE:なぜか２回続けてフラグがtrueになるため
        animParameterList.ResetTrigger(KageAnimParameterList.ParameterType.isAttack);
    }
}
