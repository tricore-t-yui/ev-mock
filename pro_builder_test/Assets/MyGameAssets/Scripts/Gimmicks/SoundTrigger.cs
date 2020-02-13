using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundType = SoundSpawner.SoundType;

/// <summary>
/// トリガーに触れたとき音を出す
/// </summary>
public class SoundTrigger : MonoBehaviour
{
    [SerializeField]
    SoundSpawner spawner = default;    // 音のスポナー
    [SerializeField]
    SoundType soundType = default;     // 音のタイプ

    /// <summary>
    /// トリガーに触れたとき
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        spawner.Play(soundType);
    }
}