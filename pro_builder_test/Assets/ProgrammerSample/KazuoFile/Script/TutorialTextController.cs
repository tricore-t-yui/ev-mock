using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアルテキストクラス
/// </summary>
public class TutorialTextController : MonoBehaviour
{
    /// <summary>
    /// 開始条件
    /// </summary>
    enum StartCondition
    {
        TRIGGER,
        DOOR,
        HIDE,
    }
    [SerializeField]
    PlayerDoorController doorController = default;
    [SerializeField]
    PlayerHideController hideController = default;

    [SerializeField]
    Text tutorialText = default;                    // チュートリアルテキスト

    [SerializeField]
    TutorialTrrigerContoller startTriger = default; // チュートリアル開始トリガー
    [SerializeField]
    TutorialTrrigerContoller endTriger = default;   // チュートリアル終了トリガー
    [SerializeField]
    [TextArea(1, 3)]
    [Tooltip("チュートリアルが開始したときに表示したいテキストを入力してください。")]
    string startText = default;                     // チュートリアルが開始したときのテキスト
    [SerializeField]
    [TextArea(1, 3)]
    [Tooltip("チュートリアルが終了したときに表示したいテキストを入力してください。")]
    string endText = default;                       // チュートリアルが終了したときのテキスト

    [SerializeField]
    [Tooltip("開始条件を選んでください。 " +
        "TRIGGERはトリガーで判断します。 " +
        "DOORはドアにインタラクトしているか、していないかで判断します。 " +
        "LOCKERはロッカーにインタラクトしているか、していないかで判断します。 " +
        "BEDはベッドにインタラクトしているか、していないかで判断します。 ")]
    StartCondition startCondition = default;        // 開始条件

    bool isStart = false;                           // チュートリアル開始フラグ
    bool isEnd = false;                             // チュートリアル終了フラグ

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (!isEnd)
        {
            switch (startCondition)
            {
                case StartCondition.TRIGGER:

                    // 開始トリガーにふれたらチュートリアル開始
                    if (startTriger.IsHit && !isStart)
                    {
                        tutorialText.text = startText;
                        isStart = true;
                    }
                    // 終了トリガーにふれたらチュートリアル終了
                    if (endTriger.IsHit && isStart)
                    {
                        tutorialText.text = endText;
                        isEnd = true;
                    }
                    break;
                case StartCondition.DOOR:
                    // 開始トリガーにふれたらチュートリアル開始
                    if (startTriger.IsHit)
                    {
                        if (doorController.enabled)
                        {
                            tutorialText.text = startText;
                        }
                        else
                        {
                            tutorialText.text = endText;
                        }
                    }
                    // 終了トリガーにふれたらチュートリアル終了
                    if (endTriger.IsHit)
                    {
                        isEnd = true;
                    }
                    break;
                case StartCondition.HIDE:
                    // 開始トリガーにふれたらチュートリアル開始
                    if (startTriger.IsHit && !isStart)
                    {
                        if (hideController.enabled)
                        {
                            tutorialText.text = endText;
                            isStart = true;
                        }
                        else
                        {
                            tutorialText.text = startText;
                        }
                    }
                    if ((isStart && !hideController.enabled) || endTriger.IsHit)
                    {
                        tutorialText.text = "";
                        isEnd = true;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// テキストのリセット
    /// </summary>
    public void TextReset()
    {
        tutorialText.text = "";
        startTriger.TriggerReset();
        endTriger.TriggerReset();
        isStart = false;
        isEnd = false;
    }
}
