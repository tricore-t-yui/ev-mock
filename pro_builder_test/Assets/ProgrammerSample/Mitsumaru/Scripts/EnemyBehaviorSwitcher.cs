using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// エネミーの状態を状況に応じて切り替える
/// </summary>
public class EnemyBehaviorSwitcher : MonoBehaviour
{
    // ナビメッシュ
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    // 徘徊に戻るポイントを管理したクラス
    [SerializeField]
    EnemyReturnPointList returnPointList = default;

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
        // プレイヤーを発見した瞬間
        if (enemyVisibility.IsPlayerDiscoverMoment())
        {
            // 追跡を開始する
            chaser.SetActive(true);
            // 徘徊を中断する
            move.SetActive(false);
        }
        
        // プレイヤーを見失った瞬間
        if (enemyVisibility.IsPlayerDiscoverExit())
        {
            // 徘徊を再開する
            move.SetActive(true);
            // 追跡を諦める
            chaser.SetActive(false);

            // 徘徊に戻る一番近いポイントを取得
            ReturnPointData returnPointData = returnPointList.GetNearReturnPoint(transform.root);
            // 取得したポイントを次の目標位置に設定
            navMeshAgent.SetDestination(returnPointData.position);
        }
    }
}
