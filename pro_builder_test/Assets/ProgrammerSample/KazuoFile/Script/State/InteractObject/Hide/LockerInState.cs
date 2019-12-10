using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロッカーに入る時のステート
/// </summary>
public class LockerInState : StateMachineBehaviour
{
    PlayerHideController hideController = default;          // 隠れるアクションクラス
    HideObjectController hideObjectController = default;    // 隠れるオブジェクトクラス

    [SerializeField]
    bool isPlayer = default;                                // プレイヤーかどうか

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isPlayer)
        {
            // 隠れるアクションクラス取得
            hideController = animator.gameObject.GetComponent<PlayerHideController>() ?? hideController;
        }
        else
        {
            // 隠れるオブジェクトクラス取得
            hideObjectController = animator.gameObject.GetComponent<HideObjectController>() ?? hideObjectController;
        }
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // マウスの入力が途切れたら隠れるのをやめる
        if (stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("Close", true);
            if (isPlayer)
            {
                if (!hideController.IsHideKey())
                {
                    animator.SetTrigger("LockerOut");
                }
            }
            else
            {
                if (!hideObjectController.IsHideKey())
                {
                    animator.SetTrigger("LockerOut");
                }
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}