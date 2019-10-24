using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 徘徊に戻るポイントの情報
[System.Serializable]
public class ReturnPointData
{
    public Vector3 position;       // 位置
    [HideInInspector]
    public float distanceToEnemy;  // エネミーとの距離
}

/// <summary>
/// 追跡を諦めて徘徊に戻るポイントを管理するクラス
/// </summary>
public class EnemyReturnPointList : MonoBehaviour
{
    // 徘徊に戻るポイントのリスト
    [SerializeField]
    List<ReturnPointData> returnPointDatas = default;

    /// <summary>
    /// プレイヤーとの距離を算出
    /// </summary>
    void CalculateDistanceToEnemy(Transform enemy)
    {
        foreach(ReturnPointData data in returnPointDatas)
        {
            // エネミーとの距離を求める（距離の二乗）
            float distance = (enemy.position - data.position).sqrMagnitude;
            // 求めた距離をセット
            data.distanceToEnemy = distance;
        }
    }

    /// <summary>
    /// 一番近いポイントを取得
    /// </summary>
    /// <returns></returns>
    public ReturnPointData GetNearReturnPoint(Transform enemy)
    {
        // 敵と全てのポイントで距離を算出
        CalculateDistanceToEnemy(enemy);
        // エネミーに一番近いポイントを検索して、そのリスト番号を返す
        int index = returnPointDatas.Select((val, idx) => new { V = val, I = idx }).Aggregate((max, working) => (max.V.distanceToEnemy < working.V.distanceToEnemy) ? max : working).I;
        // 一番近いポイントのデータを返す
        return returnPointDatas[index];
    }
}
