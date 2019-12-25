using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵関連のサウンドの再生クラス
/// </summary>
public class EnemySoundPlayer : MonoBehaviour
{
    // サウンドリスト
    [SerializeField]
    AudioSource[] sounds = default;

    /// <summary>
    /// サウンドの再生
    /// </summary>
    /// <param name="soundName"></param>
    public void Play(string soundName)
    {
        AudioSource audio = System.Array.Find(sounds, sound => sound.gameObject.name == soundName);
        audio.Play();
    }

    /// <summary>
    /// 指定のサウンドが再生中かどうか
    /// </summary>
    public bool IsPlaying(string soundName)
    {
        // オーディオ取得
        AudioSource audio = System.Array.Find(sounds, sound => sound.gameObject.name == soundName);
        return audio.isPlaying;
    }
}
