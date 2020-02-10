using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectType = TriggerEffectSpawner.EffectType;

/// <summary>
/// トリガーエフェクト表示のトリガー
/// </summary>
public class ScreenEffectTrigger : MonoBehaviour
{
    [SerializeField]
    TriggerEffectSpawner spawner = default;    // トリガーエフェクトのスポナー
    [SerializeField]
    EffectType effectType = default;           // トリガーエフェクトの種類

    /// <summary>
    /// トリガーに触れたとき
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        spawner.Spawn(effectType);
    }
}
