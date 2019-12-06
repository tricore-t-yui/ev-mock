using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerStealthState : StateMachineBehaviour
{
    PlayerHideController hideController = default;  // 隠れるアクションクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れるアクションクラス取得
        hideController = animator.gameObject.GetComponent<PlayerHideController>();

        // 息止め開始
        hideController.SetIsStealth(true);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 息止めキーが押されていなかったら息止め解除
        if (!Input.GetKey(KeyCode.E))
        {
            animator.SetBool("Stealth", false);
        }

        // マウスの入力が途切れたら隠れるのをやめる
        if (!Input.GetMouseButton(0) && stateInfo.normalizedTime > 1.0f)
        {
            animator.SetTrigger("LockerOut");
            hideController.SetIsStealth(false);
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Stealth", false);
        hideController.SetIsStealth(false);
    }
}
