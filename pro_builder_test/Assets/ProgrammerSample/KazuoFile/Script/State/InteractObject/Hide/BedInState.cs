using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 隠れている時のステート
/// </summary>
public class BedInState : StateMachineBehaviour
{
    PlayerHideController hideController = default;  // 隠れるアクションクラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れるアクションクラス取得
        hideController = animator.gameObject.GetComponent<PlayerHideController>();
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.15f)
        {
            // ベッドに向かって座標移動
            Vector3 exitVec = (hideController.HideObj.transform.position - animator.gameObject.transform.position).normalized;
            animator.gameObject.transform.position += new Vector3(exitVec.x, 0, exitVec.z) * 0.015f;

            // マウスの入力が途切れたら隠れるのをやめる
            if (!Input.GetMouseButton(0) && stateInfo.normalizedTime > 1.0f)
            {
                animator.SetTrigger("BedOut");
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
