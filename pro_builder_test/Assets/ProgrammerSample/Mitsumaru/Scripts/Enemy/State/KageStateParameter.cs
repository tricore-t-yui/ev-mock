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
}
