using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    PlayerStateController stateController = default;    // ステート管理クラスz

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
            case SoundType.Walk: WalkVolume();break;
            case SoundType.Dash:DashVolume();break;
            case SoundType.DamageObject: DamageObjectVolume();break;
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
            case HeartSoundType.NORMAL: audioSource.volume = 0.25f; audioSource.pitch = 0.75f; break;
            case HeartSoundType.MEDIUM: audioSource.volume = 0.5f; audioSource.pitch = 0.75f; break;
            case HeartSoundType.LARGE: audioSource.volume = 0.75f; audioSource.pitch = 1.5f; break;
        }
    }

    /// <summary>
    /// 息の音の大きさ
    /// </summary>
    void BreathVolume()
    {
        switch (breathController.State)
        {
            case BrethState.NOTCONFUSION: audioSource.volume = 0.01f; audioSource.pitch = 1f; break;
            case BrethState.SMALLCONFUSION: audioSource.volume = 0.1f; audioSource.pitch = 1.25f; break;
            case BrethState.MEDIUMCONFUSION: audioSource.volume = 0.2f; audioSource.pitch = 1.5f; break;
            case BrethState.LARGECONFUSION: audioSource.volume = 0.4f; audioSource.pitch = 1.75f; break;
            case BrethState.BREATHLESSNESS: audioSource.volume = 0.5f; audioSource.pitch = 2f; break;
        }
    }

    /// <summary>
    /// 歩き音の大きさ
    /// </summary>
    void WalkVolume()
    {
        if(stateController.IsSquat)
        {
            audioSource.volume = 0.25f;
        }
        else
        {
            audioSource.volume = 0.5f;
        }
    }

    /// <summary>
    /// 走る音の大きさ
    /// </summary>
    void DashVolume()
    {
        audioSource.volume = 0.5f + 0.1f * Random.Range(-1, 1);
        audioSource.pitch = 1 + 0.05f * Random.Range(-1, 1);
    }

    /// <summary>
    /// ダメージオブジェクトを踏んだときの大きさ
    /// </summary>
    void DamageObjectVolume()
    {
        audioSource.volume = 0.25f;
    }
}
