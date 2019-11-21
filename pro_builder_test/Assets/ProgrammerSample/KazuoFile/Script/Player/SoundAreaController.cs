using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音管理クラス
/// </summary>
public class SoundAreaController : MonoBehaviour
{
    /// <summary>
    /// 行動音
    /// </summary>
    public enum ActionSoundType
    {
        STEALTH,            // 息止め
        HIDE,               // 隠れる
        WAIT,               // 待機
        WALK,               // 移動
        SQUAT,              // しゃがみ 
        DASH,               // ダッシュ
        DOOROPEN,           // ドア開閉
        DASHDOOROPEN,       // ダッシュでドア開閉
        SMALLCONFUSION,     // 息の小さな乱れ
        MEDIUMCONFUSION,    // 息の乱れ
        LARGECONFUSION,     // 息の大きな乱れ
        BREATHLESSNESS,     // 息切れ
        DEEPBREATH,         // 深呼吸
        DAMAGE,             // ダメージ
        DAMAGEHALFHEALTH,   // 体力が半分
        DAMAGEPINCHHEALTH,  // 体力がピンチ
        BAREFOOT,           // 裸足
        BAREFOOTDAMAGE,     // 裸足でダメージ
    }

    /// <summary>
    /// 心音
    /// </summary>
    public enum HeartSoundType
    {
        NORMAL,     // 通常
        SMALL,      // 小
        MEDIUM,     // 中
        LARGE,      // 大
    }

    [SerializeField]
    SphereCollider collider = default;                      // 音発生の領域
    [SerializeField]
    HideStateController hideState = default;                // 隠れる状態管理クラス

    [SerializeField]
    float areaMagnification = 0.3f;                         // 拡大倍率

    float soundLevel = 0;                                   // 音量レベル
    public float TotalSoundLevel { get; private set; } = 0; // 音量レベルの合計
    public HeartSoundType HeartSound { get; private set; } = HeartSoundType.NORMAL;      // 心音

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 合計値に適応、レベルリセット
        TotalSoundLevel = soundLevel;
        soundLevel = 0;

        // 合計値に応じて領域拡大
        collider.radius = 1 + (areaMagnification * TotalSoundLevel);

        // 心音の変更
        ChangeHeartSound();
    }

    /// <summary>
    /// 音のレベルの加算
    /// </summary>
    public void AddSoundLevel(ActionSoundType type)
    {
        // レベル加算用変数
        float addLevel = 0;

        // 行動音によって音レベルを加算
        switch (type)
        {
            case ActionSoundType.STEALTH: addLevel = -3; break;
            case ActionSoundType.WAIT: addLevel = 1; break;
            case ActionSoundType.WALK: addLevel = 6; break;
            case ActionSoundType.SQUAT: addLevel = -2; break;
            case ActionSoundType.SMALLCONFUSION: addLevel = 2; break;
            case ActionSoundType.MEDIUMCONFUSION: addLevel = 3; break;
            case ActionSoundType.LARGECONFUSION: addLevel = 4; break;
            case ActionSoundType.HIDE: addLevel = -2; break;
            case ActionSoundType.DOOROPEN: addLevel = 1; break;
            case ActionSoundType.DEEPBREATH: addLevel = 3; break;
            case ActionSoundType.DASHDOOROPEN: addLevel = 4; break;
            case ActionSoundType.DASH: addLevel = 8; break;
            case ActionSoundType.BREATHLESSNESS: addLevel = 5; break;
            case ActionSoundType.DAMAGE: addLevel = 3; break;
            case ActionSoundType.DAMAGEHALFHEALTH: addLevel = 5; break;
            case ActionSoundType.DAMAGEPINCHHEALTH: addLevel = 8; break;
            case ActionSoundType.BAREFOOT: addLevel = -2; break;
        }

        soundLevel += addLevel;
    }

    /// <summary>
    /// 心音の変更
    /// </summary>
    void ChangeHeartSound()
    {
        if (hideState.IsSafety)
        {
            // 安全地帯内に敵がいて、まだ敵が見えていない状態(消費中)
            HeartSound = HeartSoundType.MEDIUM;

            if (hideState.IsLookEnemy)
            {
                // 安全地帯内に敵がいて、敵が見えている状態(消費大)
                HeartSound = HeartSoundType.LARGE;
            }
        }
        else
        {
            // 安全地帯内に敵がおらず、敵が見えていない状態(消費小)
            HeartSound = HeartSoundType.SMALL;

            // 安全地帯内に敵がおらず、敵が見えている状態
            if (hideState.IsLookEnemy)
            {
                // 安全地帯内に敵がいて姿を見ている状態(消費中)
                HeartSound = HeartSoundType.MEDIUM;
            }
        }
    }
}
