using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 鬼の徘徊ルートマネージャー
/// </summary>
public class OniLoiteringRouteManager : MonoBehaviour
{
    // 鬼のオブジェクト
    [SerializeField]
    Transform oni = default;

    // 徘徊ルート
    [SerializeField]
    List<OniLoiteringRoute> routes = default;

    // 初期位置
    Vector3 initPos = Vector3.zero;

    // 一番近いチェックポイントのインデックス
    public int NearCheckPointOfIndex { get; private set; } = 0;
    // 一番近いチェックポイントのルート
    public OniLoiteringRoute NearCheckPointOfRoute { get; private set; }

    /// <summary>
    /// 開始
    /// </summary>
    void OnEnable()
    {
        if (initPos == Vector3.zero)
        {
            // 初期位置を保存
            initPos = transform.position;
        }
        // 初期位置をセット
        transform.position = initPos;
    }

    /// <summary>
    /// 鬼に一番近いルートを更新
    /// </summary>
    public void UpdateNearRoute()
    {
        // それぞれのチェックポイントとの距離
        List<Tuple<float, Vector3, OniLoiteringRoute>> checkPointDistances = new List<Tuple<float, Vector3, OniLoiteringRoute>>();

        routes.ForEach(route =>
        {
            foreach(Vector3 checkPoint in route.CheckPoints)
            {
                // チェックポイントとの距離
                Tuple<float, Vector3, OniLoiteringRoute> checkPointDistance;
                // それぞれの距離を算出
                checkPointDistance = new Tuple<float, Vector3, OniLoiteringRoute>((checkPoint - oni.position).sqrMagnitude, checkPoint, route);
                // 算出した距離を格納
                checkPointDistances.Add(checkPointDistance);
            }
        });

        // 一番距離が近い要素を返す
        Tuple<float, Vector3, OniLoiteringRoute> near = checkPointDistances.OrderBy(s => s.Item1).FirstOrDefault();

        // それぞれをセット
        NearCheckPointOfIndex = near.Item3.CheckPoints.ToList().IndexOf(near.Item2);
        NearCheckPointOfRoute = near.Item3;
    }

#if UNITY_EDITOR
    /// <summary>
    /// ギズモ描画
    /// </summary>
    private void OnDrawGizmos()
    {
        // 現在と次の目標位置を線でつなぐ
        routes.ForEach(route =>
        {
            DrawLineToTargetPos(route.CheckPoints);
        });
    }

    /// <summary>
    /// 現在と次の目標位置を線でつなぐ
    /// </summary>
    void DrawLineToTargetPos(IReadOnlyList<Vector3> checkPoint)
    {
        for (int i = 0; i < checkPoint.Count; i++)
        {
            // リストの末尾にきた場合は、末尾と先頭でつなぐ
            if (i == checkPoint.Count - 1)
            {
                // 線を表示
                Debug.DrawLine(checkPoint[i], checkPoint[0], Color.cyan);
            }
            else
            {
                // 線を表示
                Debug.DrawLine(checkPoint[i], checkPoint[i + 1], Color.cyan);
            }
        }
    }
#endif
}
