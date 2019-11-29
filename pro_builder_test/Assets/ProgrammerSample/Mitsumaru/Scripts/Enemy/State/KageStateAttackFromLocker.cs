using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート：ロッカーでの攻撃
/// </summary>
public class KageStateAttackFromLocker : StateMachineBehaviour
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
    // ロッカーのアニメーター
    Animator lockerAnimator = null;

    [SerializeField]
    int lockerOpenTime = 0;
    [SerializeField]
    int damageTime = 0;
    [SerializeField]
    int lockerCloseTime = 0;

    int counter = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 影人間を停止させる
        navMesh.isStopped = true;

        // ダメージイベントのクラスを取得
        damageEvent = animator.GetComponentInChildren<PlayerDamageEvent>() ?? damageEvent;

        // ロッカーのアニメーターを取得
        lockerAnimator = GameObject.FindGameObjectWithTag("Locker").GetComponent<Animator>() ?? lockerAnimator;
    }


    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ロッカーを開ける
        if (counter == lockerOpenTime)
        {
            lockerAnimator.SetBool("DragOut", true);
        }
        // 吹き飛ばす
        else if (counter == damageTime)
        {
            damageEvent.Invoke(PlayerDamageEvent.DamageType.Normal, animator.transform.position, 50);
        }
        // ロッカーを閉める
        else if (counter == lockerCloseTime)
        {
            lockerAnimator.SetBool("DragOut", false);
        }
        counter++;
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // カウンターリセット
        counter = 0;

        // ここで攻撃フラグをオフにする
        // NOTE:なぜか２回続けてフラグがtrueになるため
        animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttack, false);
        animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttackFromLocker, false);
        animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttackFromBed, false);
    }
}
