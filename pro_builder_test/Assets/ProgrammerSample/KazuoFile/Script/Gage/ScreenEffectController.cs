using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面エフェクト管理クラス
/// </summary>
public class ScreenEffectController : MonoBehaviour
{
    /// <summary>
    /// イメージのタイプ
    /// </summary>
    enum ImageType
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
        breathEffect.color = DisplayEffect(ImageType.BREATH);
        healthEffect.color = DisplayEffect(ImageType.HEALTH);
        staminaEffect.color = DisplayEffect(ImageType.STAMINA);
        objectDamageEffect.color = DisplayEffect(ImageType.OBJECTDAMAGE);
    }

    /// <summary>
    /// エフェクト表示
    /// </summary>
    Color DisplayEffect(ImageType type)
    {
        // イメージのカラー
        Color color = new Color(0,0,0);

        switch(type)
        {
            case ImageType.BREATH:
                color = breathEffect.color;
                color.a = 1.0f - (breathController.NowAmount / 100);
                break;
            case ImageType.HEALTH:
                color = healthEffect.color;
                color.a = 1.0f - (healthController.NowAmount / 100);
                break;
            case ImageType.STAMINA:
                color = staminaEffect.color;
                color.a = 0.02f - (staminaController.NowAmount / 5000);
                break;
            case ImageType.FADEOUT:
                color = fadeOut.color;
                color.a = 1;
                break;
            case ImageType.OBJECTDAMAGE:
                color = objectDamageEffect.color;
                color.a = objectDamageController.NowDamage / 100;
                break;
        }

        return color;
    }
}
