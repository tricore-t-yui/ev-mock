using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れるステート
/// </summary>
public class PlayerHideState : StateMachineBehaviour
{
    [SerializeField]
    PlayerEventCaller eventCaller = default;    // プレイヤーのイベント呼び出しクラス
    [SerializeField]
    PlayerStateSwitcher stateSwitcher = default;// プレイヤーのステート変更条件クラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // イベント処理呼び出し
        eventCaller.Invoke(PlayerEventCaller.EventType.HIDE);

        // 各ステートの検知
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.HIDE);
        stateSwitcher.CheckState(PlayerStateSwitcher.StateType.BREATHLESSNESS);

        // 息切れを起こしていたらゲームオーバー
        if (animator.GetBool("Brethlessness"))
        {
            Debug.Log("ゲームオーバー");
        }
    }

    /// <summary>
    /// ステートに出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // イベント終了処理呼び出し
        eventCaller.Invoke(PlayerEventCaller.EventType.HIDEEND);
    }
}
