using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveType = PlayerStateController.ActionStateType;

/// <summary>
/// スタミナ管理クラス
/// </summary>
public class playerStaminaController : MonoBehaviour
{
    [SerializeField]
    PlayerStatusData playerData = default;                        // プレイヤーのデータのスクリプタブルオブジェクト
    [SerializeField]
    PlayerBreathController breathController = default;            // 息クラス

    [SerializeField]
    bool isDebug = false;

    public float NowAmount { get; private set; } = 100;     // 現在量
    public bool IsDisappear { get; private set; } = false;  // スタミナ切れフラグ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsDisappear = false;
        NowAmount = 100;
    }

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType type, bool isSquat)
    {
        if (!isDebug)
        {
            Debug.Log(RecoveryMagnification());
            // 各ステートに合わせた処理を実行
            switch (type)
            {
                case MoveType.DASH: NowAmount -= playerData.StaminaDecrement * ConsumptionMagnification(); break;
                default: NowAmount += playerData.StaminaNormalRecovery * RecoveryMagnification(); SquatRecovery(isSquat); break;
            }
        }

        // スタミナの状態変更
        ChangeState();

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);
    }

    /// <summary>
    /// しゃがみ回復
    /// </summary>
    void SquatRecovery(bool flag)
    {
        if (flag)
        {
            NowAmount += playerData.StaminaSquatRecovery * RecoveryMagnification();
        }
    }

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void DeepBreathRecovery()
    {
        NowAmount += playerData.StaminaDeepBreathRecovery * RecoveryMagnification() ;

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);
    }

    /// <summary>
    /// スタミナの状態変更
    /// </summary>
    void ChangeState()
    {
        if (!IsDisappear && NowAmount <= 0)
        {
            IsDisappear = true;
        }
        if (IsDisappear && NowAmount >= 100)
        {
            IsDisappear = false;
        }
    }

    /// <summary>
    /// 息の状態による回復倍率
    /// </summary>
    float RecoveryMagnification()
    {
        if(breathController.NowAmount >= 50)
        {
            return 1;
        }
        else
        {
            return 0.3f;
        }
    }

    /// <summary>
    /// 息の状態による消費倍率
    /// </summary>
    float ConsumptionMagnification()
    {
        if (breathController.NowAmount >= 50)
        {
            return 1;
        }
        else
        {
            return 1.7f;
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetAmount()
    {
        IsDisappear = false;
        NowAmount = 100;
    }
}