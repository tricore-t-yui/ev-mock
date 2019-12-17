using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ツンのエリアデータ管理クラス
/// </summary>
public class TunAreaDataManager : MonoBehaviour
{
    // エリアデータテーブル
    Dictionary<int, TunAreaData> tunAreaDataTable = new Dictionary<int, TunAreaData>();

    // ハイドコントローラー
    [SerializeField]
    PlayerHideController playerHideController = default;

    [SerializeField]
    int spawnTime = 0;
    int spawnTimeCounter = 0;

    // ツンのオブジェクト
    [SerializeField]
    GameObject tun = default;

    bool isSpawn = false;

    /// <summary>
    /// 起動
    /// </summary>
    private void Awake()
    {
        foreach(TunAreaData areaData in GetComponentsInChildren<TunAreaData>())
        {
            foreach(GameObject hideObject in areaData.HideObject)
            {
                // ハイドオブジェクトとエリアデータをセットで追加
                // note : KeyにハイドオブジェクトのインスタンスIDを指定することで、
                //        そのハイドオブジェクトが属しているエリアデータを取得出来る。
                tunAreaDataTable.Add(hideObject.GetInstanceID(), areaData);
            }
        }
    }

    /// <summary>
    /// ハイドオブジェクトから属しているエリアデータを取得
    /// </summary>
    /// <returns></returns>
    public TunAreaData GetTunAreaData(int hideObjectInstanceId)
    {
        // ハイドオブジェクトが属しているエリアデータを返す
        return tunAreaDataTable[hideObjectInstanceId];
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // ハイドコントローラがオンになったら
        // ツンもオンにする
        // TODO : プレイヤーが隠れてから、一定時間経過後にスポーンするようにする。
        // note : ステートマシンはアクティブなオブジェクトでしか動作しないため
        //        プレイヤーが隠れたかどうかの判定はここで行う。
        if (playerHideController.enabled)
        {
            spawnTimeCounter++;

            if (spawnTime < spawnTimeCounter)
            {
                if (!isSpawn)
                {
                    TunAreaData areaData = GetTunAreaData(playerHideController.HideObj.GetInstanceID());
                    tun.transform.position = areaData.SpawnPos;
                    tun.SetActive(true);
                    isSpawn = true;
                    spawnTimeCounter = 0;
                }
            }
        }
        else
        {
            spawnTimeCounter = 0;
        }

        if (!tun.activeSelf) { isSpawn = false; }
    }
}
