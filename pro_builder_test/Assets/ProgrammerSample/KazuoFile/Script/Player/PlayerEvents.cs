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
    CameraController camera = default;    // 息管理クラス

    /// <summary>
    /// 待機
    /// </summary>
    public void Wait()
    {
        camera.MoveShake(0.01f);
        brethController.StateUpdate(PlayerBrethController.BrethState.WAIT);
    }

    /// <summary>
    /// 待機終了
    /// </summary>
    public void WaitEnd() { }

    /// <summary>
    /// 歩く
    /// </summary>
    public void Walk()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
        brethController.StateUpdate(PlayerBrethController.BrethState.WALK);
        moveController.Move();
    }

    /// <summary>
    /// 歩く終了
    /// </summary>
    public void WalkEnd()
    {
        brethController.StateUpdate(PlayerBrethController.BrethState.WAIT);
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        SquatEnd();
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.DASH);
        brethController.StateUpdate(PlayerBrethController.BrethState.DASH);
        moveController.Move();
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
        brethController.StateUpdate(PlayerBrethController.BrethState.STEALTH);
        moveController.Move();
    }

    /// <summary>
    /// 忍び歩き終了
    /// </summary>
    public void StealthEnd()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
    }

    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreath()
    {
        brethController.StateUpdate(PlayerBrethController.BrethState.DEEPBREATH);
    }

    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreathEnd(){}

    /// <summary>
    /// 息切れ
    /// </summary>
    public void Brethlessness()
    { 
        brethController.StateUpdate(PlayerBrethController.BrethState.BREATHLESSNESS);
    }

    /// <summary>
    /// 息切れ終了
    /// </summary>
    public void BrethlessnessEnd() { }

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
        brethController.StateUpdate(PlayerBrethController.BrethState.HIDE);
    }

    /// <summary>
    /// 隠れる終了
    /// </summary>
    public void HideEnd() { }
}