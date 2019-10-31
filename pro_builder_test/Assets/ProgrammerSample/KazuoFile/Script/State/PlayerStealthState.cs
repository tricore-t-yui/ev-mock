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
    [SerializeField]
    PlayerStateSwitcher stateSwitcher = default;// プレイヤーのステート変更条件クラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){ }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // イベント処理呼び出し
        eventCaller.Invoke(PlayerEventCaller.EventType.STEALTH);

        // 各ステートの検知
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.WALK);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.DASH);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.STEALTH);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.SQUAT);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.DOOROPEN);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.HIDE);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.BREATHLESSNESS);
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // イベント終了処理呼び出し
        eventCaller.Invoke(PlayerEventCaller.EventType.STEALTHEND);
    }
}
