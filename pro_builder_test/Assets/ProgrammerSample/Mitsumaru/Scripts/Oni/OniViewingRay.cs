using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鬼のレイの処理
/// </summary>
public class OniViewingRay : MonoBehaviour
{
    [SerializeField]
    Transform player = default;

    // レイに衝突する対象のレイヤー名
    [SerializeField]
    List<string> rayTargetLayer = default;

    // レイヤーマスク
    int layerMask = 0;

    // プレイヤーを見つけたかどうか
    public Transform HitObject { get; private set; } = default;

    private void Start()
    {
        // レイに衝突する対象のレイヤーを取得
        layerMask = LayerMask.GetMask(rayTargetLayer.ToArray());
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 鬼からプレイヤーに向かってレイを飛ばす
        Ray ray = new Ray(transform.position, (player.transform.position - transform.position).normalized);

        // プレイヤーに向かってレイを飛ばして、プレイヤーにヒットしたか判定する
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask))
        {
            HitObject = hit.collider.transform;
        }
    }
}
