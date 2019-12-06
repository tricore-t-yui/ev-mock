using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロッカーに入っている間のステート
/// </summary>
public class LockerLoopState : StateMachineBehaviour
{
    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 息止めキーをおされたら息止め開始
        if (Input.GetKey(KeyCode.E))
        {
            animator.SetBool("Stealth", true);
        }

        // マウスの入力が途切れたら隠れるのをやめる
        if (!Input.GetMouseButton(0) && !animator.GetBool("DragOut"))
        {
            animator.SetTrigger("LockerOut");
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
