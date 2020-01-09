using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータス系のデータ
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/Create PlayerStatusData")]
public class PlayerStatusData : ScriptableObject
{
    // 息
    [Header("息系")]
    [SerializeField]
    [Tooltip("通常時の息の回復量")]
    float normalRecovery = 0.3f;                // 通常の息の回復量
    [SerializeField]
    [Tooltip("深呼吸時の息の回復量")]
    float deepBreathRecovery = 0.5f;            // 深呼吸時の息の回復量
    [SerializeField]
    [Tooltip("息切れ時の息の回復量")]
    float breathlessnessRecovery = 0.3f;        // 息切れ時の息の回復量
    [SerializeField]
    [Tooltip("息止め時の息消費量")]
    float stealthDecrement = 0.175f;            // 息止め時の息消費量
    [SerializeField]
    [Tooltip("隠れる＋息止め時の息消費量(小)")]
    float hideSmallDecrement = 0.3f;            // 隠れる＋息止め時の息消費量(小)
    [SerializeField]
    [Tooltip("隠れる＋息止め時の息消費量(中)")]
    float hideMediumDecrement = 0.3f;           // 隠れる＋息止め時の息消費量(中)
    [SerializeField]
    [Tooltip("隠れる＋息止め時の息消費量(大)")]
    float hideLargeDecrement = 0.3f;            // 隠れる＋息止め時の息消費量(大)
    [SerializeField]
    [Tooltip("息我慢時(連打あり)の息消費量")]
    float buttonPatienceDecrement = 0.01f;      // 息我慢時(連打あり)の息消費量
    [SerializeField]
    [Tooltip("息の乱れ(小)の基準値")]
    float smallDisturbance = 75;                // 息の乱れ(小)の基準値
    [SerializeField]
    [Tooltip("息の乱れ(中)の基準値")]
    float mediumDisturbance = 50;               // 息の乱れ(中)の基準値
    [SerializeField]
    [Tooltip("息の乱れ(大)の基準値")]
    float largeDisturbance = 20;                // 息の乱れ(大)の基準値

    // 体力
    [Header("体力系")]
    [SerializeField]
    [Tooltip("体力回復が始まるまでのフレーム数")]
    float healthRecoveryFrame = 240;            // 体力回復が始まるまでのフレーム数
    [SerializeField]
    [Tooltip("体力の回復量")]
    float healthRecoveryAmount = 0.1f;          // 体力の回復量
    [SerializeField]
    [Tooltip("無敵時間")]
    float invincibleSecond = 2;           // 無敵時間

    // スタミナ
    [Header("スタミナ系")]
    [SerializeField]
    [Tooltip("ダッシュのスタミナの消費量")]
    float staminaDecrement = 0.5f;              // ダッシュのスタミナの消費量
    [SerializeField]
    [Tooltip("通常時のスタミナの回復量")]
    float staminaNormalRecovery = 0.3f;         // 通常時のスタミナの回復量
    [SerializeField]
    [Tooltip("歩き時のスタミナの回復量")]
    float staminaWalkRecovery = 0.15f;          // 歩き時のスタミナの回復量
    [SerializeField]
    [Tooltip("しゃがみ時のスタミナの回復量")]
    float staminaSquatRecovery = 0.3f;          // しゃがみ時のスタミナの回復量
    [SerializeField]
    [Tooltip("深呼吸時のスタミナの回復量")]
    float staminaDeepBreathRecovery = 0.5f;     // 深呼吸時のスタミナの回復量

    // オブジェクトダメージ
    [Header("オブジェクトダメージ系")]
    [SerializeField]
    [Tooltip("ダメージ量")]
    float objectDamageAmount = 0.05f;           // ダメージ量
    [SerializeField]
    [Tooltip("深呼吸時の回復量")]
    float objectDamagedeepBreathRecovery = 0.3f;// 深呼吸時の回復量

    // 各値のプロパティ
    public float NormalRecovery => normalRecovery;
    public float DeepBreathRecovery => deepBreathRecovery;
    public float BreathlessnessRecovery => breathlessnessRecovery;
    public float StealthDecrement => stealthDecrement;
    public float HideSmallDecrement => hideSmallDecrement;
    public float HideMediumDecrement => hideMediumDecrement;
    public float HideLargeDecrement => hideLargeDecrement;
    public float ButtonPatienceDecrement => buttonPatienceDecrement;
    public float SmallDisturbance => smallDisturbance;
    public float MediumDisturbance => mediumDisturbance;
    public float LargeDisturbance => largeDisturbance;

    public float HealthRecoveryFrame => healthRecoveryFrame;
    public float HealthRecoveryAmount => healthRecoveryAmount;

    public float StaminaDecrement => staminaDecrement;
    public float StaminaNormalRecovery => staminaNormalRecovery;
    public float StaminaWalkRecovery => staminaWalkRecovery;
    public float StaminaSquatRecovery => staminaSquatRecovery;
    public float StaminaDeepBreathRecovery => staminaDeepBreathRecovery;

    public float ObjectDamageAmount => objectDamageAmount;
    public float ObjectDamagedeepBreathRecovery => objectDamagedeepBreathRecovery;

    public float InvincibleSecond => invincibleSecond;
}