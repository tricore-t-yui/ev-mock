using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ時のステート
/// </summary>
public class DamageState : StateMachineBehaviour
{
    PlayerDamageController damageController = default;  // ダメージリアクションクラス


    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // クラスを取得
        damageController = animator.gameObject.GetComponent<PlayerDamageController>();
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            damageController.EndBlowAway();
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("StandUp");
        animator.SetBool("DamageEnd", true);
    }
}
