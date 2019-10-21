using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の視界の処理を行う
/// </summary
public class EnemyVisibility : MonoBehaviour
{
    // プレイヤー
    [SerializeField]
    Transform player = default;

    // 視界の角度
    [SerializeField]
    [Range(0, 180)]
    float angle = 10;

    // 視界の距離
    [SerializeField]
    float distance = 0;

    // 視界の左側の境界
    Vector3 leftBorder = Vector3.zero;
    // 視界の右側の境界
    Vector3 rightBorder = Vector3.zero;

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 境界ベクトルを作成
        CreateBorderVector();
    }

    /// <summary>
    /// 視界の境界ベクトルを作成
    /// </summary>
    void CreateBorderVector()
    {
        // 左側の境界ベクトル
        Vector3 leftVec = Quaternion.Euler(0, -angle * 0.5f, 0) * transform.forward;
        // 視界の距離だけベクトルを伸ばす
        leftBorder = leftVec * distance;

        // 右側の境界ベクトル
        Vector3 rightVec = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        // 視界の距離だけベクトルを伸ばす
        rightBorder = rightVec * distance;
    }

    /// <summary>
    /// プレイヤーを発見しているかどうか
    /// </summary>
    /// <returns>発見中かどうかのフラグ</returns>
    public bool IsPlayerDiscover()
    {
        // プレイヤーとエネミーとのベクトルを算出
        Vector3 playerToEnemy = player.transform.position - transform.position;
        // 算出したベクトルとプレイヤーの向きベクトルの角度を算出
        float dot = Vector3.Angle(transform.forward, playerToEnemy.normalized);

        // [条件１]
        // 算出した角度が左右の境界ベクトルよりも傾いているかどうか
        // memo : 傾いている→視界の範囲外　傾いていない→視界の範囲内
        // [条件２]
        // プレイヤーとエネミーの距離が視界の距離よりも離れているかどうか
        // memo : 離れている→視界の範囲外　離れていない→視界の範囲内
        if (dot < (angle * 0.5f) && playerToEnemy.sqrMagnitude < distance * distance)
        {
            // プレイヤーを見つけた
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sceneビューのみに表示
    /// </summary>
    void OnDrawGizmos()
    {
        // 境界ベクトルを作成
        CreateBorderVector();
        // デバッグ用に境界ベクトルを表示する
        Debug.DrawRay(transform.position, leftBorder, Color.green);
        Debug.DrawRay(transform.position, rightBorder, Color.green);
    }
}
