using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// トリガーの画面エフェクト管理クラス
/// </summary>
public class TriggerEffectController : MonoBehaviour
{
    [SerializeField]
    TriggerEffectSpawner spawner = default;    // トリガーエフェクトのスポナー
    [SerializeField]
    Image image = default;                     // トリガーエフェクト
    [SerializeField]
    float waitSecond = 3;                      // フェードアウトするまでの時間

    float alpha = 0;                           // トリガーエフェクトのアルファ値

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 初期化してフェードイン開始
        image.sprite = spawner.displaySprite;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        alpha = 0;

        StartCoroutine(FadeInTriggerEffect());
    }

    /// <summary>
    /// フェードインのコルーチン
    /// </summary>
    IEnumerator FadeInTriggerEffect()
    {
        while (alpha < 1)
        {
            image.color += new Color(0, 0, 0, 0.01f);
            alpha += 0.01f;
            yield return null;
        }

        yield return new WaitForSeconds(waitSecond);

        StartCoroutine(FadeOutTriggerEffect());
    }

    /// <summary>
    /// フェードアウトのコルーチン
    /// </summary>
    IEnumerator FadeOutTriggerEffect()
    {
        while (alpha > 0)
        {
            image.color -= new Color(0, 0, 0, 0.01f);
            alpha -= 0.01f;
            yield return null;
        }

        // フェードアウトし終わったら消す
        gameObject.SetActive(false);
    }
}
