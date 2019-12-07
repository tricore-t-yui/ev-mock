using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ロッカーでの攻撃処理
/// </summary>
public class OniStateAttackHideInLocker : StateMachineBehaviour
{
    // ダメージ量
    [SerializeField]
    float damage = 0;

    // ダメージイベント
    PlayerDamageEvent damageEvent = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;
    // ロッカーのアニメーター
    Animator lockerAnimator = null;

    [SerializeField]
    int lockerOpenTime = 0;
    [SerializeField]
    int damageTime = 0;

    int counter = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 影人間を停止させる
        navMesh.isStopped = true;

        // ダメージイベントのクラスを取得
        damageEvent = FindObjectOfType<PlayerDamageEvent>() ?? damageEvent;

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
            damageEvent.Invoke(animator.transform, 50);
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

        // ドアを閉める
        lockerAnimator.SetBool("DragOut", false);
    }
}
