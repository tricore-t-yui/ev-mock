using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステータスの画面エフェクト管理クラス
/// </summary>
public class StatusEffectController : MonoBehaviour
{
    /// <summary>
    /// エフェクトのタイプ
    /// </summary>
    enum EffectType
    {
        BREATH,
        HEALTH,
        STAMINA,
        FADEOUT,
        OBJECTDAMAGE,
    }

    [SerializeField]
    PlayerBreathController breathController = default;              // 息クラス
	[SerializeField]
	PlayerHealthController healthController = default;              // 体力クラス
    [SerializeField]
    playerStaminaController staminaController = default;            // スタミナクラス
    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;  // オブジェクトダメージクラス

    [SerializeField]
    Image breathEffect = default;                                   // 息の画面エフェクト
    [SerializeField]
    Image healthEffect = default;                                   // 体力の画面エフェクト
    [SerializeField]
    Image staminaEffect = default;                                  // スタミナの画面エフェクト
    [SerializeField]
    Image fadeOut = default;                                        // フェードアウト用イメージ
    [SerializeField]
    Image objectDamageEffect = default;                             // オブジェクトダメージの画面エフェクト

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // それぞれの状態に応じてによってエフェクト表示
        breathEffect.color = DisplayEffect(EffectType.BREATH);
        healthEffect.color = DisplayEffect(EffectType.HEALTH);
        staminaEffect.color = DisplayEffect(EffectType.STAMINA);
        objectDamageEffect.color = DisplayEffect(EffectType.OBJECTDAMAGE);
    }

    /// <summary>
    /// エフェクト表示
    /// </summary>
    Color DisplayEffect(EffectType type)
    {
        // イメージのカラー
        Color color = new Color(0,0,0);
        // 75 /? = 0.5
        switch(type)
        {
            case EffectType.BREATH:
                color = breathEffect.color;
                if (breathController.NowAmount <= 75)
                {
                    color.a = 0.75f - (breathController.NowAmount / 75);
                }
                else
                {
                    color.a = 0;
                }
                break;
            case EffectType.HEALTH:
                color = healthEffect.color;
                color.a = 1.0f - (healthController.NowAmount / 100);
                break;
            case EffectType.STAMINA:
                color = staminaEffect.color;
                color.a = 0.02f - (staminaController.NowAmount / 5000);
                break;
            case EffectType.FADEOUT:
                color = fadeOut.color;
                color.a = 1;
                break;
            case EffectType.OBJECTDAMAGE:
                color = objectDamageEffect.color;
                color.a = objectDamageController.NowDamage / 100;
                break;
        }

        return color;
    }
}
