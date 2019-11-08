using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ終了ステート
/// </summary>
public class DamageEndState : StateMachineBehaviour
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
        animator.SetBool("DamageEnd", true);
        animator.ResetTrigger("StandUp");
    }
}
