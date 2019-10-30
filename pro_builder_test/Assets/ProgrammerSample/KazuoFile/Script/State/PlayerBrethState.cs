using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 息管理ステート
/// </summary>
public class PlayerBrethState : StateMachineBehaviour
{
    [SerializeField]
    PlayerBrethController brethController = default;    // 息管理クラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        brethController.StateUpdate();
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
