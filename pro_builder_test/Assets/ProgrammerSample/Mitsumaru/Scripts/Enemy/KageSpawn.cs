using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間の生成クラス
/// </summary>
public class KageSpawn : MonoBehaviour
{
    [SerializeField]
    AreaManager areaManager = default;  // エリアマネージャー
    [SerializeField]
    GameObject[] kage = default;        // 影人間

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
        switch(areaManager.GetExistAreaToCharacter("Player"))
        {
            case "Area05": kage[0].SetActive(true); break;
            case "Area08": kage[1].SetActive(true); break;
            case "Area12": kage[2].SetActive(true); break;
            case "Area13": kage[3].SetActive(true); break;
            case "Area21": kage[4].SetActive(true); kage[5].SetActive(true); break;
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
