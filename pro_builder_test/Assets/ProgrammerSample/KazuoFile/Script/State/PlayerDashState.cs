using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダッシュステート
/// </summary>
public class PlayerDashState : StateMachineBehaviour
{
    [SerializeField]
    PlayerEventCaller eventCaller = default;    // プレイヤーのイベント呼び出しクラス
    [SerializeField]
    PlayerStateSwitcher stateSwitcher = default;// プレイヤーのステート変更条件クラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // しゃがみを解除する
        eventCaller.Invoke(PlayerEventCaller.EventType.SQUATEND);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // イベント処理呼び出し
        eventCaller.Invoke(PlayerEventCaller.EventType.DASH);

        // 各ステートの検知
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.WALK);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.DASH);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.STEALTH);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.DOOROPEN);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.HIDE);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.BREATHLESSNESS);
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventCaller.Invoke(PlayerEventCaller.EventType.DASHEND);
    }
}
