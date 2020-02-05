using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間の警戒範囲の設定
/// </summary>
public class KageVigilanceRange : MonoBehaviour
{
    // 警戒範囲のパラメータ
    [System.Serializable]
    class RangeParam
    {
        // ステートの種類
        public KageState.Kind state = default;
        // 警戒範囲の半径
        public float radius = 0;
    }

    [SerializeField]
    SphereCollider rangeCollider = default;

    // ステートごとの警戒範囲
    [SerializeField]
    List<RangeParam> rangeParams = default;

    // 現在の警戒範囲
    float currentRadius = 0;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // 最初の警戒範囲を設定する
        ChangeRadius(KageState.Kind.Normal);
    }

    /// <summary>
    /// 警戒範囲半径の変更を行う
    /// </summary>
    /// <param name="stateName"></param>
    public void ChangeRadius(KageState.Kind state)
    {
        // 引数と同じステート名のパラメータを取得
        RangeParam param = rangeParams.Find(x => x.state == state);
        // 現在の視界の距離を変更
        currentRadius = param.radius;
        // コライダーに反映する
        rangeCollider.radius = currentRadius;
    }
}
