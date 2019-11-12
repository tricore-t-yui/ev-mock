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
    public enum StateKind
    {
        Standing = 1,   // 待機
        Loitering,      // 徘徊
    }

    // ステートの種類
    StateKind stateType = StateKind.Standing;

    // 影人間のステートのパラーメータを取得
    KageStateParameter stateParameter = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ステートパラメータを取得
        stateParameter = animator.GetComponent<KageStateParameter>() ?? stateParameter;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // パラメータをセット
        stateType = stateParameter.StateNormalOfType;

        // 指定された状態に変更
        animParameterList.SetInteger(ParameterType.normalBehaviourKindId, (int)stateType);
    }
}
