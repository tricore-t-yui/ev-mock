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

    public int RayIrradiationTimeToPlayer { get; private set; } = 0;
    public int RayBlockingTimeToPlayer { get; private set; } = 0;

    /// <summary>
    /// 開始
    /// </summary>
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
        Vector3 origin = transform.position + new Vector3(0, 1, 0);
        Vector3 dir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        // 鬼からプレイヤーに向かってレイを飛ばす
        Ray ray = new Ray(origin,dir);
        Debug.DrawRay(origin, dir, Color.yellow);

        // プレイヤーに向かってレイを飛ばして、プレイヤーにヒットしたか判定する
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask))
        {
            // レイに当たっているオブジェクトをセット
            HitObject = hit.collider.transform;
            
            if (hit.collider.tag == "Player")
            {
                RayIrradiationTimeToPlayer++;
                RayBlockingTimeToPlayer = 0;
            }
            else
            {
                RayIrradiationTimeToPlayer = 0;
                RayBlockingTimeToPlayer++;
            }
        }
        else
        {
            RayIrradiationTimeToPlayer = 0;
            RayBlockingTimeToPlayer++;
        }
    }
}
