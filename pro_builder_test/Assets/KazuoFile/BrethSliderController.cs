using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 息ゲージ管理クラス
/// </summary>
public class BrethSliderController : MonoBehaviour
{
    [SerializeField]
    PlayerBrethController brethController = default;    // 息管理クラス
    [SerializeField]
    Slider slider = default;                       // 息ゲージ

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ResidualAmountが0 ~ 100なので、100で割って0.00 ~ 1.00に変換して値を更新
        slider.value = brethController.ResidualAmount / 100;
    }
}
