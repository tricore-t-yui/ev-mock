using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 抽選の割合をパラメータに渡す
        animator.SetFloat("LoiteringBehaviorRate", behaviourRate);

        // ０～１００のランダム値を取得
        float num = Random.Range(0, 100);

        // ランダム値が割合を超えていたら
        if (num > behaviourRate)
        {
            // 徘徊中の専用のアクションを開始する
            animator.SetTrigger("LoiteringOtherActionStart");
        }
    }
}
