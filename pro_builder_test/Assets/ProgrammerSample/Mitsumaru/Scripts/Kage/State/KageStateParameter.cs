using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KageStateParameter : MonoBehaviour
{
    // 初期位置
    public Vector3 InitializePos { get; private set; } = Vector3.zero;

    // アニメーター
    [SerializeField]
    Animator animator = default;

    [SerializeField]
    SphereCollider randomRangeCollider = default;

    // 通常状態の種類
    [SerializeField]
    [Tooltip("通常状態の種類を設定します。\n待機型か徘徊型か。")]
    KageStateNormal.StateKind stateNormalOfType = default;
    public KageStateNormal.StateKind StateNormalOfType => stateNormalOfType;

    // 徘徊の種類
    [SerializeField]
    [Tooltip("徘徊型の種類を設定します。\nルートかランダムか。")]
    KageStateLoitering.LoiteringKind stateLoiteringOfType = default;
    public KageStateLoitering.LoiteringKind StateLoiteringOfType => stateLoiteringOfType;

    // 自動でスポーンを行うか
    [SerializeField]
    [Tooltip("自動でスポーンを行うかどうかを設定する。\nこのフラグがオンの場合は、リセット後に自動でスポーンする。")]
    bool isAutoSpawn = true;
    public bool IsAutoSpawn => isAutoSpawn;

    // 状態変化を行わないフラグ
    [SerializeField]
    [Tooltip("状態変化を行うかどうか。\nこのフラグがオンの場合は一切状態が変化しない。")]
    bool isStaticState = false;

    // ランダム徘徊の範囲のサイズ
    [SerializeField]
    [Tooltip("ランダム徘徊の徘徊範囲の半径を設定します。")]
    float randomRangeRadius = 1;
    public float RandomRangeRadius => randomRangeRadius;

    // ルート徘徊するときのチェックポイントのリスト
    [SerializeField]
    [Tooltip("ルート徘徊の各チェックポイントを設定します。\n上から順番に移動していきます。\n最後まで移動すると最初のチェックポイントに戻ります。")]
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

        // 状態変化をフラグをセット
        animator.SetBool("isStaticState", isStaticState);
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
