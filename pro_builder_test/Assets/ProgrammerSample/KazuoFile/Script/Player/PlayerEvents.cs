using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimType = PlayerAnimationContoller.AnimationType;

/// <summary>
/// 各イベント関数置き場
/// </summary>
public class PlayerEvents : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider collider = default;                     // コライダー
    [SerializeField]
    PlayerMoveController moveController = default;          // 移動クラス
    [SerializeField]
    PlayerBreathController breathController = default;        // 息管理クラス
    [SerializeField]
    PlayerHideController hideController = default;          // 隠れるアクションクラス
    [SerializeField]
    PlayerDoorController doorController = default;          // 隠れるアクションクラス
    [SerializeField]
    CameraController camera = default;                      // カメラクラス
    [SerializeField]
    PlayerDamageController damageController = default;      // ダメージリアクションクラス
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;  // アニメーションクラス

    /// <summary>
    /// 待機
    /// </summary>
    public void Wait()
    {
        breathController.StateUpdate(PlayerBreathController.BrethState.WAIT);
        camera.IsRotationCamera(true);
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
        breathController.StateUpdate(PlayerBreathController.BrethState.WALK);
        moveController.Move();
        animationContoller.AnimStart(AnimType.WALK);
        camera.IsRotationCamera(true);
    }

    /// <summary>
    /// 歩く終了
    /// </summary>
    public void WalkEnd()
    {
        breathController.StateUpdate(PlayerBreathController.BrethState.WAIT);
        animationContoller.AnimStop(AnimType.WALK);
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        SquatEnd();
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.DASH);
        breathController.StateUpdate(PlayerBreathController.BrethState.DASH);
        moveController.Move();
        animationContoller.AnimStart(AnimType.DASH);
        camera.IsRotationCamera(true);
    }

    /// <summary>
    /// ダッシュ狩猟
    /// </summary>
    public void DashEnd()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
        animationContoller.AnimStop(AnimType.DASH);
    }

    /// <summary>
    /// しゃがみ
    /// </summary>
    public void Squat()
    {
        collider.height = 0.4f;
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.SQUAT);
        animationContoller.AnimStart(AnimType.SQUAT);
        camera.IsRotationCamera(true);
    }

    /// <summary>
    /// しゃがみ終了
    /// </summary>
    public void SquatEnd()
    {
        collider.height = 1.4f;
        animationContoller.AnimStop(AnimType.SQUAT);
    }

    /// <summary>
    /// 忍び歩き時
    /// </summary>
    public void Stealth()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.STEALTH);
        breathController.StateUpdate(PlayerBreathController.BrethState.STEALTH);
        animationContoller.AnimStart(AnimType.STEALTH);
        camera.IsRotationCamera(true);
        moveController.Move();
    }

    /// <summary>
    /// 忍び歩き終了
    /// </summary>
    public void StealthEnd()
    {
        moveController.ChangeSpeedLimit(PlayerMoveController.SpeedLimitType.WALK);
        animationContoller.AnimStop(AnimType.STEALTH);
    }

    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreath()
    {
        breathController.StateUpdate(PlayerBreathController.BrethState.DEEPBREATH);
        moveController.IsRootMotion(true, true);
        camera.IsRotationCamera(false);
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
        breathController.StateUpdate(PlayerBreathController.BrethState.BREATHLESSNESS);
        moveController.IsRootMotion(true, true);
        animationContoller.AnimStart(AnimType.BREATHLESSNESS);
        camera.IsRotationCamera(false);
    }

    /// <summary>
    /// 息切れ終了
    /// </summary>
    public void BrethlessnessEnd()
    {
        animationContoller.AnimStop(AnimType.BREATHLESSNESS);
    }

    /// <summary>
    /// ドア開閉時
    /// </summary>
    public void DoorOpen()
    {
        moveController.IsRootMotion(true, true);
        doorController.EndDoorAction();
    }

    /// <summary>
    /// ドア開閉終了
    /// </summary>
    public void DoorOpenEnd() { }

    /// <summary>
    /// 隠れる時
    /// </summary>
    public void Hide()
    {
        breathController.StateUpdate(PlayerBreathController.BrethState.HIDE);
        hideController.EndHideAction();

        switch (LayerMask.LayerToName(hideController.HideObj.layer))
        {
            case "Locker":
                if (hideController.IsAnimRotation)
                {
                    moveController.IsRootMotion(true, true);
                }
                else
                {
                    moveController.IsRootMotion(true, false);
                }
                break;
            case "Bed":
                if (hideController.IsAnimRotation)
                {
                    moveController.IsRootMotion(false, true);
                }
                else
                {
                    moveController.IsRootMotion(false, false);
                }
                break;
        }
    }

    /// <summary>
    /// 隠れる終了
    /// </summary>
    public void HideEnd() { }

    /// <summary>
    /// ダメージ時
    /// </summary>
    public void Damage()
    {
        moveController.IsRootMotion(true, true);
        damageController.EndDamageAction();
        camera.IsRotationCamera(false);
    }

    /// <summary>
    /// ダメージ終了
    /// </summary>
    public void DamageEnd() { }
}
