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
        FADEOUT,
    }

    [SerializeField]
    PlayerBreathController breathController = default;    // 息管理クラス
	[SerializeField]
	PlayerHealthController healthController = default;  // 体力クラス

    [SerializeField]
    Image breathEffect = default;   // 息の画面エフェクト
    [SerializeField]
    Image healthEffect = default;    // 体力の画面エフェクト
    [SerializeField]
    Image fadeOut = default;        // フェードアウト用イメージ

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 息、体力によってエフェクト表示
        breathEffect.color = DisplayEffect(ImageType.BREATH);
        healthEffect.color = DisplayEffect(ImageType.HEALTH);

        // 死んでしまったら
        if (healthController.IsDeath)
        {
            //fadeOut.color = DisplayEffect(ImageType.FADEOUT);
        }
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
            case ImageType.FADEOUT:
                color = fadeOut.color;
                color.a += 0.01f;
                break;
        }

        return color;
    }
}
