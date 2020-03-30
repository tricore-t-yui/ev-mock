﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveType = PlayerStateController.ActionStateType;

/// <summary>
/// プレイヤーのステータス管理クラス
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    [SerializeField]
    PlayerBreathController breathController = default;              // 息クラス
    [SerializeField]
    PlayerHealthController healthController = default;              // 体力クラス
    [SerializeField]
    playerStaminaController staminaController = default;            // スタミナクラス
    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;  // オブジェクトダメージクラス

    [SerializeField]
    bool isUseStamina = false;                                     // スタミナを使うかどうか

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType type, bool isSquat)
    {
        breathController.StateUpdate(type, isUseStamina);
        objectDamageController.StateUpdate(type);
        healthController.HealthRecovery();
        if (isUseStamina)
        {
            staminaController.StateUpdate(type, isSquat);
        }
    }

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void DeepBreathRecovery()
    {
        breathController.DeepBreathRecovery();
        objectDamageController.DeepBreathRecovery();
        healthController.HealthRecovery();
        if (isUseStamina)
        {
            staminaController.DeepBreathRecovery();
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetStatus()
    {
        breathController.ResetAmount();
        objectDamageController.ResetDamage();
        healthController.ResetAmount();
        if (isUseStamina)
        {
            staminaController.ResetAmount();
        }
    }
}