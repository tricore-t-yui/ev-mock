using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSoundType = SoundAreaSpawner.ActionSoundType;

/// <summary>
/// プレイヤーの体力管理クラス
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]
    SoundAreaSpawner soundArea = default;               // 音管理クラス
    [SerializeField]
    PlayerStatusData playerData = default;                    // プレイヤーデータのスクリプタブルオブジェクト

    [SerializeField]
    bool isDebug = false;

    float damageFrame = 0;                              // ダメージを食らってからのフレーム

    public float NowAmount { get; private set; } = 100; // 体力
    public bool IsDamage { get; private set; } = false; // ダメージ所持状態フラグ
    public bool IsDeath { get; private set; } = false;  // 死んでしまったかどうかのフラグ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 初期化
        IsDeath = false;
        IsDamage = false;
        damageFrame = 0;
        NowAmount = 100;
    }

    /// <summary>
    /// 体力回復
    /// </summary>
    public void HealthRecovery()
    {
        if (!IsDeath && !isDebug)
        {
            // ダメージ所持状態フラグがたっている間は回復できない
            if (IsDamage)
            {
                // ダメージフレームをカウント
                damageFrame++;

                // 体力の残量による音の発生
                HealthSound();

                // ダメージフレームが回復フレームより大きくなったら回復開始
                if (playerData.HealthRecoveryFrame <= damageFrame)
                {
                    damageFrame = 0;
                    IsDamage = false;
                }
            }
            else
            {
                NowAmount += playerData.HealthRecoveryAmount;
            }
        }

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);
    }

    /// <summary>
    /// 体力の残量による音の発生
    /// </summary>
    void HealthSound()
    {
        //if (NowAmount <= playerData.SmallDisturbance)
        //{
        //    soundArea.SetSoundLevel(ActionSoundType.DAMAGEHALFHEALTH);
        //    if (NowAmount <= playerData.LargeDisturbance)
        //    {
        //        soundArea.SetSoundLevel(ActionSoundType.DAMAGEPINCHHEALTH);
        //    }
        //}
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage(float damage)
    {
        damageFrame = 0;
        NowAmount -= damage;

        // 体力が0以下になったらゲームオーバー
        if (NowAmount <= 0)
        {
            IsDeath = true;
        }
        // なっていないならダメージ所持状態フラグを立てる
        else
        {
            IsDamage = true;
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetAmount()
    {
        IsDeath = false;
        IsDamage = false;
        damageFrame = 0;
        NowAmount = 100;
    }
}
