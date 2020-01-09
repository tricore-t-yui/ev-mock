using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create PlayerMoveData")]
public class PlayerMoveData : ScriptableObject
{
    [SerializeField]
    float forwardSpeed = 2f;                    // 前移動時のスピード
    [SerializeField]
    float sideSpeed = 1.75f;                    // 横移動時のスピード
    [SerializeField]
    float backSpeed = 1.5f;                     // 後ろ移動時のスピード
    [SerializeField]
    float speedMagnification = 10;              // 移動速度の倍率

    [SerializeField, Range(0, 1)]
    float barefootSpeedReduction = 0.85f;       // 裸足時の移動速度の減少割合
    [SerializeField, Range(0, 1)]
    float objectDamageSpeedReduction = 0.85f;   // オブジェクトダメージ時の移動速度の減少割合
    [SerializeField, Range(0, 1)]
    float staminaSpeedReduction = 0.85f;        // スタミナ切れ時の移動速度の減少割合

    [SerializeField]
    float walkSpeedLimit = 0.75f;               // 歩き時の移動速度の限界
    [SerializeField]
    float dashSpeedLimit = 1.5f;                // ダッシュ時の移動速度の限界
    [SerializeField]
    float squatSpeedLimit = 0.25f;              // しゃがみ時の移動速度の限界
    [SerializeField]
    float stealthSpeedLimit = 0.25f;            // 忍び歩き時の移動速度の限界

    [SerializeField, Range(0, 180)]
    float stepAngle = 60;                       // 段差の許容角度
    [SerializeField]
    float stepUpPower = 1.45f;                  // 段差に当たった時に加える上方向の力

    // 各値のプロパティ
    public float ForwardSpeed => forwardSpeed;
    public float SideSpeed => sideSpeed;
    public float BackSpeed => backSpeed;
    public float SpeedMagnification => speedMagnification;
    public float BarefootSpeedReduction => barefootSpeedReduction;
    public float ObjectDamageSpeedReduction => objectDamageSpeedReduction;
    public float StaminaSpeedReduction => staminaSpeedReduction;
    public float WalkSpeedLimit => walkSpeedLimit;
    public float DashSpeedLimit => dashSpeedLimit;
    public float SquatSpeedLimit => squatSpeedLimit;
    public float StealthSpeedLimit => stealthSpeedLimit;
    public float StepAngle => stepAngle;
    public float StepUpPower => stepUpPower;
}