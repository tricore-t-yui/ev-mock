using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れる状態管理クラス
/// </summary>
public class HideStateController : MonoBehaviour
{
    /// <summary>
    /// 心音
    /// </summary>
    public enum HeartSoundType
    {
        NORMAL,     // 通常
        SMALL,      // 小
        MEDIUM,     // 中
        LARGE,      // 大
    }

    Renderer enemyRenderer = default;   // 敵のレンダラー

    public bool IsSafety { get; private set; } = false;     // 安全地帯内かどうか
    public bool IsLookEnemy { get; private set; } = false;  // 敵が見えているかどうか
    public HeartSoundType HeartSound { get; private set; } = HeartSoundType.NORMAL;      // 心音

    /// <summary>
    /// トリガーがヒットしたら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            enemyRenderer = other.gameObject.GetComponent<Renderer>();
            IsSafety = true;
        }
    }

    /// <summary>
    /// トリガーがヒットしている間
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            if (enemyRenderer.isVisible)
            {
                IsLookEnemy = true;
            }
            else
            {
                IsLookEnemy = false;
            }
        }
    }

    /// <summary>
    /// トリガーが離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            enemyRenderer = null;
            IsSafety = false;
        }
    }

    /// <summary>
    /// 心音の変更
    /// </summary>
    void ChangeHeartSound()
    {
        if (IsSafety)
        {
            // 安全地帯内に敵がいて、まだ敵が見えていない状態(消費中)
            HeartSound = HeartSoundType.MEDIUM;

            if (IsLookEnemy)
            {
                // 安全地帯内に敵がいて、敵が見えている状態(消費大)
                HeartSound = HeartSoundType.LARGE;
            }
        }
        else
        {
            // 安全地帯内に敵がおらず、敵が見えていない状態(消費小)
            HeartSound = HeartSoundType.SMALL;

            // 安全地帯内に敵がおらず、敵が見えている状態
            if (IsLookEnemy)
            {
                // 安全地帯内に敵がいて姿を見ている状態(消費中)
                HeartSound = HeartSoundType.MEDIUM;
            }
        }
    }
}
