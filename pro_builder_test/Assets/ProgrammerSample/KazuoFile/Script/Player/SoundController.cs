using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using SoundType = SoundSpawner.SoundType;
using HeartSoundType = HideStateController.HeartSoundType;
using BrethState = PlayerBreathController.BrethState;

/// <summary>
/// 音再生クラス
/// </summary>
public class SoundController : MonoBehaviour
{
    [SerializeField]
    SoundSpawner spawner = default;                     // 音のスポナー
    [SerializeField]
    AudioSource audioSource = default;                  // オーディオ
    [SerializeField]
    HideStateController hideStateController = default;  // 隠れる状態管理クラス
    [SerializeField]
    PlayerBreathController breathController = default;  // 息管理クラス

    SoundType soundType = default;                      // 音のタイプ

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        soundType = spawner.GetSoundType();
        audioSource.clip = spawner.GetPlaySound();
        audioSource.loop = spawner.GetIsLoop();
        audioSource.Play();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        switch(soundType)
        {
            case SoundType.Breth: BreathVolume(); break;
            case SoundType.HeartSound: HeartSoundVolume(); break;
            default:break;
        }

        if (!audioSource.isPlaying || spawner.IsStopSound(audioSource.clip))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 心音の大きさ
    /// </summary>
    void HeartSoundVolume()
    {
        switch(hideStateController.HeartSound)
        {
            case HeartSoundType.NORMAL: audioSource.pitch = 0.25f; break;
            case HeartSoundType.MEDIUM: audioSource.pitch = 0.5f; break;
            case HeartSoundType.LARGE: audioSource.pitch = 1f; break;
        }
    }

    /// <summary>
    /// 息の大きさ
    /// </summary>
    void BreathVolume()
    {
        switch (breathController.State)
        {
            case BrethState.NOTCONFUSION: audioSource.volume = 0.1f; break;
            case BrethState.SMALLCONFUSION: audioSource.volume = 0.25f; break;
            case BrethState.MEDIUMCONFUSION: audioSource.volume = 0.5f; break;
            case BrethState.LARGECONFUSION: audioSource.volume = 0.75f; break;
            case BrethState.BREATHLESSNESS: audioSource.volume = 1; break;
        }
    }
}
