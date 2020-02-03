using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アクションテキスト表示クラス
/// </summary>
public class ActionText : MonoBehaviour
{
    [SerializeField]
    PlayerStateController stateController = default;    // 状態管理クラス

    [SerializeField]
    Text[] texts = default;                             // テキスト

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        foreach (var item in texts)
        {
            item.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if(stateController.IsCanHide())
        {
            texts[0].gameObject.SetActive(true);
        }
        else
        {
            texts[0].gameObject.SetActive(false);
        }
    }
}
