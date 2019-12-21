﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鬼のレイの処理
/// </summary>
public class OniViewingRay : MonoBehaviour
{
    [SerializeField]
    Transform player = default;

    // レイヤーマスク
   // int layerMask = 0;

    // プレイヤーを見つけたかどうか
    public Transform HitObject { get; private set; } = default;

    [SerializeField]
    LayerMask layerMask = default;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        // レイに衝突する対象のレイヤーを取得
        //layerMask = 1 << LayerMask.GetMask(new string[] { "Player", "Stage", "Locker","Bed" });
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
        if (Physics.Raycast(ray,out hit,Mathf.Infinity, layerMask.value))
        {
            // レイに当たっているオブジェクトをセット
            HitObject = hit.collider.transform;
        }
    }
}
