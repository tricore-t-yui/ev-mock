using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダッシュ時のドアの開閉ステート
/// </summary>
public class DashDoorOpenState : StateMachineBehaviour
{
    [SerializeField]
    PlayerDoorController doorController = default;  // ドア開閉クラス
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
        // それぞれのトリガーをリセット
        animator.ResetTrigger("DashOpen");

        //　プレイヤーなら反転フラグをリセットして初期化
        if (isPlayer)
        {
            animator.SetBool("Reverse", false);
            doorController.EndAction();
        }
    }
}
