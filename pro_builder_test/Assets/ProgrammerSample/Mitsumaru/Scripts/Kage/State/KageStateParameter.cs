using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KageStateParameter : MonoBehaviour
{
    // 初期位置
    public Vector3 InitializePos { get; private set; } = Vector3.zero;

    // 通常状態の種類
    [SerializeField]
    KageStateNormal.StateKind stateNormalOfType = default;
    public KageStateNormal.StateKind StateNormalOfType => stateNormalOfType;

    // 徘徊の種類
    [SerializeField]
    KageStateLoitering.LoiteringKind stateLoiteringOfType = default;
    public KageStateLoitering.LoiteringKind StateLoiteringOfType => stateLoiteringOfType;

    // ランダム徘徊の範囲のサイズ
    [SerializeField]
    float randomRangeRadius = 1;
    public float RandomRangeRadius => randomRangeRadius;

    [SerializeField]
    SphereCollider randomRangeCollider = default;

    // ルート徘徊するときのチェックポイントのリスト
    [SerializeField]
    List<Vector3> routeCheckPointList = default;
    public IReadOnlyList<Vector3> RouteCheckPointList
    {
        get
        {
            return routeCheckPointList;
        }
    }

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // 初期位置をセット
        InitializePos = transform.position;

        // ランダム移動の範囲を設定
        randomRangeCollider.radius = randomRangeRadius;
    }

#if UNITY_EDITOR
    /// <summary>
    /// ギズモ描画
    /// </summary>
    private void OnDrawGizmos()
    {
        // 現在と次の目標位置を線でつなぐ
        DrawLineToTargetPos(routeCheckPointList);
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
