using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアルトリガー管理クラス
/// </summary>
public class TutorialTriggerManager : MonoBehaviour
{
    [SerializeField]
    TutorialTextController[] tutorialTextController = default;  // チュートリアルテキストクラス

    /// <summary>
    /// トリガーのリセット
    /// </summary>
    public void TriggerReset()
    {
        foreach (var item in tutorialTextController)
        {
            item.TextReset();
        }
    }
}
