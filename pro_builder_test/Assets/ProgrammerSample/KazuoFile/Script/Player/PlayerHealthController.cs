using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの体力管理クラス
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
    public float Health { get; private set; } = 0;  // 体力

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        Health = 100;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 体力が0以下になったら
        if (Health <= 0)
        {
            // ゲームオーバー
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage(float damage)
    {
        Health -= damage;
    }
}
