using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 裸足終了ステート
/// </summary>
public class BarefootEndState : StateMachineBehaviour
{
    PlayerAnimationContoller animationContoller = default;  // プレイヤーのアニメーション管理クラス

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // クラスを取得してフラグを立てる
        animationContoller = animator.gameObject.GetComponent<PlayerAnimationContoller>();
        animator.SetBool("Shoes", true);

        // 靴を非表示
        animationContoller.DisplayShoes(false);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.SHOES);
        animator.ResetTrigger("TakeOffShoes");
    }
}
