using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ベッドでの攻撃処理
/// </summary>
public class KageStateAttackFromBed : StateMachineBehaviour
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

    Transform colliderParent = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        colliderParent = animator.transform.Find("Collider") ?? colliderParent;
        colliderParent.gameObject.SetActive(false);

        // 影人間を停止させる
        navMesh.isStopped = true;

        // ダメージイベントのクラスを取得
        damageEvent = FindObjectOfType<PlayerDamageEvent>() ?? damageEvent;

        // ダメージイベントを呼ぶ
        damageEvent.Invoke(animator.transform, 50);
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ここで攻撃フラグをオフにする
        // NOTE:なぜか２回続けてフラグがtrueになるため
        animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttack, false);
        animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttackFromLocker, false);
        animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttackFromBed, false);

        colliderParent.gameObject.SetActive(true);
    }
}
