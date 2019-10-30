using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 待機時のステート
/// </summary>
public class PlayerWaitState : StateMachineBehaviour
{
    [SerializeField]
    PlayerEventCaller eventCaller = default;    // プレイヤーのイベント呼び出しクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Move", false);
        animator.SetBool("Dash", false);
        animator.SetBool("Stealth", false);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.WAIT);
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.WAITEND);
    }
}
