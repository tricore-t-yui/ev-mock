using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 罠にかかっている間のステート
/// </summary>
public class TrapEndState : StateMachineBehaviour
{
    [SerializeField]
    bool isPlayer = default;                        // プレイヤーかどうか

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("TrapOperate");
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら
        if (isPlayer)
        {
            animator.SetBool("TrapEnd", true);
        };
    }
}
