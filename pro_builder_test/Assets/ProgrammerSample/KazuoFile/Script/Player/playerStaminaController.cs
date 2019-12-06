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
    float dashDecrement = 0.5f;                             // 息止め時の息消費量
                                                            
    [SerializeField]                                        
    float normalRecovery = 1f;                              // 通常の息の回復量
    [SerializeField]                                        
    float walkRecovery = 0.5f;                              // 通常の息の回復量
    [SerializeField]                                        
    float squatRecovery = 0.5f;                             // 通常の息の回復量
    [SerializeField]                                        
    float deepBreathRecovery = 0.15f;                        // 深呼吸時の息の回復量

    public float NowAmount { get; private set; } = 100;     // 現在量
    public bool IsDisappear { get; private set; } = false;  // スタミナ切れフラグ

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType type, bool isSquat)
    {
        // 各ステートに合わせた処理を実行
        switch (type)
        {
            case MoveType.WAIT: NowAmount += normalRecovery; SquatRecovery(isSquat); break;
            case MoveType.WALK: NowAmount += walkRecovery; SquatRecovery(isSquat); break;
            case MoveType.DASH: NowAmount -= dashDecrement; break;
            default: break;
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
            NowAmount += squatRecovery;
        }
    }

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void DeepBreathRecovery()
    {
        NowAmount += deepBreathRecovery;

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
}