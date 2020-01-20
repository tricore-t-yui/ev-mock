using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class OniWarpAreaWatcher : MonoBehaviour
{
    // エリアのコライダー
    [SerializeField]
    BoxCollider warpTriggerArea = default;

    // ワープエリアのBounds
    [Tooltip("ワープを行うエリアの基準位置とサイズを指定する。")]
    [SerializeField]
    Bounds triggerBounds = default;

    // リスポーン位置
    [Tooltip("プレイヤーがエリアから出たときのリスポーン位置を指定します。")]
    [SerializeField]
    Vector3 respawnPosition = Vector3.zero;

    //鬼のリスト
    List<NavMeshAgent> onis = new List<NavMeshAgent>();

    // 対象の鬼
    NavMeshAgent targetOniAgent = null;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // 鬼のナビメッシュ取得
        foreach(OniLoiteringRouteManager routeManager in FindObjectsOfType<OniLoiteringRouteManager>())
        {
            onis.Add(routeManager.GetComponent<NavMeshAgent>());
        }
    }

    /// <summary>
    /// ギズモ
    /// </summary>
    void OnDrawGizmos()
    {
        // Boundsのcenterとsizeをコライダーにセット
        warpTriggerArea.center = triggerBounds.center;
        warpTriggerArea.size = triggerBounds.size;

        // Boundsを表示
        Gizmos.DrawWireCube(transform.position + triggerBounds.center,triggerBounds.size);

        // リスポーン位置に球体を表
        Gizmos.DrawSphere(transform.position + respawnPosition, 0.5f);
    }

    /// <summary>
    /// コライダーに接触
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが接触したら
        if (other.tag == "Player")
        {
            // アクティブ中の鬼を取得
            NavMeshAgent oni = new NavMeshAgent();
            oni = onis.Find(elem => elem.gameObject.activeSelf);

            // 鬼を消す
            if (oni != default)
            {
                oni.gameObject.SetActive(false);
                targetOniAgent = oni;
            }
        }
    }

    /// <summary>
    /// コライダーから抜けた
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        // プレイヤーだったら
        if (other.tag == "Player")
        {
            // 消した鬼を表示
            targetOniAgent?.gameObject.SetActive(true);
            // 表示した鬼をリスポーン位置までワープさせる
            targetOniAgent?.Warp(transform.position + respawnPosition);
        }
    }
}
