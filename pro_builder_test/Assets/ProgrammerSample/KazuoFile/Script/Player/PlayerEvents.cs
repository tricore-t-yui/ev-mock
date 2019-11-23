using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimType = PlayerAnimationContoller.AnimationType;
using MoveType = PlayerStateController.ActionStateType;
using SpeedType = PlayerMoveController.SpeedLimitType;
using ActionSoundType = SoundAreaSpawner.ActionSoundType;
using CameraType = CameraController.RotationType;
using CameraAnimType = CameraAnimationController.AnimationType;

/// <summary>
/// 各イベント関数置き場
/// </summary>
public class PlayerEvents : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider playerCollider = default;                             // コライダー
    [SerializeField]
    PlayerMoveController moveController = default;                  // 移動クラス
    [SerializeField]
    PlayerBreathController breathController = default;              // 息管理クラス
    [SerializeField]
    PlayerHideController hideController = default;                  // 隠れるアクションクラス
    [SerializeField]
    PlayerDoorController doorController = default;                  // 隠れるアクションクラス
    [SerializeField]
    CameraController moveCamera = default;                              // カメラクラス
    [SerializeField]
    PlayerDamageController damageController = default;              // ダメージリアクションクラス
    [SerializeField]
    PlayerAnimationContoller playerAnimationContoller = default;    // アニメーションクラス
    [SerializeField]
    PlayerStateController stateController = default;                // ステート管理クラス
    [SerializeField]
    SoundAreaSpawner soundArea = default;                           // 音管理クラス
    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;  // ダメージリアクションクラス
    [SerializeField]
    CameraAnimationController cameraAnimationController = default;  // ダメージリアクションクラス

    /// <summary>
    /// 待機
    /// </summary>
    public void WaitStart()
    {
        moveController.ChangeSpeedLimit(SpeedType.NOTMOVE);
        moveController.IsRootMotion(false, false);
        moveCamera.IsRotationCamera(true);
    }
    /// <summary>
    /// 待機
    /// </summary>
    public void Wait()
    {
        breathController.StateUpdate(MoveType.WAIT);
        moveCamera.Rotation(CameraType.NORMAL);
        soundArea.AddSoundLevel(ActionSoundType.WAIT);
    }
    /// <summary>
    /// 待機
    /// </summary>
    public void WaitEnd() { }


    /// <summary>
    /// 歩く開始
    /// </summary>
    public void WalkStart()
    {
        moveCamera.IsRotationCamera(true);
    }
    /// <summary>
    /// 歩く
    /// </summary>
    public void Walk()
    {
        playerAnimationContoller.AnimStart(PlayerAnimType.WALK);
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        breathController.StateUpdate(MoveType.WALK);
        soundArea.AddSoundLevel(ActionSoundType.WALK);
        moveCamera.Rotation(CameraType.NORMAL);
    }
    /// <summary>
    /// 歩く終了
    /// </summary>
    public void WalkEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.WALK);
    }

    /// <summary>
    /// ダッシュ開始
    /// </summary>
    public void DashStart()
    {
        moveCamera.IsRotationCamera(true);
    }
    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        SquatEnd();
        cameraAnimationController.AnimStart(CameraAnimType.DASH);
        moveController.ChangeSpeedLimit(SpeedType.DASH);
        playerAnimationContoller.AnimStart(PlayerAnimType.DASH);
        breathController.StateUpdate(MoveType.DASH);
        soundArea.AddSoundLevel(ActionSoundType.DASH);
        moveCamera.Rotation(CameraType.NORMAL);
    }
    /// <summary>
    /// ダッシュ終了
    /// </summary>
    public void DashEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.DASH);
        cameraAnimationController.AnimStop(CameraAnimType.DASH);
    }

    /// <summary>
    /// しゃがみ開始
    /// </summary>
    public void SquatStart()
    {
        playerCollider.height = 0.4f;
        moveCamera.IsRotationCamera(true);
    }
    /// <summary>
    /// しゃがみ
    /// </summary>
    public void Squat()
    {
        moveController.ChangeSpeedLimit(SpeedType.SQUAT);
        playerAnimationContoller.AnimStart(PlayerAnimType.SQUAT);
        soundArea.AddSoundLevel(ActionSoundType.SQUAT);
        moveCamera.Rotation(CameraType.NORMAL);
    }
    /// <summary>
    /// しゃがみ終了
    /// </summary>
    public void SquatEnd()
    {
        playerCollider.height = 1.4f;
        playerAnimationContoller.AnimStop(PlayerAnimType.SQUAT);
    }

    /// <summary>
    /// 忍び歩き開始
    /// </summary>
    public void StealthStart()
    {
        moveCamera.IsRotationCamera(true);
    }
    /// <summary>
    /// 忍び歩き
    /// </summary>
    public void Stealth()
    {
        cameraAnimationController.AnimStart(CameraAnimType.STEALTH);
        moveController.ChangeSpeedLimit(SpeedType.STEALTH);
        playerAnimationContoller.AnimStart(PlayerAnimType.STEALTH);
        breathController.StateUpdate(MoveType.STEALTH);
        moveCamera.Rotation(CameraType.NORMAL);
        soundArea.AddSoundLevel(ActionSoundType.STEALTH);
        if (stateController.GetDirectionKey())
        {
            soundArea.AddSoundLevel(ActionSoundType.WALK);
        }
    }
    /// <summary>
    /// 忍び歩き終了
    /// </summary>
    public void StealthEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.STEALTH);
        cameraAnimationController.AnimStop(CameraAnimType.STEALTH);
    }

    /// <summary>
    /// 深呼吸開始
    /// </summary>
    public void DeepBreathStart()
    {
        moveCamera.IsRotationCamera(true);
    }
    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreath()
    {
        cameraAnimationController.AnimStart(CameraAnimType.DEEPBREATH);
        moveController.ChangeSpeedLimit(SpeedType.NOTMOVE);
        soundArea.AddSoundLevel(ActionSoundType.DEEPBREATH);
        breathController.StateUpdate(MoveType.DEEPBREATH);
        objectDamageController.RecoveryDeepBreath();
        moveCamera.Rotation(CameraType.NORMAL);
    }
    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreathEnd()
    {
        cameraAnimationController.AnimStop(CameraAnimType.DEEPBREATH);
    }

    /// <summary>
    /// 息切れ開始
    /// </summary>
    public void BrethlessnessStart()
    {
        moveController.IsRootMotion(true, true);
        moveCamera.IsRotationCamera(false);
    }
    /// <summary>
    /// 息切れ
    /// </summary>
    public void Brethlessness()
    {
        cameraAnimationController.AnimStart(CameraAnimType.BREATHLESSNESS);
        playerAnimationContoller.AnimStart(PlayerAnimType.BREATHLESSNESS);
        breathController.StateUpdate(MoveType.BREATHLESSNESS);
    }
    /// <summary>
    /// 息切れ終了
    /// </summary>
    public void BrethlessnessEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.BREATHLESSNESS);
        cameraAnimationController.AnimStop(CameraAnimType.BREATHLESSNESS);
    }

    /// <summary>
    /// ドア開閉開始
    /// </summary>
    public void DoorOpenStart()
    {
        moveController.IsRootMotion(true, true);
    }
    /// <summary>
    /// ドア開閉
    /// </summary>
    public void DoorOpen()
    {
        doorController.EndDoorAction(false);
        if (stateController.IsDashOpen)
        {
            soundArea.AddSoundLevel(ActionSoundType.DASHDOOROPEN);
        }
        else
        {
            soundArea.AddSoundLevel(ActionSoundType.DOOROPEN);
        }
    }
    /// <summary>
    /// ドア開閉終了
    /// </summary>
    public void DoorOpenEnd()
    {
        doorController.EndDoorAction(true);
        moveController.IsRootMotion(false, false);
    }

    /// <summary>
    /// 隠れる開始
    /// </summary>
    public void HideStart() { }
    /// <summary>
    /// 隠れる
    /// </summary>
    public void Hide()
    {
        breathController.StateUpdate(MoveType.HIDE);
        hideController.EndHideAction(false);
        soundArea.AddSoundLevel(ActionSoundType.HIDE);
        hideController.HideCameraMove();
        if (hideController.IsHideStealth())
        {
            soundArea.AddSoundLevel(ActionSoundType.STEALTH);
        }

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
    public void HideEnd()
    {
        hideController.EndHideAction(true);
        moveController.IsRootMotion(false, false);
    }

    /// <summary>
    /// ダメージ開始
    /// </summary>
    public void DamageStart()
    {
        moveController.IsRootMotion(true, true);
        moveCamera.IsRotationCamera(false);
    }
    /// <summary>
    /// ダメージ
    /// </summary>
    public void Damage()
    {
        damageController.EndDamageAction();
    }
    /// <summary>
    /// ダメージ終了
    /// </summary>
    public void DamageEnd() { }

    /// <summary>
    /// 裸足開始
    /// </summary>
    public void BarefootStart()
    {
        playerAnimationContoller.AnimStart(PlayerAnimType.SHOES);
        moveCamera.IsRotationCamera(false);
        moveController.IsRootMotion(true, true);
    }
    /// <summary>
    /// 裸足
    /// </summary>
    public void Barefoot()
    {
        playerAnimationContoller.BarefootRightArm();
        soundArea.AddSoundLevel(ActionSoundType.BAREFOOT);
    }
    /// <summary>
    /// 裸足終了
    /// </summary>
    public void BarefootEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.SHOES);
        moveCamera.IsRotationCamera(false);
        moveController.IsRootMotion(true, true);
    }
}
