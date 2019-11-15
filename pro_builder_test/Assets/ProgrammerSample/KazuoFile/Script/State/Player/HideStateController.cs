using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れる状態管理クラス
/// </summary>
public class HideStateController : MonoBehaviour
{
    Renderer enemyRenderer = default;   // 敵のレンダラー

    public bool IsSafety { get; private set; } = false;     // 安全地帯内かどうか
    public bool IsLookEnemy { get; private set; } = false;  // 敵が見えているかどうか

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
}
