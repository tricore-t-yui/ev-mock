using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れるのをやめる時のステート
/// </summary>
public class HideObjectOutState : StateMachineBehaviour
{
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    bool isPlayer = default;                        // プレイヤーかどうか

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら初期化を始める
        if (isPlayer)
        {
            hideController.EndAction();
        }
    }
}
