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
            // 各ステートに合わせた処理を実行
            switch (type)
            {
                case MoveType.WALK: NowAmount += playerData.StaminaWalkRecovery; SquatRecovery(isSquat); break;
                case MoveType.DASH: NowAmount -= playerData.StaminaDecrement; break;
                default: NowAmount += playerData.StaminaNormalRecovery; SquatRecovery(isSquat); break;
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
            NowAmount += playerData.StaminaSquatRecovery;
        }
    }

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void DeepBreathRecovery()
    {
        NowAmount += playerData.StaminaDeepBreathRecovery;

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
    /// リセット
    /// </summary>
    public void ResetAmount()
    {
        IsDisappear = false;
        NowAmount = 100;
    }
}