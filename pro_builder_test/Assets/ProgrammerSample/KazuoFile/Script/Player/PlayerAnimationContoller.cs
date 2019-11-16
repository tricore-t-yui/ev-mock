﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションクラス
/// </summary>
public class PlayerAnimationContoller : MonoBehaviour
{
    /// <summary>
    /// アニメーションのタイプ
    /// </summary>
    public enum AnimationType
    {
        WALK,               // 歩き
        DASH,               // ダッシュ
        STEALTH,            // 忍び歩き
        SQUAT,              // しゃがみ
        BREATHLESSNESS,     // 息切れ
        SHOES,              // 靴
        DAMAGE,             // ダメージ
        DEATH,              // 死亡
        HIDELOCKER,         // ロッカーに隠れる
        HIDEBED,            // ベッドに隠れる
        OPENDOOR,           // ドア開閉
        DASHOPENDOOR,       // ダッシュでドア開閉
        REVERSEOPENDOOR,    // 逆からドア開閉
    }

    /// <summary>
    /// アニメーション終了検知のタイプ
    /// </summary>
    public enum EndAnimationType
    {
        DOOR,
        HIDE,
        DAMAGE,
        SHOES,
    }

    [SerializeField]
    Animator animator = default;                                // プレイヤーのアニメーター

    [SerializeField]
    GameObject rightArm = default;                              // 右腕
    [SerializeField]
    GameObject shoesArm = default;                              // 靴もっている腕
    [SerializeField]
    GameObject shoes = default;                                 // 右腕に持っている靴

    public bool IsEndAnim { get; private set; } = true;         // アニメーションが終わったかどうか

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 靴を履く
        animator.SetBool("Shoes", true);
    }

    /// <summary>
    /// アニメーションの開始
    /// </summary>
    /// <param name="type">アニメーションのタイプ</param>
    public void AnimStart(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.WALK: animator.SetBool("Walk", true);break;
            case AnimationType.DASH: animator.SetBool("Dash", true); break;
            case AnimationType.STEALTH: animator.SetBool("Stealth",true);break;
            case AnimationType.SQUAT: animator.SetBool("Squat", true);break;
            case AnimationType.BREATHLESSNESS: animator.SetBool("Brethlessness", true); DisplayRightArm(true); break;
            case AnimationType.DAMAGE: animator.SetTrigger("Damage"); DisplayRightArm(true); break;
            case AnimationType.DEATH: animator.SetBool("Death", true); DisplayRightArm(true); break;
            case AnimationType.HIDELOCKER: animator.SetTrigger("LockerIn"); DisplayRightArm(true); break;
            case AnimationType.HIDEBED: animator.SetTrigger("BedIn"); DisplayRightArm(true); break;
            case AnimationType.OPENDOOR: animator.SetTrigger("DoorOpen"); DisplayRightArm(true); break;
            case AnimationType.DASHOPENDOOR: animator.SetTrigger("DashDoorOpen"); DisplayRightArm(true); break;
            case AnimationType.REVERSEOPENDOOR: animator.SetBool("ReverseDoorOpen", true); DisplayRightArm(true); break;
            case AnimationType.SHOES:animator.SetTrigger("TakeOffShoes"); IsEndAnim = false; DisplayShoesArm(true, true); break;
        }
    }

    /// <summary>
    /// アニメーションの停止
    /// </summary>
    /// <param name="type">アニメーションのタイプ</param>
    public void AnimStop(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.WALK: animator.SetBool("Walk", false); break;
            case AnimationType.DASH: animator.SetBool("Dash", false); break;
            case AnimationType.STEALTH: animator.SetBool("Stealth", false); break;
            case AnimationType.SQUAT: animator.SetBool("Squat", false); break;
            case AnimationType.BREATHLESSNESS: animator.SetBool("Brethlessness", false); break;
            case AnimationType.DAMAGE: animator.ResetTrigger("Damage"); break;
            case AnimationType.DEATH: animator.SetBool("Death", false); break;
            case AnimationType.HIDELOCKER: animator.ResetTrigger("LockerIn"); break;
            case AnimationType.HIDEBED: animator.ResetTrigger("BedIn"); break;
            case AnimationType.OPENDOOR: animator.ResetTrigger("DoorOpen"); break;
            case AnimationType.DASHOPENDOOR: animator.ResetTrigger("DashDoorOpen"); break;
            case AnimationType.REVERSEOPENDOOR: animator.SetBool("ReverseDoorOpen", false); break;
            case AnimationType.SHOES: animator.SetTrigger("TakeOffShoes"); IsEndAnim = false; break;
        }
    }

    /// <summary>
    /// それぞれのアニメーション終了検知
    /// </summary>
    /// <param name="type">アニメーション終了検知のタイプ</param>
    public bool EndAnimation(EndAnimationType type)
    {
        switch (type)
        {
            case EndAnimationType.DOOR:
                if (animator.GetBool("DoorEnd"))
                {
                    return true;
                }
                return false;
            case EndAnimationType.HIDE:
                if (animator.GetBool("HideEnd"))
                {
                    return true;
                }
                return false;
            case EndAnimationType.DAMAGE:
                if (animator.GetBool("DamageEnd"))
                {
                    return true;
                }
                return false;
            default:
                IsEndAnim = true;
                return true;
        }
    }

    /// <summary>
    /// アニメーション終了フラグのセット関数
    /// </summary>
    /// <param name="type"></param>
    public void SetEndAnimationFlag(EndAnimationType type)
    {
        switch (type)
        {
            case EndAnimationType.DOOR:animator.SetBool("DoorEnd", false);break;
            case EndAnimationType.HIDE:animator.SetBool("HideEnd", false); break;
            case EndAnimationType.DAMAGE:
                animator.SetBool("DamageEnd", false);
                animator.SetBool("DoorEnd", false);
                break;
        }
    }

    /// <summary>
    /// 靴を持った腕表示するかどうか
    /// </summary>
    public void DisplayShoesArm(bool flag, bool isShoes)
    {
        // 表示
        if (flag)
        {
            // 靴をもった腕を表示
            shoesArm.SetActive(true);
            // 手の靴表示非表示
            if (isShoes)
            {
                shoes.SetActive(true);
            }
            else
            {
                shoes.SetActive(false);
            }
        }
        // 非表示
        else
        {
            // 靴をもった腕を非表示
            shoesArm.SetActive(false);
            // 手の靴表示非表示
            if (isShoes)
            {
                shoes.SetActive(true);
            }
            else
            {
                shoes.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 右手を表示するかどうか
    /// </summary>
    public void DisplayRightArm(bool flag)
    {
        if (flag)
        {
            rightArm.gameObject.SetActive(true);
        }
        else
        {
            rightArm.gameObject.SetActive(false);
        }
    }
}