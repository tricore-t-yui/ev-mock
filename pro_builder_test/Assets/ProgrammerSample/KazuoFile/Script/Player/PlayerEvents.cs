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
    CapsuleCollider playerCollider = default;                       // コライダー
    [SerializeField]
    PlayerMoveController moveController = default;                  // 移動クラス
    [SerializeField]
    PlayerHideController hideController = default;                  // 隠れるアクションクラス
    [SerializeField]
    PlayerDoorController doorController = default;                  // 隠れるアクションクラス
    [SerializeField]
    CameraController moveCamera = default;                          // カメラクラス
    [SerializeField]
    PlayerDamageController damageController = default;              // ダメージリアクションクラス
    [SerializeField]
    PlayerAnimationContoller playerAnimationContoller = default;    // アニメーションクラス
    [SerializeField]
    PlayerStateController stateController = default;                // ステート管理クラス
    [SerializeField]
    SoundAreaSpawner soundArea = default;                           // 音領域管理クラス
    [SerializeField]
    SoundSpawner sound = default;                                   // 音生成クラス
    [SerializeField]
    CameraAnimationController cameraAnimationController = default;  // カメラアニメーションクラス
    [SerializeField]
    PlayerStatusController statusController = default;              // ステータス管理クラス
    [SerializeField]
    PlayerTrapController trapController = default;                  // 罠アクションクラス

    /// <summary>
    /// 待機
    /// </summary>
    public void WaitStart() { }
    /// <summary>
    /// 待機
    /// </summary>
    public void Wait()
    {
        moveController.IsRootMotion(false, false);
        moveCamera.Rotation(CameraType.NORMAL);
        soundArea.AddSoundLevel(ActionSoundType.WAIT);
        statusController.StateUpdate(MoveType.WAIT, stateController.IsSquat);
    }
    /// <summary>
    /// 待機
    /// </summary>
    public void WaitEnd() { }


    /// <summary>
    /// 歩く開始
    /// </summary>
    public void WalkStart() { }
    /// <summary>
    /// 歩く
    /// </summary>
    public void Walk()
    {
        playerAnimationContoller.AnimStart(PlayerAnimType.WALK);
        moveController.ChangeMoveTypeSpeedLimit(SpeedType.WALK);
        moveCamera.Rotation(CameraType.NORMAL);
        moveController.Move();
        statusController.StateUpdate(MoveType.WALK, stateController.IsSquat);
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
    public void DashStart() { }
    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        SquatEnd();
        cameraAnimationController.AnimStart(CameraAnimType.DASH);
        moveController.ChangeMoveTypeSpeedLimit(SpeedType.DASH);
        playerAnimationContoller.AnimStart(PlayerAnimType.DASH);
        soundArea.AddSoundLevel(ActionSoundType.DASH);
        moveCamera.Rotation(CameraType.NORMAL);
        moveController.Move();
        statusController.StateUpdate(MoveType.DASH, stateController.IsSquat);
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
    public void SquatStart() { }
    /// <summary>
    /// しゃがみ
    /// </summary>
    public void Squat()
    {
        playerCollider.height = 0.4f;
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
    /// 息止め開始
    /// </summary>
    public void BreathHoldStart() {}
    /// <summary>
    /// 息止め
    /// </summary>
    public void BreathHold()
    {
        cameraAnimationController.AnimStart(CameraAnimType.BREATHHOLD);
        playerAnimationContoller.AnimStart(PlayerAnimType.BREATHHOLD);
        statusController.StateUpdate(MoveType.BREATHHOLD, stateController.IsSquat);
        moveCamera.Rotation(CameraType.NORMAL);
        soundArea.AddSoundLevel(ActionSoundType.BREATHHOLD);
    }
    /// <summary>
    /// 息止め移動
    /// </summary>
    public void BreathHoldMove()
    {
        cameraAnimationController.AnimStart(CameraAnimType.BREATHHOLD);
        playerAnimationContoller.AnimStart(PlayerAnimType.BREATHHOLD);
        statusController.StateUpdate(MoveType.BREATHHOLDMOVE, stateController.IsSquat);
        moveCamera.Rotation(CameraType.NORMAL);
        soundArea.AddSoundLevel(ActionSoundType.BREATHHOLD);
        moveController.ChangeMoveTypeSpeedLimit(SpeedType.BREATHHOLD);
        moveController.Move();
    }
    /// <summary>
    /// 息止め終了
    /// </summary>
    public void BreathHoldEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.BREATHHOLD);
        cameraAnimationController.AnimStop(CameraAnimType.BREATHHOLD);
    }

    /// <summary>
    /// 深呼吸開始
    /// </summary>
    public void DeepBreathStart() { }
    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreath()
    {
        SquatEnd();
        cameraAnimationController.AnimStart(CameraAnimType.DEEPBREATH);
        soundArea.AddSoundLevel(ActionSoundType.DEEPBREATH);
        moveCamera.Rotation(CameraType.NORMAL);
        statusController.DeepBreathRecovery();
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
    }
    /// <summary>
    /// 息切れ
    /// </summary>
    public void Brethlessness()
    {
        cameraAnimationController.AnimStart(CameraAnimType.BREATHLESSNESS);
        playerAnimationContoller.AnimStart(PlayerAnimType.BREATHLESSNESS);
        statusController.StateUpdate(MoveType.BREATHLESSNESS, stateController.IsSquat);
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
        SquatEnd();
        statusController.StateUpdate(MoveType.DOOROPEN, stateController.IsSquat);
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
        moveController.IsRootMotion(false, false);
    }

    /// <summary>
    /// 隠れる開始
    /// </summary>
    public void HideStart()
    {
        SquatEnd();
        sound.Play(SoundSpawner.SoundType.HeartSound);
        moveController.IsRootMotion(true, true);
    }
    /// <summary>
    /// 隠れる
    /// </summary>
    public void Hide()
    {
        statusController.StateUpdate(MoveType.HIDE, stateController.IsSquat);
        hideController.EndHideAction(false);
        soundArea.AddSoundLevel(ActionSoundType.HIDE);
        hideController.HideCameraMove();
        if (hideController.IsHideStealth())
        {
            soundArea.AddSoundLevel(ActionSoundType.BREATHHOLD);
        }

        hideController.ChangeRootMotion();
    }
    /// <summary>
    /// 隠れる終了
    /// </summary>
    public void HideEnd()
    {
        sound.Stop(SoundSpawner.SoundType.HeartSound);
        hideController.EndHideAction(true);
        moveController.IsRootMotion(false, false);
    }

    /// <summary>
    /// ダメージ開始
    /// </summary>
    public void DamageStart() { }
    /// <summary>
    /// ダメージ
    /// </summary>
    public void Damage()
    {
        SquatEnd();
        switch (damageController.Type)
        {
            case PlayerDamageController.DamageType.HIDELOCKER:
                moveController.IsRootMotion(true, true);
                break;
            case PlayerDamageController.DamageType.DEATH:
                if(damageController.IsFinishBlow)
                {
                    moveController.IsRootMotion(false, false);
                    cameraAnimationController.AnimStart(CameraAnimType.DEATH);
                    moveCamera.Rotation(CameraType.DEATH);
                }
                else
                {
                    moveController.IsRootMotion(false, true);
                }
                break;
            default:
                moveController.IsRootMotion(false, true);
                break;
        }
        damageController.EndDamageAction();
    }
    /// <summary>
    /// ダメージ終了
    /// </summary>
    public void DamageEnd() { }

    /// <summary>
    /// 人形ゲット開始
    /// </summary>
    public void DollGetStart()
    {
        playerAnimationContoller.AnimStart(PlayerAnimType.GETDOLL);
    }
    /// <summary>
    /// 人形ゲット
    /// </summary>
    public void DollGet()
    {
        moveController.IsRootMotion(true, true);
    }
    /// <summary>
    /// 人形ゲット終了
    /// </summary>
    public void DollGetEnd()
    {
        playerAnimationContoller.AnimStop(PlayerAnimType.GETDOLL);
        moveController.IsRootMotion(false, false);
    }

    /// <summary>
    /// 裸足開始
    /// </summary>
    public void BarefootStart()
    {
        playerAnimationContoller.AnimStart(PlayerAnimType.SHOES);
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
        moveController.IsRootMotion(true, true);
    }

    /// <summary>
    /// 罠アクション開始
    /// </summary>
    public void TrapStart() { }
    /// <summary>
    /// 罠アクション
    /// </summary>
    public void Trap()
    {
        SquatEnd();
        trapController.EndTrapAction();
        moveController.IsRootMotion(true, true);
    }
    /// <summary>
    /// 罠アクション終了
    /// </summary>
    public void TrapEnd() { }
}
