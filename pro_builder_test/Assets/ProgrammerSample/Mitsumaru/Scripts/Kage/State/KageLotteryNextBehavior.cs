using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParameterType = KageAnimParameterList.ParameterType;
using UnityEngine.AI;

/// <summary>
/// 影人間の徘徊中にそのまま徘徊を続けるかその他の行動に移行するかの抽選を行う
/// </summary>
public class KageLotteryNextBehavior : StateMachineBehaviour
{
    // 移動とその他行動の抽選割合
    [SerializeField]
    [Range(0, 100)]
    [Header("[move : other]")]
    float behaviourRate = 60;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // ナビメッシュによる移動を停止
        navMesh.isStopped = true;

        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 抽選の割合をパラメータに渡す
        animParameterList.SetFloat(ParameterType.loiteringBehaviorRate, behaviourRate);

        // ０～１００のランダム値を取得
        float num = Random.Range(0, 100);

        // ランダム値が割合を超えていたら
        if (num > behaviourRate)
        {
            // 徘徊中の専用のアクションを開始する
            animParameterList.SetTrigger(ParameterType.loiteringOtherActionStart);
            //次の行動の派生を再生するフラグを立てる。
            animator.SetBool("isPlayingNextBehavior", true);
        }
        else
        {
            animParameterList.SetBool(ParameterType.isLoiteringMove, true);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュによる移動を再開
        navMesh.isStopped = false;
    }
}
