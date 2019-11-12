using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲージ管理クラス
/// </summary>
public class SliderController : MonoBehaviour
{
    /// <summary>
    /// ゲージのタイプ
    /// </summary>
    enum SliderType
    {
        BRETH,  // 息
        HEALTH, // 体力
    }

    [SerializeField]
    PlayerBreathController breathController = default;    // 息管理クラス
    [SerializeField]
    PlayerHealthController healthController = default;  // 体力クラス

    [SerializeField]
    Slider brethSlider = default;                       // 息ゲージ
    [SerializeField]
    Slider healthSlider = default;                      // 体力ゲージ

    [SerializeField]
    GageBlink brethSliderBlink = default;               // 息ゲージの点滅クラス
    [SerializeField]
    GageBlink healthSliderBlink = default;              // 体力ゲージの点滅クラス

    [SerializeField, Range(0, 1)]
    float blinkPercent = 0.25f;                         // この値以下になったら点滅

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 値がが0 ~ 100なので、100で割って0.00 ~ 1.00に変換して値を更新
        brethSlider.value = breathController.NowAmount / 100;
        healthSlider.value = healthController.NowAmount / 100;

        // 点滅処理
        Blink();
    }

    /// <summary>
    /// 点滅処理
    /// </summary>
    void Blink()
    {
        // 体力ゲージの値が点滅させる基準を下回ったら点滅開始、上回ったら点滅終了
        if (healthSlider.value <= blinkPercent)
        {
            healthSliderBlink.enabled = true;
        }
        else
        {
            healthSliderBlink.enabled = false;
        }

        // 息ゲージも点滅
        if(brethSlider.value <= blinkPercent)
        {
            brethSliderBlink.enabled = true;
        }
        else
        {
            brethSliderBlink.enabled = false;
        }
    }
}
