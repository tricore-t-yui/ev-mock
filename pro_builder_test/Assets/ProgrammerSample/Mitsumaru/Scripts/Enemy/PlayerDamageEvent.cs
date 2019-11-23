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
    class DamageUnityEvent : UnityEvent<Vector3, float> { }
    DamageUnityEvent playerDamageEvent = new DamageUnityEvent();

    /// <summary>
    /// コールバックを呼ぶ
    /// </summary>
    public void Invoke(Vector3 enemyPos,float damege)
    {
        playerDamageEvent?.Invoke(enemyPos,damege);
    }

    /// <summary>
    /// コールバック追加
    /// </summary>
    public void Add(UnityAction<Vector3,float> call)
    {
        playerDamageEvent.AddListener(call);
    }
}
