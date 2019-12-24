﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドアの開閉ステート
/// </summary>
public class DoorOpenState : StateMachineBehaviour
{
    [SerializeField]
    float openFrame = 15;                                   // ドアが自動で開くフレーム
    [SerializeField]
    bool isPlayer = false;                                  // このアニメーターの親がプレイヤーかどうか

    PlayerDoorController playerDoorController = default;    // プレイヤードア開閉クラス
    DoorController doorController = default;                // ドア管理クラス

    float flame = 0;                                        // アニメーションのフレーム

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isPlayer)
        {
            // プレイヤードア開閉クラス取得
            playerDoorController = animator.gameObject.GetComponent<PlayerDoorController>() ?? playerDoorController;
        }
        else
        {
            // ドア管理クラス取得
            doorController = animator.gameObject.GetComponent<DoorController>() ?? doorController;
        }

        // リセット
        animator.SetFloat("DoorOpenSpeed", 0);
        animator.ResetTrigger("AutoDoorClose");
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションの進んだフレームをプラスしてドア開閉
        flame += UpdateAnimationSpeed();
        animator.SetFloat("DoorOpenSpeed", UpdateAnimationSpeed());

        // フレームが0以下の時(閉じきっている状態でなお、後ろを入力した時)
        if (flame < 0)
        {
            // ドアから離れる
            animator.SetTrigger("AutoDoorClose");
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら閉じたフラグを立てる
        if (isPlayer)
        {
            animator.SetBool("DoorEnd", true);

            // フレームが0以上(開け切って終わった)時はエリアの番号を変更
            if (flame > 0)
            {
                playerDoorController.ChangeAreaName();
            }
        }

        // トリガーとフレームのリセット
        animator.ResetTrigger("DoorOpen");
        flame = 0;
    }

    /// <summary>
    /// アニメーションを進める
    /// </summary>
    float UpdateAnimationSpeed()
    {
        // 自動で開けるフレームを超えたら自動で開ける
        if (flame >= openFrame)
        {
            return 0.5f;
        }
        else
        {
            if (isPlayer)
            {
                // マウスを押している間はキー操作でマウスが離れたら自動で閉める
                if (playerDoorController.IsDoorKey())
                {
                    return DoorOpen();
                }
            }
            else
            {
                // マウスを押している間はキー操作でマウスが離れたら自動で閉める
                if (doorController.IsDoorKey())
                {
                    return DoorOpen();
                }
            }
        }

        return -0.5f;
    }

    /// <summary>
    /// ドア開閉
    /// </summary>
    float DoorOpen()
    {
        if (isPlayer)
        {
            // 開ける
            if (playerDoorController.GetOpenStick().y > 0)
            {
                return 0.5f;
            }
            // 閉める
            if (playerDoorController.GetOpenStick().y < 0)
            {
                return -0.5f;
            }
        }
        else
        {
            // 開ける
            if (doorController.GetOpenStick().y > 0)
            {
                return 0.5f;
            }
            // 閉める
            if (doorController.GetOpenStick().y < 0)
            {
                return -0.5f;
            }
        }
       
        return 0;
    }
}