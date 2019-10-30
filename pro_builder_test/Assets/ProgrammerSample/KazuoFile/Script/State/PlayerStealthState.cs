using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 忍び歩きステート
/// </summary>
public class PlayerStealthState : StateMachineBehaviour
{
    [SerializeField]
    PlayerEventCaller eventCaller = default;    // プレイヤーのイベント呼び出しクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){ }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.STEALTH);
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.STEALTHEND);
    }
}
