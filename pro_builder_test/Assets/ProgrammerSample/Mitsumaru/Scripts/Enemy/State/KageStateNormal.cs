using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 指定された状態に変更
        animator.SetInteger("NormalBehaviourKindId", (int)behaviourKind);
    }
}
