using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create PlayerStatusData")]
public class PlayerStatusData : ScriptableObject
{
    // ステータス系
    // 息
    [SerializeField]
    float normalRecovery = 0.3f;                // 通常の息の回復量
    [SerializeField]
    float deepBreathRecovery = 0.5f;            // 深呼吸時の息の回復量
    [SerializeField]
    float breathlessnessRecovery = 0.3f;        // 息切れ時の息の回復量
    [SerializeField]
    float stealthDecrement = 0.175f;            // 息止め時の息消費量
    [SerializeField]
    float hideSmallDecrement = 0.3f;            // 隠れる＋息止め時の息消費量(小)
    [SerializeField]
    float hideMediumDecrement = 0.3f;           // 隠れる＋息止め時の息消費量(中)
    [SerializeField]
    float hideLargeDecrement = 0.3f;            // 隠れる＋息止め時の息消費量(大)
    [SerializeField]
    float buttonPatienceDecrement = 0.01f;      // 息我慢時(連打あり)の息消費量
    [SerializeField]
    float smallDisturbance = 75;                // 息の乱れ(小)の基準値
    [SerializeField]
    float mediumDisturbance = 50;               // 息の乱れ(中)の基準値
    [SerializeField]
    float largeDisturbance = 20;                // 息の乱れ(大)の基準値

    // 体力
    [SerializeField]
    float healthRecoveryFrame = 240;            // 体力回復が始まるまでのフレーム数
    [SerializeField]
    float healthRecoveryAmount = 0.1f;          // 体力の回復量

    // スタミナ
    [SerializeField]
    float staminaDecrement = 0.5f;              // ダッシュのスタミナの消費量
    [SerializeField]
    float staminaNormalRecovery = 0.3f;         // 通常のスタミナの回復量
    [SerializeField]
    float staminaWalkRecovery = 0.15f;          // 通常のスタミナの回復量
    [SerializeField]
    float staminaSquatRecovery = 0.3f;          // 通常のスタミナの回復量
    [SerializeField]
    float staminaDeepBreathRecovery = 0.5f;     // 深呼吸時のスタミナの回復量

    // オブジェクトダメージ
    [SerializeField]
    float objectDamageAmount = 0.05f;           // ダメージ量
    [SerializeField]
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
}