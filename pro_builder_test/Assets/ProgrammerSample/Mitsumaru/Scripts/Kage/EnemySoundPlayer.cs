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
    /// 更新
    /// </summary>
    void Update()
    {
        // 再生中のサウンドを調べて、終了しているものはオブジェクトをオフにする
        foreach(var sound in sounds)
        {
            if (sound.gameObject.activeSelf)
            {
                if (!sound.isPlaying)
                {
                    sound.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// サウンドの再生
    /// </summary>
    /// <param name="soundName"></param>
    public void Play(string soundName)
    {
        // オブジェクトをオンにする
        transform.Find(soundName).gameObject.SetActive(true);
    }
}
