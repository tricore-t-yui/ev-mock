using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の警戒ゾーンに入ったかどうかの判定を行う
/// </summary>
public class WarningAreaEnterChecker : MonoBehaviour
{
    // 入ったかどうか
    public bool isEnter { get; private set; } = false;
    // 入ったオブジェクトの名前
    public string entedObjectName { get; private set; } = null;

    /// <summary>
    /// 衝突した瞬間のコールバック
    /// </summary>
    /// <param name="other">オブジェクトのコライダー</param>
    void OnTriggerEnter(Collider other)
    {
        isEnter = true;
        entedObjectName = other.name;
    }

    /// <summary>
    /// 離れた瞬間のコールバック
    /// </summary>
    /// <param name="other">オブジェクトのコライダー</param>
    void OnTriggerExit(Collider other)
    {
        isEnter = false;
        entedObjectName = null;
    }
}
