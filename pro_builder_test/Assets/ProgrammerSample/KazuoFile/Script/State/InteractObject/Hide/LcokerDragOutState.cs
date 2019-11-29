using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LcokerDragOutState : StateMachineBehaviour
{
    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れる終了
        animator.ResetTrigger("LockerIn");
        animator.ResetTrigger("LockerOut");
        animator.ResetTrigger("DragOutStandUp");
        animator.SetBool("Stealth", false);
        animator.SetBool("DragOut", false);
        animator.SetBool("HideEnd", true);
        animator.SetBool("DamageEnd", true);
    }
}
