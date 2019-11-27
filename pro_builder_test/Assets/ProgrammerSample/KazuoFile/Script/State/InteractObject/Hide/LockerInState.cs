using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロッカーに入る時のステート
/// </summary>
public class LockerInState : StateMachineBehaviour
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
        // マウスの入力が途切れたら隠れるのをやめる
        if (stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("Close", true);
            if (!Input.GetMouseButton(0))
            {
                animator.SetTrigger("LockerOut");
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}