using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

/// <summary>
/// 音再生クラス
/// </summary>
public class SoundController : MonoBehaviour
{
    [SerializeField]
    SoundSpawner spawner = default;     // 音のスポナー
    [SerializeField]
    AudioSource audioSource = default;  // オーディオ

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        audioSource.clip = spawner.GetPlaySound();
        audioSource.Play();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (!audioSource.isPlaying || spawner.IsStopSound(audioSource.clip))
        {
            gameObject.SetActive(false);
        }
    }
}
