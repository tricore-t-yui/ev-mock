using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面エフェクト管理クラス
/// </summary>
public class ScreenEffectController : MonoBehaviour
{
    [SerializeField]
    PlayerBreathController breathController = default;    // 息管理クラス
	[SerializeField]
	PlayerHealthController healthController = default;  // 体力クラス

    [SerializeField]
    Image breathEffect = default;   // 息の画面エフェクト
    [SerializeField]
    Image heathEffect = default;    // 体力の画面エフェクト

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 息、体力によってエフェクト表示
        DisplayEffect();
    }

    /// <summary>
    /// エフェクト表示
    /// </summary>
    void DisplayEffect()
    {
        // イメージのカラー
        Color color = default;

        // 息
        color = breathEffect.color;
        color.a = 1.0f - (breathController.NowAmount / 100);
        breathEffect.color = color;

        // 体力
        color = heathEffect.color;
        color.a = 1.0f - (healthController.NowAmount / 100);
        heathEffect.color = color;
    }
}
