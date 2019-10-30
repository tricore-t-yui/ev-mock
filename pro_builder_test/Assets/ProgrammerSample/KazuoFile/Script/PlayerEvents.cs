using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各イベント関数置き場
/// </summary>
public class PlayerEvents : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider collider = default;                 // プレイヤーのコライダー
    [SerializeField]
    PlayerMoveController moveController = default;      // プレイヤーの移動クラス
    [SerializeField]
    PlayerBrethController brethController = default;    // 息管理クラス
    [SerializeField]
    PlayerDoorController doorController = default;      // ドア開閉クラス

    /// <summary>
    /// 移動
    /// </summary>
    public void Move()
    {
        moveController.Move();
    }

    /// <summary>
    /// 移動終了
    /// </summary>
    public void MoveEnd()
    {
        brethController.ChangeState(PlayerBrethController.BrethState.WAIT);
    }

    /// <summary>
    /// 歩く
    /// </summary>
    public void Walk()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
        brethController.ChangeState(PlayerBrethController.BrethState.WALK);
    }

    /// <summary>
    /// 歩く終了
    /// </summary>
    public void WalkEnd(){}

    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.DASH);
        brethController.ChangeState(PlayerBrethController.BrethState.DASH);
    }

    /// <summary>
    /// ダッシュ狩猟
    /// </summary>
    public void DashEnd()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
    }

    /// <summary>
    /// しゃがみ
    /// </summary>
    public void Squat()
    {
        collider.height = 0.7f;
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.SQUAT);
    }

    /// <summary>
    /// しゃがみ終了
    /// </summary>
    public void SquatEnd()
    {
        collider.height = 1.4f;
    }

    /// <summary>
    /// 忍び歩き時
    /// </summary>
    public void Stealth()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.STEALTH);
        brethController.ChangeState(PlayerBrethController.BrethState.STEALTH);
        brethController.StealthConsumeBreath();
    }

    /// <summary>
    /// 忍び歩き終了
    /// </summary>
    public void StealthEnd()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
    }

    /// <summary>
    /// 忍び歩き時
    /// </summary>
    public void DeepBreath()
    {
        brethController.ChangeState(PlayerBrethController.BrethState.DEEPBREATH);
    }

    /// <summary>
    /// 忍び歩き終了
    /// </summary>
    public void DeepBreathEnd(){}

    /// <summary>
    /// ドア開閉時
    /// </summary>
    public void DoorOpen() { }

    /// <summary>
    /// ドア開閉終了
    /// </summary>
    public void DoorOpenEnd() { }

    /// <summary>
    /// 隠れる時
    /// </summary>
    public void Hide()
    {
        brethController.ChangeState(PlayerBrethController.BrethState.HIDE);

        // 息が切れてしまったらゲームオーバー
        if (brethController.NowState != PlayerBrethController.BrethState.BREATHLESSNESS)
        {
            // ゲームオーバー
        }
    }

    /// <summary>
    /// 隠れる終了
    /// </summary>
    public void HideEnd() { }
}