using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドアの開閉ステート
/// </summary>
public class DoorOpenState : StateMachineBehaviour
{
    /// <summary>
    /// オブジェクトタイプ
    /// </summary>
    enum Type
    {
        DOOR,
        PLAYER,
    }

    [SerializeField]
    PlayerDoorController doorController = default;  // ドア開閉クラス
    [SerializeField]
    Type type = default;                            // オブジェクトタイプ
    [SerializeField]
    float openFrame = 15;                           // ドアが自動で開くフレーム

    float flame = 0;                                // アニメーションのフレーム

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // リセット
        flame = 0;
        animator.SetFloat("Speed", 0);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションの進んだフレームをプラス
        flame += UpdateAnimationSpeed();

        // フレームが0以下の時(閉じきっている状態でなお、後ろを入力した時)
        if (flame < 0)
        {
            // ドアから離れる
            animator.SetTrigger("Exit");
        }

        // アニメーションにスピードを適用してドア開閉
        animator.SetFloat("Speed", UpdateAnimationSpeed());
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // それぞれのトリガーをリセット
        animator.ResetTrigger("Exit");
        animator.ResetTrigger("Open");

        // プレイヤーだったら初期化を始める
        if (type == Type.PLAYER)
        {
            animator.SetBool("Reverse", false);
            doorController.EndAction();
        }
    }

    /// <summary>
    /// アニメーションを進める
    /// </summary>
    float UpdateAnimationSpeed()
    {
        // アニメーションの速度
        float animSpeed = 0;

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
        // マウスが離れたら現在のフレーム数に合わせて自動で開閉
        else
        {
            if (flame > openFrame)
            {
                animSpeed = 0.5f;
            }
            else
            {
                animSpeed = -0.5f;
            }
        }

        return animSpeed;
    }
}
