using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// ロッカーに隠れるのをやめるステート
/// </summary>
public class LockerOutState : StateMachineBehaviour
{
    PlayerHideController hideController = default;          // 隠れるアクションクラス

    [SerializeField]
    bool isPlayer = default;                                // プレイヤーかどうか

    float exitRotationSpeed = 2;                            // 脱出方向へ向くスピード
    Quaternion exitRotation = default;                      // 脱出方向

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら
        if (isPlayer)
        {
            // 隠れるアクションクラス取得
            hideController = animator.gameObject.GetComponent<PlayerHideController>() ?? hideController;

            // アニメーション回転フラグを切る
            hideController.SetIsAnimRotation(false);

            // 脱出方向を求める
            ExitRotation();
        }
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら回転開始
        if (isPlayer)
        {
            // 求めた脱出方向に向かって回転
            if (animator.gameObject.transform.rotation.eulerAngles != exitRotation.eulerAngles && !hideController.IsAnimRotation)
            {
                Quaternion rotation = Quaternion.RotateTowards(animator.gameObject.transform.rotation, exitRotation, exitRotationSpeed);
                animator.gameObject.transform.rotation = rotation;
            }
            // 回転し終わったらアニメーションに回転を任せる
            else
            {
                hideController.SetIsAnimRotation(true);
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら初期化を始める
        if (isPlayer && !animator.GetBool("DragOut"))
        {
            animator.ResetTrigger("LockerIn");
            animator.ResetTrigger("LockerOut");
            animator.SetBool("HideEnd", true);
            animator.SetBool("Stealth", false);
        }
    }

    /// <summary>
    /// 脱出方向を求める
    /// </summary>
    void ExitRotation()
    {
        switch (hideController.HideObjDir)
        {
            case DirType.FORWARD: exitRotation = Quaternion.Euler(0, 180, 0); break;
            case DirType.BACK: exitRotation = Quaternion.Euler(0, 0, 0); break;
            case DirType.RIGHT: exitRotation = Quaternion.Euler(0, 270, 0); break;
            case DirType.LEFT: exitRotation = Quaternion.Euler(0, 90, 0); break;
        }
    }
}