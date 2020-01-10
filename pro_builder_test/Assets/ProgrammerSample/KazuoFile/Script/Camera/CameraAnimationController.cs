using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動用カメラのアニメーションクラス
/// </summary>
public class CameraAnimationController : MonoBehaviour
{
    /// <summary>
    /// アニメーションのタイプ
    /// </summary>
    public enum AnimationType
    {
        NORMAL,             // 通常
        WALK,               // 歩き
        BREATHRECOVERY,     // 息回復
        DAMAGE,             // ダメージ
        DASH,               // ダッシュ
        BREATHHOLD,         // 息止め
        BREATHLESSNESS,     // 息切れ
        DEATH,              // 死亡
        OBJECTDAMAGE,       // オブジェクトダメージ
        DEEPBREATH,         // 深呼吸
    }

    [SerializeField]
    Animator animator = default;                            // カメラのアニメーター
    [SerializeField]
    GameObject[] playerArm = default;                       // カメラについているプレイヤーの腕

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        DisplayPlayerArm(false);
    }

    /// <summary>
    /// アニメーションの開始
    /// </summary>
    /// <param name="type">アニメーションのタイプ</param>
    public void AnimStart(AnimationType type)
    {
        switch (type)
        {
            case AnimationType.WALK: animator.SetBool("Walk", true); break;
            case AnimationType.BREATHRECOVERY: animator.SetBool("BreathRecovery", true); break;
            case AnimationType.DAMAGE: animator.SetBool("Damage", true); break;
            case AnimationType.DASH: animator.SetBool("Dash", true); break;
            case AnimationType.BREATHHOLD: animator.SetBool("Stealth", true); break;
            case AnimationType.BREATHLESSNESS: animator.SetBool("Breathlessness", true); break;
            case AnimationType.DEATH: animator.SetBool("Death", true);break;
            case AnimationType.OBJECTDAMAGE: animator.SetBool("ObjectDamage", true); break;
            case AnimationType.DEEPBREATH: animator.SetBool("DeepBreath", true); break;
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
            case AnimationType.BREATHRECOVERY: animator.SetBool("BreathRecovery", false); break;
            case AnimationType.DAMAGE: animator.SetBool("Damage", false); break;
            case AnimationType.DASH: animator.SetBool("Dash", false); break;
            case AnimationType.BREATHHOLD: animator.SetBool("Stealth", false); break;
            case AnimationType.BREATHLESSNESS: animator.SetBool("Breathlessness", false); break;
            case AnimationType.DEATH: animator.SetBool("Death", false); break;
            case AnimationType.OBJECTDAMAGE: animator.SetBool("ObjectDamage", false); break;
            case AnimationType.DEEPBREATH: animator.SetBool("DeepBreath", false); break;
        }
    }

    /// <summary>
    /// プレイヤーの腕の表示
    /// </summary>
    public void DisplayPlayerArm(bool flag)
    {
        for (int i = 0; i < playerArm.Length; i++)
        {
            playerArm[i].SetActive(flag);
        }
    }

    /// <summary>
    /// 腕の表示
    /// </summary>
    /// NOTE:k.oishi アニメーション用関数
    public void DisplayArm()
    {
        DisplayPlayerArm(true);
    }
}
