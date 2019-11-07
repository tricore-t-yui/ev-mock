using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの体力管理クラス
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
    public float Health { get; private set; } = 0;      // 体力
    public bool IsDeath { get; private set; } = false;  // 死んでしまったかどうかのフラグ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsDeath = false;
        Health = 100;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage(float damage)
    {
        Health -= damage;

        // 体力が0以下になったら
        if (Health <= 0)
        {
            IsDeath = true;
        }
    }
}
