using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音管理クラス
/// </summary>
public class SoundAreaController : MonoBehaviour
{
    [SerializeField]
    SphereCollider collider = default;  // 音発生の領域
    [SerializeField]
    float areaMagnification = 0.3f;     // 拡大倍率

    float soundLevel = 0;               // 音量レベル
    public float TotalSoundLevel { get; private set; } = 0;  // 音量レベルの合計

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 合計値に適応、レベルリセット
        TotalSoundLevel = soundLevel;
        soundLevel = 0;

        // 合計値に応じて領域拡大
        collider.radius = 1 + (areaMagnification * soundLevel);
    }

    /// <summary>
    /// 音のレベルの加算
    /// </summary>
    public void ChangeSoundLevel(float level)
    {
        soundLevel += level;
    }
}
