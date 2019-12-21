using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間の生成クラス
/// </summary>
public class KageSpawn : MonoBehaviour
{
    [SerializeField]
    AreaManager areaManager = default;          // エリアマネージャー
    [SerializeField]
    GameController gameController = default;    // ゲームコントローラー
    [SerializeField]
    GameObject[] kage = default;                // 影人間

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        foreach(var item in kage)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (!gameController.IsReturn)
        {
            switch (areaManager.GetExistAreaToCharacter("Player"))
            {
                case "Area21": kage[0].SetActive(true); kage[1].SetActive(true); break;
            }
        }
        else
        {
            switch (areaManager.GetExistAreaToCharacter("Player"))
            {
                case "Area08": kage[0].SetActive(true); kage[1].SetActive(true); break;
                case "Area11": kage[2].SetActive(true); break;
                case "Area21": kage[3].SetActive(true); kage[4].SetActive(true); break;
                case "Area10": kage[5].SetActive(true); break;
            }
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetKage()
    {
        foreach (var item in kage)
        {
            item.SetActive(false);
        }
    }
}
