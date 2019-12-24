using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間の生成クラス
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemy = default;   // 敵

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        ResetEnemy();
    }

    /// <summary>
    /// 当たったら敵を生成
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            foreach (var item in enemy)
            {
                item.SetActive(true);
            }
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetEnemy()
    {
        foreach (var item in enemy)
        {
            item.SetActive(false);
        }
    }
}
