using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの体力管理クラス
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]
    SoundAreaController soundArea = default;            // 音管理クラス

    [SerializeField]
    float recoveryFrame = 240;                          // 回復が始まるまでのフレーム数
    [SerializeField]
    float recoveryAmount = 0.1f;                        // 回復量
    [SerializeField]
    float smallDisturbance = 75;                        // 息の乱れ(小)の基準値
    [SerializeField]
    float largeDisturbance = 50;                        // 息の乱れ(大)の基準値

    float damageFrame = 0;                              // ダメージを食らってからのフレーム
    bool isDamage = false;                              // ダメージ所持状態フラグ

    public float NowAmount { get; private set; } = 100; // 体力
    public bool IsDeath { get; private set; } = false;  // 死んでしまったかどうかのフラグ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 初期化
        IsDeath = false;
        isDamage = false;
        damageFrame = 0;
        NowAmount = 100;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 回復
        Recovery();

        // 体力の残量による音の発生
        HealthSound();

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);
    }

    /// <summary>
    /// 体力回復
    /// </summary>
    void Recovery()
    {
        // ダメージ所持状態フラグがたっている間は回復できない
        if (isDamage)
        {
            // ダメージフレームをカウント
            damageFrame++;

            // ダメージフレームが回復フレームより大きくなったら回復開始
            if (recoveryFrame <= damageFrame)
            {
                damageFrame = 0;
                isDamage = false;
            }
        }
        else
        {
            NowAmount += recoveryAmount;
        }
    }

    /// <summary>
    /// 体力の残量による音の発生
    /// </summary>
    void HealthSound()
    {
        if (NowAmount <= smallDisturbance)
        {
            soundArea.ChangeSoundLevel(5);
            if (NowAmount <= largeDisturbance)
            {
                soundArea.ChangeSoundLevel(7);
            }
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage(float damage)
    {
        NowAmount -= damage;

        // 体力が0以下になったらゲームオーバー
        if (NowAmount <= 0)
        {
            IsDeath = true;
        }
        // なっていないならダメージ所持状態フラグを立てる
        else
        {
            isDamage = true;
        }
    }
}
