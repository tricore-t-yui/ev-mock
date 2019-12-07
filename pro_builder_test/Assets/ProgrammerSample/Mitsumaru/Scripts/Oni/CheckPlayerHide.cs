using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerHide : StateMachineBehaviour
{
    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーのハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーがハイドポイントに入ったら
        if (playerHideController.IsHideLocker || playerHideController.IsHideBed)
        {
            // ステートを変更
            animator.SetBool("isPlayerHide", true);
        }
        else
        {
            // ステートを変更
            animator.SetBool("isPlayerHide", false);
        }
    }
}
