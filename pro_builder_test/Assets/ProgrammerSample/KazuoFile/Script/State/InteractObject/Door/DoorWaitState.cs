using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドアの待機ステート
/// </summary>
public class DoorWaitState : StateMachineBehaviour
{
    Vector3 initPos = Vector3.zero; // 初期位置
    bool isInit = false;            // 初期化フラグ

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // はじめに初期位置を取得
        if (!isInit)
        {
            isInit = true;
            initPos = animator.gameObject.transform.position;
        }
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 待機中は初期位置
        animator.gameObject.transform.position = initPos;
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
