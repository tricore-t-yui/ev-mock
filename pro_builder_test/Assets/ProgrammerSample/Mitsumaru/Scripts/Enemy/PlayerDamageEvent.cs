using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーのダメージイベント
/// </summary>
public class PlayerDamageEvent : MonoBehaviour
{
    /// <summary>
    /// ダメージの種類
    /// </summary>
    public enum DamageType
    {
        Normal,    // 通常
        Locker,    // ロッカー
        Bed,       // ベッド
    }

    // プレイヤーのダメージイベント
    class DamageUnityEvent : UnityEvent<Vector3, float> { }
    DamageUnityEvent playerDamageEvent = new DamageUnityEvent();
    DamageUnityEvent playerDamageFromLockerEvent = new DamageUnityEvent();
    DamageUnityEvent playerDamageFromBedEvent = new DamageUnityEvent();

    /// <summary>
    /// コールバックを呼ぶ
    /// </summary>
    public void Invoke(DamageType type,Vector3 enemyPos,float damege)
    {
        // 通常
        if (type == DamageType.Normal)
        {
            playerDamageEvent?.Invoke(enemyPos, damege);
        }
        // ロッカー
        else if (type == DamageType.Locker)
        {
            playerDamageFromLockerEvent?.Invoke(enemyPos, damege);
        }
        // ベッド
        else if (type == DamageType.Bed)
        {
            playerDamageFromBedEvent?.Invoke(enemyPos, damege);
        }
    }

    /// <summary>
    /// コールバック追加
    /// </summary>
    public void Add(DamageType type,UnityAction<Vector3,float> call)
    {
        // 通常
        if (type == DamageType.Normal)
        {
            playerDamageEvent?.AddListener(call);
        }
        // ロッカー
        else if (type == DamageType.Locker)
        {
            playerDamageFromLockerEvent?.AddListener(call);
        }
        // ベッド
        else if (type == DamageType.Bed)
        {
            playerDamageFromBedEvent?.AddListener(call);
        }
    }
}
