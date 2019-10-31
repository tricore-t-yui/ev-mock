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
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 一旦全てリセット
        animator.SetBool("Dash", false);
        animator.SetBool("Stealth", false);
        animator.SetBool("Squat", false);
        animator.SetBool("Hide", false);
        animator.SetBool("DoorOpen", false);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // イベント処理呼び出し
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
        // イベント終了処理呼び出し
        eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESSEND);
    }
}