using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ObjectType = PlayerHideController.HideObjectType;

/// <summary>
/// 隠れている時のステート
/// </summary>
public class HideObjectInState : StateMachineBehaviour
{
    [SerializeField]
    ObjectType type = default;                      // オブジェクトタイプ

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        // マウスの入力が途切れたら隠れるのをやめる
        if (!Input.GetMouseButton(0) && stateInfo.normalizedTime > 1.0f)
        { 
            switch (type)
            {
                case ObjectType.LOCKER: animator.SetTrigger("LockerOut"); break;
                case ObjectType.BED: animator.SetTrigger("BedOut"); break;
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
