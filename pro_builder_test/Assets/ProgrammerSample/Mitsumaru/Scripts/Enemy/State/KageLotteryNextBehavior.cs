using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParameterType = KageAnimParameterList.ParameterType;

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

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // パラメータクラスを取得
        animParameterList = animParameterList ?? animator.GetComponent<KageAnimParameterList>();

        // 抽選の割合をパラメータに渡す
        animParameterList.SetFloat(ParameterType.loiteringBehaviorRate, behaviourRate);

        // ０～１００のランダム値を取得
        float num = Random.Range(0, 100);

        // ランダム値が割合を超えていたら
        if (num > behaviourRate)
        {
            // 徘徊中の専用のアクションを開始する
            animParameterList.SetTrigger(ParameterType.loiteringOtherActionStart);
        }
    }
}
