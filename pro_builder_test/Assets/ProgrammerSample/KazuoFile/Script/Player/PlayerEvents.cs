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
    CapsuleCollider collider = default;                             // コライダー
    [SerializeField]
    PlayerMoveController moveController = default;                  // 移動クラス
    [SerializeField]
    PlayerBreathController breathController = default;              // 息管理クラス
    [SerializeField]
    PlayerHideController hideController = default;                  // 隠れるアクションクラス
    [SerializeField]
    PlayerDoorController doorController = default;                  // 隠れるアクションクラス
    [SerializeField]
    CameraController camera = default;                              // カメラクラス
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
    public void Wait()
    {
        breathController.StateUpdate(MoveType.WAIT);
        soundArea.AddSoundLevel(ActionSoundType.WAIT);
        camera.IsRotationCamera(true);
        camera.Rotation(CameraType.NORMAL);
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
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        breathController.StateUpdate(MoveType.WALK);
        playerAnimationContoller.AnimStart(PlayerAnimType.WALK);
        soundArea.AddSoundLevel(ActionSoundType.WALK);
        camera.IsRotationCamera(true);
        camera.Rotation(CameraType.NORMAL);
    }

    /// <summary>
    /// 歩く終了
    /// </summary>
    public void WalkEnd()
    {
        breathController.StateUpdate(MoveType.WAIT);
        playerAnimationContoller.AnimStop(PlayerAnimType.WALK);
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        SquatEnd();
        moveController.ChangeSpeedLimit(SpeedType.DASH);
        breathController.StateUpdate(MoveType.DASH);
        playerAnimationContoller.AnimStart(PlayerAnimType.DASH);
        soundArea.AddSoundLevel(ActionSoundType.DASH);
        camera.IsRotationCamera(true);
        camera.Rotation(CameraType.NORMAL);
        cameraAnimationController.AnimStart(CameraAnimType.DASH);
    }

    /// <summary>
    /// ダッシュ終了
    /// </summary>
    public void DashEnd()
    {
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        playerAnimationContoller.AnimStop(PlayerAnimType.DASH);
        cameraAnimationController.AnimStop(CameraAnimType.DASH);
    }

    /// <summary>
    /// しゃがみ
    /// </summary>
    public void Squat()
    {
        collider.height = 0.4f;
        soundArea.AddSoundLevel(ActionSoundType.SQUAT);
        moveController.ChangeSpeedLimit(SpeedType.SQUAT);
        playerAnimationContoller.AnimStart(PlayerAnimType.SQUAT);
        camera.IsRotationCamera(true);
        camera.Rotation(CameraType.NORMAL);
    }

    /// <summary>
    /// しゃがみ終了
    /// </summary>
    public void SquatEnd()
    {
        collider.height = 1.4f;
        playerAnimationContoller.AnimStop(PlayerAnimType.SQUAT);
    }

    /// <summary>
    /// 忍び歩き
    /// </summary>
    public void Stealth()
    {
        moveController.ChangeSpeedLimit(SpeedType.STEALTH);
        breathController.StateUpdate(MoveType.STEALTH);
        playerAnimationContoller.AnimStart(PlayerAnimType.STEALTH);
        camera.IsRotationCamera(true);
        camera.Rotation(CameraType.NORMAL);
        cameraAnimationController.AnimStart(CameraAnimType.STEALTH);
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
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        playerAnimationContoller.AnimStop(PlayerAnimType.STEALTH);
        cameraAnimationController.AnimStop(CameraAnimType.STEALTH);
    }

    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreath()
    {
        soundArea.AddSoundLevel(ActionSoundType.DEEPBREATH);
        breathController.StateUpdate(MoveType.DEEPBREATH);
        objectDamageController.RecoveryDeepBreath();
        camera.IsRotationCamera(true);
        camera.Rotation(CameraType.NORMAL);
        cameraAnimationController.AnimStart(CameraAnimType.DEEPBREATH);
    }

    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreathEnd()
    {
        cameraAnimationController.AnimStop(CameraAnimType.DEEPBREATH);
    }

    /// <summary>
    /// 息切れ
    /// </summary>
    public void Brethlessness()
    {
        moveController.IsRootMotion(true, true);
        breathController.StateUpdate(MoveType.BREATHLESSNESS);
        playerAnimationContoller.AnimStart(PlayerAnimType.BREATHLESSNESS);
        camera.IsRotationCamera(false);
        cameraAnimationController.AnimStart(CameraAnimType.BREATHLESSNESS);
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
    /// ドア開閉
    /// </summary>
    public void DoorOpen()
    {
        moveController.IsRootMotion(true, true);
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
    }

    /// <summary>
    /// 隠れる
    /// </summary>
    public void Hide()
    {
        breathController.StateUpdate(MoveType.HIDE);
        hideController.EndHideAction();
        soundArea.AddSoundLevel(ActionSoundType.HIDE);
        hideController.HideCameraMove();
        if(hideController.IsHideStealth())
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
    public void HideEnd() { }

    /// <summary>
    /// ダメージ
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

    /// <summary>
    /// 裸足終了
    /// </summary>
    public void BarefootEnter()
    {
        playerAnimationContoller.AnimStart(PlayerAnimType.SHOES);
        camera.IsRotationCamera(false);
        moveController.IsRootMotion(true, true);
    }

    /// <summary>
    /// 裸足
    /// </summary>
    public void Barefoot()
    {
        if (stateController.State == MoveType.HIDE)
        {
            playerAnimationContoller.DisplayShoesArm(false, true);

            if (hideController.IsHideStealth())
            {
                playerAnimationContoller.DisplayRightArm(false);
            }
            else
            {
                playerAnimationContoller.DisplayRightArm(true);
            }
        }
        else if (stateController.State == MoveType.WAIT || stateController.State == MoveType.WALK || stateController.State == MoveType.DASH)
        {
            playerAnimationContoller.DisplayRightArm(false);
            playerAnimationContoller.DisplayShoesArm(true, true);
        }

        soundArea.AddSoundLevel(ActionSoundType.BAREFOOT);
    }

    /// <summary>
    /// 裸足終了
    /// </summary>
    public void BarefootEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.SHOES);
        camera.IsRotationCamera(false);
        moveController.IsRootMotion(true, true);
    }
}
