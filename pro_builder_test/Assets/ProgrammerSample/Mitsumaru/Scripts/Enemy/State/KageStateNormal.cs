using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParameterType = KageAnimParameterList.ParameterType;

/// <summary>
/// 影人間のステート：通常状態
/// </summary>
public class KageStateNormal : StateMachineBehaviour
{
    /// <summary>
    /// 通常状態時のステートの種類
    /// </summary>
    public enum BehaviourKind
    {
        Standing = 1,   // 待機
        Loitering,      // 徘徊
    }

    // ステートの種類
    [SerializeField]
    BehaviourKind behaviourKind = BehaviourKind.Standing;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // パラメータクラスを取得
        animParameterList = animParameterList ?? animator.GetComponent<KageAnimParameterList>();

        // 指定された状態に変更
        animParameterList.SetInteger(ParameterType.normalBehaviourKindId, (int)behaviourKind);
    }
}
