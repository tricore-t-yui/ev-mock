using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドアの開閉ステート
/// </summary>
public class DoorOpenState : StateMachineBehaviour
{
    [SerializeField]
    float openFrame = 30;                           // ドアが自動で開くフレーム
    [SerializeField]
    bool isPlayer = false;                          // このアニメーターの親がプレイヤーかどうか

    float flame = 0;                                // アニメーションのフレーム

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // リセット
        flame = 0;
        animator.SetFloat("DoorOpenSpeed", 0);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションの進んだフレームをプラス
        flame += UpdateAnimationSpeed(animator);

        // フレームが0以下の時(閉じきっている状態でなお、後ろを入力した時)
        if (flame < 0)
        {
            // ドアから離れる
            animator.SetTrigger("AutoDoorClose");
        }

        // アニメーションにスピードを適用してドア開閉
        animator.SetFloat("DoorOpenSpeed", UpdateAnimationSpeed(animator));
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // トリガーのリセット
        animator.ResetTrigger("DoorOpen");
        animator.ResetTrigger("AutoDoorClose");

        // プレイヤーだったら閉じたフラグを立てる
        if (isPlayer)
        {
            animator.SetBool("DoorEnd", true);
        }
    }

    /// <summary>
    /// アニメーションを進める
    /// </summary>
    float UpdateAnimationSpeed(Animator animator)
    {
        // アニメーションの速度
        float animSpeed = 0;

        // 自動で開けるフレームを超えたら自動で開ける
        if (flame >= openFrame)
        {
            animSpeed = 0.5f;
        }
        else
        {
            // マウスを押している間はキー操作で
            if (Input.GetMouseButton(0))
            {
                // 開ける
                if (Input.GetKey(KeyCode.W))
                {
                    animSpeed = 0.5f;
                }
                // 閉める
                else if (Input.GetKey(KeyCode.S))
                {
                    animSpeed = -0.5f;
                }
                // そのまま
                else
                {
                    animSpeed = 0;
                }
            }
            // マウスが離れたら自動で閉める
            else
            {
                animSpeed = -0.5f;
            }
        }

        return animSpeed;
    }
}
