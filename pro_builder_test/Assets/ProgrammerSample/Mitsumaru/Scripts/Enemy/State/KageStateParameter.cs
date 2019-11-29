using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KageStateParameter : MonoBehaviour
{
    // 通常状態の種類
    [SerializeField]
    KageStateNormal.StateKind stateNormalOfType = default;
    public KageStateNormal.StateKind StateNormalOfType => stateNormalOfType;

    // 徘徊の種類
    [SerializeField]
    KageStateLoitering.LoiteringKind stateLoiteringOfType = default;
    public KageStateLoitering.LoiteringKind StateLoiteringOfType => stateLoiteringOfType;

    // ランダム徘徊の範囲用コライダーのオブジェクト名
    [SerializeField]
    string randomMoveRangeOfObjectName = default;
    public string RandomMoveRangeOfObjectName => randomMoveRangeOfObjectName;

    // ルート徘徊するときのチェックポイントのリスト
    [SerializeField]
    List<Vector3> routeCheckPointList = default;
    public IReadOnlyList<Vector3> RouteCheckPointList => routeCheckPointList;

#if UNITY_EDITOR
    /// <summary>
    /// ギズモ描画
    /// </summary>
    private void OnDrawGizmos()
    {
        // 現在と次の目標位置を線でつなぐ
        DrawLineToTargetPos(routeCheckPointList);
        MonoBehaviour monoBehaviour;
    }

    /// <summary>
    /// 現在と次の目標位置を線でつなぐ
    /// </summary>
    void DrawLineToTargetPos(List<Vector3> checkPoint)
    {
        for (int i = 0; i < checkPoint.Count; i++)
        {
            // リストの末尾にきた場合は、末尾と先頭でつなぐ
            if (i == checkPoint.Count - 1)
            {
                // 線を表示
                Debug.DrawLine(checkPoint[i], checkPoint[0], Color.red);
            }
            else
            {
                // 線を表示
                Debug.DrawLine(checkPoint[i], checkPoint[i + 1], Color.red);
            }
        }
    }
#endif
}
