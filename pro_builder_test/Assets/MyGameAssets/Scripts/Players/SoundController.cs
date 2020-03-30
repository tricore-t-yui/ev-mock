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
    public void Spawned(SoundType type, AudioClip clip)
    {
        soundType = type;
        switch (soundType)
        {
            case SoundType.Walk: WalkVolume(); break;
            case SoundType.Dash: DashVolume(); break;
            case SoundType.DamageObject: DamageObjectVolume(); break;
            default: break;
        }
        audioSource.clip = clip;
        gameObject.SetActive(true);
        audioSource.Play();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        switch (soundType)
        {
            case SoundType.Walk: WalkVolume(); break;
            case SoundType.Dash: DashVolume(); break;
            case SoundType.DamageObject: DamageObjectVolume(); break;
            default: break;
        }
        if (!audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 歩き音の大きさ
    /// </summary>
    void WalkVolume()
    {
        if(stateController.IsSquat)
        {
            audioSource.volume = 0.02f * 0.5f;
        }
        else
        {
            audioSource.volume = 0.02f;
        }
        audioSource.pitch = 0.85f + 0.05f * Random.Range(-1, 1);
    }

    /// <summary>
    /// 走る音の大きさ
    /// </summary>
    void DashVolume()
    {
        audioSource.volume = 0.045f + 0.005f * Random.Range(-1, 1);
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
