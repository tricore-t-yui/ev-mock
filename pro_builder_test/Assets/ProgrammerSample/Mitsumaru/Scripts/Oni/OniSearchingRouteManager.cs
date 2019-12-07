using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class OniSearchingRouteManager : MonoBehaviour
{
    [SerializeField]
    Transform oni = default;


    [SerializeField]
    List<OniSearchingRoute> routes = default;

    public int NearCheckPointOfIndex { get; private set; } = 0;
    public OniSearchingRoute NearCheckPointOfRoute { get; private set; }

    /// <summary>
    /// 鬼に一番近いルートを更新
    /// </summary>
    public void UpdateNearRoute()
    {
        // それぞれのチェックポイントとの距離
        List<Tuple<float, Vector3, OniSearchingRoute>> checkPointDistances = new List<Tuple<float, Vector3, OniSearchingRoute>>();

        routes.ForEach(route =>
        {
            foreach(Vector3 checkPoint in route.CheckPoints)
            {
                Tuple<float, Vector3, OniSearchingRoute> checkPointDistance;
                checkPointDistance = new Tuple<float, Vector3, OniSearchingRoute>((checkPoint - oni.position).sqrMagnitude, checkPoint, route);
                checkPointDistances.Add(checkPointDistance);
            }
        });

        // 一番距離が近い要素を返す
        Tuple<float, Vector3, OniSearchingRoute> near = checkPointDistances.OrderBy(s => s.Item1).FirstOrDefault();

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
