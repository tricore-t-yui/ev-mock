using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 音発生の追加量
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/Create PlayerSoundData")]
public class PlayerSoundData : ScriptableObject
{
    [SerializeField]
    [LabelText("息が少し乱れているときの増加係数")]
    float breathSmallConfusionFactor = 1.5f;
    [SerializeField]
    [LabelText("息が乱れているときの増加係数")]
    float breathMediumConfusionFactor = 2.0f;

    [SerializeField]
    [LabelText("隠れているときの音")]
    float hide = 0;
    [SerializeField]
    [LabelText("待機時の音")]
    float wait = 0;
    [SerializeField]
    [LabelText("歩き時の音")]
    float walk = 3.5f;
    [SerializeField]
    [LabelText("歩き時の音(息止め)")]
    float breathHoldWalk = 3.5f;
    [SerializeField]
    [LabelText("忍び歩き時の音")]
    float stealth = 0.5f;
    [SerializeField]
    [LabelText("忍び歩き時の音(息止め)")]
    float breathHoldStealth = 0.5f;
    [SerializeField]
    [LabelText("しゃがみ時の音")]
    float squat = -2;
    [SerializeField]
    [LabelText("しゃがみ時の音(息止め)")]
    float breathHoldSquat = -2;
    [SerializeField]
    [LabelText("走っているときの音")]
    float dash = 3.5f;
    [SerializeField]
    [LabelText("ドアを開けているときの音")]
    float doorOpen = 1;
    [SerializeField]
    [LabelText("走りながらドアを開けたときの音")]
    float dashDoorOpen = 4;
    [SerializeField]
    [LabelText("息切れしたときの音")]
    float breathlessness = 4;
    [SerializeField]
    [LabelText("深呼吸しているときの音")]
    float deepBreath = 2;
    [SerializeField]
    [LabelText("ダメージを食らったときの音")]
    float damage = 3;
    [SerializeField]
    [LabelText("体力が半分になっているときの音")]
    float halfBreath = 1;
    [SerializeField]
    [LabelText("体力がピンチになっているときの音")]
    float pinchBreath = 2;
    [SerializeField]
    [LabelText("裸足になっているときの音")]
    float barefoot = -3;
    [SerializeField]
    [LabelText("裸足でダメージオブジェクトを踏んだときの音")]
    float barefootObjectDamage = 15;
    [SerializeField]
    [LabelText("靴でダメージオブジェクトを踏んだときの音")]
    float shoesObjectDamage = 14;
    [SerializeField]
    [LabelText("落ちたときの音")]
    float fall = 10;
    [SerializeField]
    [LabelText("スタミナが半分以下になった時の音")]
    float stamina = 10;

    // 各値のプロパティ
    public float BreathSmallConfusionFactor => breathSmallConfusionFactor;
    public float BreathMediumConfusionFactor => breathMediumConfusionFactor;
    public float Hide => hide;
    public float Wait => wait;
    public float Walk => walk;
    public float BreathHoldWalk => breathHoldWalk;
    public float Stealth => stealth;
    public float BreathHoldStealth => breathHoldStealth;
    public float Squat => squat;
    public float BreathHoldSquat => breathHoldSquat;
    public float Dash => dash;
    public float DoorOpen => doorOpen;
    public float DashDoorOpen => dashDoorOpen;
    public float Breathlessness => breathlessness;
    public float DeepBreath => deepBreath;
    public float Damage => damage;
    public float HalfBreath => halfBreath;
    public float PinchBreath => pinchBreath;
    public float Barefoot => barefoot;
    public float BarefootObjectDamage => barefootObjectDamage;
    public float ShoesObjectDamage => shoesObjectDamage;
    public float Fall => fall;
    public float Stamina => stamina;
}
