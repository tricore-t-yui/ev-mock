using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 体力ゲージ管理クラス
/// </summary>
public class HealthSliderController : MonoBehaviour
{
    [SerializeField]
    PlayerHealthController healthController = default;// プレイヤーの状態クラス
    [SerializeField]
    Slider slider = default;                        // 息ゲージ

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ResidualAmountが0 ~ 100なので、100で割って0.00 ~ 1.00に変換して値を更新
        slider.value = healthController.Health / 100;
    }
}
