using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerStealthState : StateMachineBehaviour
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
        // 息止めキーが押されていなかったら息止め解除
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool("Stealth", false);
            Debug.Log("5");
        }

        // マウスの入力が途切れたら隠れるのをやめる
        if (!Input.GetMouseButton(0) && stateInfo.normalizedTime > 1.0f)
        {
            animator.SetTrigger("LockerOut");
            animator.SetBool("Stealth", false);
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
