using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーのダメージイベント
/// </summary>
public class PlayerDamageEvent : MonoBehaviour
{
    // プレイヤーのダメージイベント
    class DamageUnityEvent : UnityEvent<Transform, float> { }
    DamageUnityEvent playerDamageEvent = new DamageUnityEvent();

    /// <summary>
    /// コールバックを呼ぶ
    /// </summary>
    public void Invoke(Transform enemyPos,float damege)
    {
        playerDamageEvent.Invoke(enemyPos, damege);
    }

    /// <summary>
    /// コールバック追加
    /// </summary>
    public void Add(UnityAction<Transform,float> call)
    {
        playerDamageEvent.AddListener(call);
    }
}
