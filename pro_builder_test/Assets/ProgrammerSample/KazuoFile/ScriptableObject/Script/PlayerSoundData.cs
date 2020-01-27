using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音発生の追加量
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/Create PlayerSoundData")]
public class PlayerSoundData : ScriptableObject
{
    [SerializeField]
    [Tooltip("息止め時の音")]
    float breathHold = -5.5f;
    [SerializeField]
    [Tooltip("隠れているときの音")]
    float hide = 0;
    [SerializeField]
    [Tooltip("待機時の音")]
    float wait = 0;
    [SerializeField]
    [Tooltip("歩き時の音")]
    float walk = 3.5f;
    [SerializeField]
    [Tooltip("忍び歩き時の音")]
    float stealth = 0.5f;
    [SerializeField]
    [Tooltip("しゃがみ時の音")]
    float squat = -2;
    [SerializeField]
    [Tooltip("走っているときの音")]
    float dash = 3.5f;
    [SerializeField]
    [Tooltip("ドアを開けているときの音")]
    float doorOpen = 1;
    [SerializeField]
    [Tooltip("走りながらドアを開けたときの音")]
    float dashDoorOpen = 4;
    [SerializeField]
    [Tooltip("息が少し乱れているときの音")]
    float breathSmallConfusion = 1;
    [SerializeField]
    [Tooltip("息が乱れているときの音")]
    float breathMediumConfusion = 1.25f;
    [SerializeField]
    [Tooltip("息が大きく乱れているときの音")]
    float breathLargeConfusion = 1.5f;
    [SerializeField]
    [Tooltip("息切れしたときの音")]
    float breathlessness = 4;
    [SerializeField]
    [Tooltip("深呼吸しているときの音")]
    float deepBreath = 2;
    [SerializeField]
    [Tooltip("ダメージを食らったときの音")]
    float damage = 3;
    [SerializeField]
    [Tooltip("体力が半分になっているときの音")]
    float halfBreath = 1;
    [SerializeField]
    [Tooltip("体力がピンチになっているときの音")]
    float pinchBreath = 2;
    [SerializeField]
    [Tooltip("裸足になっているときの音")]
    float barefoot = -3;
    [SerializeField]
    [Tooltip("裸足でダメージオブジェクトを踏んだときの音")]
    float barefootObjectDamage = 15;
    [SerializeField]
    [Tooltip("靴でダメージオブジェクトを踏んだときの音")]
    float shoesObjectDamage = 14;
    [SerializeField]
    [Tooltip("落ちたときの音")]
    float fall = 10;

    // 各値のプロパティ
    public float Hide => hide;
    public float BreathHold => breathHold;
    public float Wait => wait;
    public float Walk => walk;
    public float Stealth => stealth;
    public float Squat => squat;
    public float Dash => dash;
    public float DoorOpen => doorOpen;
    public float DashDoorOpen => dashDoorOpen;
    public float BreathSmallConfusion => breathSmallConfusion;
    public float BreathMediumConfusion => breathMediumConfusion;
    public float BreathLargeConfusion => breathLargeConfusion;
    public float Breathlessness => breathlessness;
    public float DeepBreath => deepBreath;
    public float Damage => damage;
    public float HalfBreath => halfBreath;
    public float PinchBreath => pinchBreath;
    public float Barefoot => barefoot;
    public float BarefootObjectDamage => barefootObjectDamage;
    public float ShoesObjectDamage => shoesObjectDamage;
    public float Fall => fall;
}
