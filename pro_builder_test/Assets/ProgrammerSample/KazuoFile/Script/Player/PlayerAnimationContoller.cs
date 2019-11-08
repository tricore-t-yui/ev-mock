using System.Collections;
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
    }

    [SerializeField]
    Animator animator = default;    // プレイヤーのアニメーター

    /// <summary>
    /// アニメーションの開始
    /// </summary>
    /// <param name="type">アニメーションのタイプ</param>
    public void AnimStart(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.WALK: animator.SetBool("Walk", true); break;
            case AnimationType.DASH: animator.SetBool("Dash", true); break;
            case AnimationType.STEALTH: animator.SetBool("Stealth", true); break;
            case AnimationType.SQUAT: animator.SetBool("Squat", true); break;
            case AnimationType.BREATHLESSNESS: animator.SetBool("Brethlessness", true); break;
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
        }
    }
}