using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 影人間の視野の制御
/// </summary
public class KageFieldOfView : MonoBehaviour
{
    // 視野の距離のパラメータ
    [System.Serializable]
    class DistanceParam
    {
        // ステートの種類
        public KageState.Kind state = default;
        // 視野の距離
        public float distance = 0;
    }

    // ヘッドのトランスフォーム
    [SerializeField]
    Transform head = default;

    // 位置オフセット
    [SerializeField]
    Vector3 positionOffset = Vector3.zero;

    // 回転オフセット
    [SerializeField]
    Quaternion rotationOffset = Quaternion.identity;

    // プレイヤー
    [SerializeField]
    Transform player = default;

    // プレイヤーのコライダー
    [SerializeField]
    Collider playerCollider = default;

    // 視界の角度
    [SerializeField]
    [Range(0, 180)]
    float angle = 0;

    // デバッグ用の距離
    [SerializeField]
    float debugOnlyDistance = 0;
    // ステートごとの視界の距離
    [SerializeField]
    List<DistanceParam> distances = default;
    // 現在の視界の距離
    float currentDistance = 0;

    // レイに衝突する対象のレイヤー名
    [SerializeField]
    List<string> rayTargetLayer = default;

    // 視野にに入っているとき
    class ColliderUnityEvent : UnityEvent<Transform,Collider> { }
    ColliderUnityEvent onInViewRange = new ColliderUnityEvent();

    // 視野から出ているとき
    ColliderUnityEvent onOutViewRange = new ColliderUnityEvent();

    // レイヤーマスク
    int layerMask = 0;

    // 視界の左側の境界
    Vector3 leftBorder = Vector3.zero;
    // 視界の右側の境界
    Vector3 rightBorder = Vector3.zero;
    // 視界の上側の境界
    Vector3 upBorder = Vector3.zero;
    // 視界の下側の境界
    Vector3 downBorder = Vector3.zero;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // レイに衝突する対象のレイヤーを取得
        layerMask = LayerMask.GetMask(rayTargetLayer.ToArray());

        // 最初の警戒範囲を設定する
        ChangeDistance(KageState.Kind.Normal);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 視野のトランスフォームをヘッドと同期する（オフセット分加算）
        transform.position = head.position + positionOffset;
        transform.rotation = head.rotation * rotationOffset;

        // 境界ベクトルを更新
        UpdateBorderVector(angle,currentDistance);

        // デバッグ用の距離を現在の距離に置き換える
        debugOnlyDistance = currentDistance;

        if (IsInViewRange())
        {
            // 範囲内に入ったときのコールバック
            onInViewRange?.Invoke(transform.parent.parent,playerCollider);
        }
        else
        {
            // 範囲からでたときのコールバック
            onOutViewRange?.Invoke(transform.parent.parent, playerCollider);
        }
    }

    /// <summary>
    ///  視野の範囲内のいるときのイベントを追加
    /// </summary>
    /// <param name="set"></param>
    public void SetOnInViewRangeEvent(UnityAction<Transform, Collider> set)
    {
        onInViewRange.AddListener(set);
    }

    /// <summary>
    /// 視界の範囲外にいるときのイベント追加
    /// </summary>
    /// <param name="set"></param>
    public void SetOnOutViewRangeEvent(UnityAction<Transform, Collider> set)
    {
        onOutViewRange.AddListener(set);
    }

    /// <summary>
    /// 視界の距離の変更を行う
    /// </summary>
    /// <param name="state"></param>
    public void ChangeDistance(KageState.Kind state)
    {
        // 引数と同じステート名のパラメータを取得
        DistanceParam param = distances.Find(x => x.state == state);
        // 現在の視界の距離を変更
        currentDistance = param.distance;
    }

    /// <summary>
    /// 視界の境界ベクトルを更新
    /// </summary>
    void UpdateBorderVector(float angle,float distance)
    {
        // 左側の境界ベクトル
        leftBorder = (Quaternion.AngleAxis(angle * 0.5f,transform.right * -1) * transform.forward) * distance;
        // 右側の境界ベクトル
        rightBorder = (Quaternion.AngleAxis(angle * 0.5f,transform.right) * transform.forward) * distance;
        // 上側の境界ベクトル
        upBorder = (Quaternion.AngleAxis(angle * 0.5f, transform.up) * transform.forward) * distance;
        // 下側の境界ベクトル
        downBorder = (Quaternion.AngleAxis(angle * 0.5f,transform.up * -1) * transform.forward) * distance;
    }

    /// <summary>
    /// プレイヤーを発見しているかどうか
    /// </summary>
    /// <returns>発見中かどうかのフラグ</returns>
    bool IsInViewRange()
    {
        // エネミーからプレイヤーに向かってレイを飛ばす
        Ray ray = new Ray(transform.position,(player.transform.position - transform.position).normalized);
        // レイにヒットしたコライダーの情報
        RaycastHit raycastHit;

        // レイにオブジェクトが当たったか
        // 当たっていなければ判定終了
        if (!Physics.Raycast(ray,out raycastHit, Mathf.Infinity, layerMask)) { return false; }

        Debug.DrawLine(transform.position, raycastHit.point);

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
        if (dot < (angle * 0.5f) && raycastHit.distance < currentDistance)
        {
            // プレイヤーを見つけた
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Sceneビューのみに表示
    /// </summary>
    void OnDrawGizmos()
    {
        // 境界ベクトルを更新
        UpdateBorderVector(angle,debugOnlyDistance);
        // デバッグ用に境界ベクトルを表示する
        Debug.DrawRay(transform.position, leftBorder, Color.green);
        Debug.DrawRay(transform.position, rightBorder, Color.green);
        Debug.DrawRay(transform.position, upBorder, Color.green);
        Debug.DrawRay(transform.position, downBorder, Color.green);
    }
#endif
}
