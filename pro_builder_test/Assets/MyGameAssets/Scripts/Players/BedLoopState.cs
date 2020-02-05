using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベッドに隠れている間のステート
/// </summary>
public class BedLoopState : StateMachineBehaviour
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
        if (!hideController.IsHide())
        {
            animator.SetTrigger("BedOut");
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
