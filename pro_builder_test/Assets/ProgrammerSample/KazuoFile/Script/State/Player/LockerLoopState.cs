using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロッカーに入っている間のステート
/// </summary>
public class LockerLoopState : StateMachineBehaviour
{
    PlayerHideController hideController = default;  // 隠れるアクションクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れるアクションクラス取得
        hideController = animator.gameObject.GetComponent<PlayerHideController>() ?? hideController;
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 息止めキーをおされたら息止め開始
        if (hideController.IsHoldBreathKey() && !hideController.IsBreathlessness())
        {
            animator.SetBool("Stealth", true);
        }

        // マウスの入力が途切れたら隠れるのをやめる
        if (!hideController.IsHideKey() && !animator.GetBool("DragOut"))
        {
            animator.SetTrigger("LockerOut");
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
