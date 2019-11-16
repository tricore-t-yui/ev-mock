using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimType = PlayerAnimationContoller.AnimationType;
using MoveType = PlayerStateController.ActionStateType;
using SpeedType = PlayerMoveController.SpeedLimitType;
using ActionSoundType = SoundAreaController.ActionSoundType;

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
    PlayerBreathController breathController = default;      // 息管理クラス
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
    [SerializeField]
    PlayerStateController stateController = default;        // ステート管理クラス
    [SerializeField]
    SoundAreaController soundArea = default;                // 音管理クラス

    /// <summary>
    /// 待機
    /// </summary>
    public void Wait()
    {
        breathController.StateUpdate(MoveType.WAIT);
        soundArea.AddSoundLevel(ActionSoundType.WAIT);
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
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        breathController.StateUpdate(MoveType.WALK);
        animationContoller.AnimStart(AnimType.WALK);
        soundArea.AddSoundLevel(ActionSoundType.WALK);
        camera.IsRotationCamera(true);
    }

    /// <summary>
    /// 歩く終了
    /// </summary>
    public void WalkEnd()
    {
        breathController.StateUpdate(MoveType.WAIT);
        animationContoller.AnimStop(AnimType.WALK);
    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    public void Dash()
    {
        SquatEnd();
        moveController.ChangeSpeedLimit(SpeedType.DASH);
        breathController.StateUpdate(MoveType.DASH);
        animationContoller.AnimStart(AnimType.DASH);
        soundArea.AddSoundLevel(ActionSoundType.DASH);
        camera.IsRotationCamera(true);
    }

    /// <summary>
    /// ダッシュ終了
    /// </summary>
    public void DashEnd()
    {
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        animationContoller.AnimStop(AnimType.DASH);
    }

    /// <summary>
    /// しゃがみ
    /// </summary>
    public void Squat()
    {
        collider.height = 0.4f;
        soundArea.AddSoundLevel(ActionSoundType.SQUAT);
        moveController.ChangeSpeedLimit(SpeedType.SQUAT);
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
    /// 忍び歩き
    /// </summary>
    public void Stealth()
    {
        moveController.ChangeSpeedLimit(SpeedType.STEALTH);
        breathController.StateUpdate(MoveType.STEALTH);
        animationContoller.AnimStart(AnimType.STEALTH);
        camera.IsRotationCamera(true);
        if (stateController.GetDirectionKey())
        {
            soundArea.AddSoundLevel(ActionSoundType.STEALTHMOVE);
        }
        else
        {
            soundArea.AddSoundLevel(ActionSoundType.STEALTH);
        }
    }

    /// <summary>
    /// 忍び歩き終了
    /// </summary>
    public void StealthEnd()
    {
        moveController.ChangeSpeedLimit(SpeedType.WALK);
        animationContoller.AnimStop(AnimType.STEALTH);
    }

    /// <summary>
    /// 深呼吸
    /// </summary>
    public void DeepBreath()
    {
        moveController.IsRootMotion(true, true);
        soundArea.AddSoundLevel(ActionSoundType.DEEPBREATH);
        breathController.StateUpdate(MoveType.DEEPBREATH);
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
        moveController.IsRootMotion(true, true);
        breathController.StateUpdate(MoveType.BREATHLESSNESS);
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
    /// 裸足
    /// </summary>
    public void Barefoot()
    {
        if (!stateController.IsShoes)
        {
            if (stateController.State == MoveType.HIDE)
            {
                animationContoller.DisplayShoesArm(false, true);

                if (hideController.IsStealth)
                {
                    animationContoller.DisplayRightArm(false);
                }
                else
                {
                    animationContoller.DisplayRightArm(true);
                }
            }
            else if (stateController.State == MoveType.WAIT || stateController.State == MoveType.WALK || stateController.State == MoveType.DASH)
            {
                animationContoller.DisplayRightArm(false);
                animationContoller.DisplayShoesArm(true, true);
            }
        }

        moveController.ChangeSpeedLimit(SpeedType.BAREFOOT);
        soundArea.AddSoundLevel(ActionSoundType.BAREFOOT);
        damageController.HitDamageObject();

        if (stateController.IsShoes)
        {
            animationContoller.AnimStart(AnimType.SHOES);
            camera.IsRotationCamera(false);
            moveController.IsRootMotion(true, true);
        }
    }

    /// <summary>
    /// 裸足終了
    /// </summary>
    public void BarefootEnd()
    {
        if (!stateController.IsShoes)
        {
            animationContoller.AnimStop(AnimType.SHOES);
            camera.IsRotationCamera(false);
            moveController.IsRootMotion(true, true);
        }
    }
}
