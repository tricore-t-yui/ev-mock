using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステート管理クラス
/// </summary>
public class PlayerStateController : StateMachineBehaviour
{
    GameObject rayObject = default;                     // レイに当たったオブジェクト

    [SerializeField]
    Transform player = default;                         // プレイヤー
    [SerializeField]
    CapsuleCollider collider = default;                 // プレイヤーのコライダー
    [SerializeField]
    PlayerBrethController brethController = default;    // 息管理クラス
    [SerializeField]
    PlayerDoorController doorController = default;      // ドア管理クラス
    [SerializeField]
    PlayerHideController hideController = default;      // 隠れる管理クラス

    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;                // ダッシュキー
    [SerializeField]
    KeyCode squatKey = KeyCode.LeftCommand;             // しゃがみキー
    [SerializeField]
    KeyCode stealthKey = KeyCode.LeftControl;           // 忍び足キー
    [SerializeField]
    KeyCode deepBreathKey = KeyCode.Space;              // 深呼吸キー

    // ステートに入っている間
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 方向キーが押されている時
        if (GetDirectionKey() && (!doorController.enabled || doorController.enabled && animator.GetBool("Dash")))
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        // しゃがんだ時
        if (Input.GetKey(squatKey))
        {
            animator.SetBool("Squat", true);
            animator.SetBool("StandUp", false);
        }
        else
        {
            animator.SetBool("StandUp", true);
            animator.SetBool("Squat", false);
        }

        // 息が切れているときはダッシュと忍び歩きができない
        if (brethController.NowAmount <= 0)
        {
            animator.SetBool("Brethlessness", true);
        }

        // 息切れ状態じゃなかったら
        if(!animator.GetBool("Brethlessness"))
        {
            animator.SetBool("Brethlessness",false);
            // 忍び歩き時
            if (Input.GetKey(stealthKey))
            {
                animator.SetBool("Stealth", true);
                animator.SetBool("BrethConsume", true);
            }
            else
            {
                animator.SetBool("Stealth", false);
                animator.SetBool("BrethConsume", false);
            }

            // ダッシュ時
            if (Input.GetKey(dashKey))
            {
                animator.SetBool("Dash", true);
                animator.SetBool("StandUp", true);

                animator.SetBool("Squat", false);
                animator.SetBool("Stealth", false);
            }
            else
            {
                animator.SetBool("Dash", false);
            }

            // 深呼吸
            if (Input.GetKey(deepBreathKey))
            {
                animator.SetBool("DeepBreath", true);
            }
            else
            {
                animator.SetBool("DeepBreath", false);
            }
        }

        // マウスが押された状態で
        if (Input.GetMouseButton(0))
        {
            // 目の前にドアがあるならドアアクション開始
            if (ObjectLayer() == LayerMask.NameToLayer("Door"))
            {
                if (!animator.GetBool("DoorOpen"))
                {
                    doorController.SetInfo(rayObject, animator.GetBool("Dash"));
                    animator.SetBool("DoorOpen", true);
                }
            }
            // 隠れるポイントがあるなら隠れるアクション開始
            else if (ObjectLayer() == LayerMask.NameToLayer("Hide") && animator.GetBool("Brethlessness"))
            {
                if (!animator.GetBool("Hide"))
                {
                    hideController.Init(rayObject);
                    animator.SetBool("Hide", true);
                    animator.SetBool("BrethConsume", true);
                }
            }
        }
        else
        {
            animator.SetBool("DoorOpen", false);
            animator.SetBool("Hide", false);
        }
    }

    /// <summary>
    /// 方向キー検知
    /// </summary>
    /// <returns>方向キーのどれか１つが押されたかどうか</returns>
    bool GetDirectionKey()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移動方向のレイに当たったオブジェクトのレイヤーを返す
    /// </summary>
    /// <returns>当たったオブジェクトのレイヤー</returns>
    int ObjectLayer()
    {
        // レイのスタート位置
        Vector3 start = player.position;

        // レイの向き
        Vector3 dir = player.forward;

        // レイの距離
        float distance = collider.radius * 3.5f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;

        // レイ作成
        Ray ray = new Ray(start, dir);
        RaycastHit hit = default;

        // デバック用ライン
        Debug.DrawLine(start, start + (dir * distance), Color.red);

        // レイに当たったらオブジェクトのレイヤー、外れていたら-1を返す
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            rayObject = hit.collider.gameObject;
            return hit.collider.gameObject.layer;
        }
        else
        {
            return -1;
        }
    }
}
