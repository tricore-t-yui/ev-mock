using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの体勢のステート(立つ、しゃがみ)
/// </summary>
public class PlayerPostureState : StateMachineBehaviour
{
    [SerializeField]
    PlayerEventCaller eventCaller = default;    // プレイヤーのイベント呼び出しクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Squat"))
        {
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUAT);
        }
        else
        {
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUATEND);
        }
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
