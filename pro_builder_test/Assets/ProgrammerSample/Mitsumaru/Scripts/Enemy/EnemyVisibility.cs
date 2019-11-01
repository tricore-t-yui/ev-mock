using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void OnDiscoverMomentDelegate();
public delegate void OnLoseMomentDelegate();

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

    // プレイヤーを見つけた瞬間
    OnDiscoverMomentDelegate onDiscoverMoment;

    // プレイヤーを見失った瞬間
    OnLoseMomentDelegate onLoseMoment;

    // 視界の左側の境界
    Vector3 leftBorder = Vector3.zero;
    // 視界の右側の境界
    Vector3 rightBorder = Vector3.zero;
    // 視界の上側の境界
    Vector3 upBorder = Vector3.zero;
    // 視界の下側の境界
    Vector3 downBorder = Vector3.zero;

    // プレイヤーを発見したか
    bool isDiscover = false;
    // プレイヤーを発見したか（前フレーム）
    bool isPrevDiscover = false;

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 境界ベクトルを更新
        UpdateBorderVector();

        // 発見中かどうかのフラグを現在と前フレームで入れ替える
        isPrevDiscover = isDiscover;
        isDiscover = IsPlayerDiscover();

        if (IsPlayerDiscoverMoment())
        {
            onDiscoverMoment();
        }
        else if (IsPlayerLoseMoment())
        {
            onLoseMoment();
        }
    }

    /// <summary>
    /// 発見したときのデリゲートをセット
    /// </summary>
    /// <param name="set"></param>
    public void SetOnDiscoverMomentDelegate(OnDiscoverMomentDelegate set)
    {
        onDiscoverMoment = set;
    }

    /// <summary>
    /// 見失ったときデリゲートをセットする
    /// </summary>
    /// <param name="set"></param>
    public void SetOnLoseMomentDelegate(OnLoseMomentDelegate set)
    {
        onLoseMoment = set;
    }

    /// <summary>
    /// 視界の境界ベクトルを更新
    /// </summary>
    void UpdateBorderVector()
    {
        // 左側の境界ベクトル
        Vector3 leftVec = Quaternion.Euler(0, -angle * 0.5f, 0) * transform.forward;
        // 視界の距離だけベクトルを伸ばす
        leftBorder = leftVec * distance;

        // 右側の境界ベクトル
        Vector3 rightVec = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
        // 視界の距離だけベクトルを伸ばす
        rightBorder = rightVec * distance;

        // 上側の境界ベクトル
        Vector3 upVec = Quaternion.Euler(-angle * 0.5f, 0, 0) * transform.forward;
        // 視界の距離だけベクトルを伸ばす
        upBorder = upVec * distance;

        // 下側の境界ベクトル
        Vector3 downVec = Quaternion.Euler(angle * 0.5f, 0, 0) * transform.forward;
        // 視界の距離だけベクトルを伸ばす
        downBorder = downVec * distance;
    }

    /// <summary>
    /// プレイヤーを発見しているかどうか
    /// </summary>
    /// <returns>発見中かどうかのフラグ</returns>
    bool IsPlayerDiscover()
    {
        // エネミーからプレイヤーに向かってレイを飛ばす
        Ray ray = new Ray(transform.position,(player.transform.position - transform.position).normalized);
        // レイにヒットしたコライダーの情報
        RaycastHit raycastHit;

        // レイにオブジェクトが当たったか
        // 当たっていなければ判定終了
        if (!Physics.Raycast(ray,out raycastHit)) { return false; }
        // 当たったオブジェクトが障害物かどうか
        // 障害物だった場合は判定終了
        if (raycastHit.collider.name != player.name) { return false; }

        // レイの向きベクトルとプレイヤーの向きベクトルの角度を算出
        float dot = Vector3.Angle(transform.forward, ray.direction);

        // [条件１]
        // 算出した角度が左右の境界ベクトルよりも傾いているかどうか
        // memo : 傾いている→視界の範囲外　傾いていない→視界の範囲内
        // [条件２]
        // プレイヤーとエネミーの距離が視界の距離よりも離れているかどうか
        // memo : 離れている→視界の範囲外　離れていない→視界の範囲内
        if (dot < (angle * 0.5f) && raycastHit.distance < distance)
        {
            // プレイヤーを見つけた
            return true;
        }
        return false;
    }

    /// <summary>
    /// プレイヤーを発見した瞬間
    /// </summary>
    /// <returns></returns>
    bool IsPlayerDiscoverMoment()
    {
        if (!isPrevDiscover)
        {
            if (isDiscover)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// プレイヤーを見失った
    /// </summary>
    bool IsPlayerLoseMoment()
    {
        if (isPrevDiscover)
        {
            if (!isDiscover)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Sceneビューのみに表示
    /// </summary>
    void OnDrawGizmos()
    {
        // 境界ベクトルを更新
        UpdateBorderVector();
        // デバッグ用に境界ベクトルを表示する
        Debug.DrawRay(transform.position, leftBorder, Color.green);
        Debug.DrawRay(transform.position, rightBorder, Color.green);
        Debug.DrawRay(transform.position, upBorder, Color.green);
        Debug.DrawRay(transform.position, downBorder, Color.green);
    }
}
