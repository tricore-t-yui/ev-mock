using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 息切れ時のステート
/// </summary>
public class PlayerBrethlessnessState : StateMachineBehaviour
{
    [SerializeField]
    PlayerEventCaller eventCaller = default;    // プレイヤーのイベント呼び出しクラス
    [SerializeField]
    PlayerBrethController brethController = default;    // プレイヤーのイベント呼び出しクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESS);

        // マックスまで回復したら
        if(brethController.NowAmount >= 100)
        {
            animator.SetBool("Brethlessness", false);
        }
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESSEND);
    }
}