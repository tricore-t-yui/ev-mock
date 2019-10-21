using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの状態を状況に応じて切り替える
/// </summary>
public class EnemyBehaviorSwitcher : MonoBehaviour
{
    // エネミーの視界判定クラス
    [SerializeField]
    EnemyVisibility enemyVisibility = default;

    // 徘徊
    [SerializeField]
    GameObject move = default;

    // 追跡
    [SerializeField]
    GameObject chaser = default;

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // プレイヤーを発見した
        if (enemyVisibility.IsPlayerDiscover())
        {
            // 追跡を開始する
            chaser.SetActive(true);
            // 徘徊を中断する
            move.SetActive(false);
        }
        // プレイヤーを発見していない、もしくは見失った
        else
        {
            // 追跡を諦める
            chaser.SetActive(false);
            // 徘徊を再開する
            move.SetActive(true);
        }
    }
}
