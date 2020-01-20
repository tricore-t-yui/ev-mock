using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[System.Serializable]
public class WarpAreaParameter
{
    public enum ExitSurface
    {
        XPlus,
        XMinus,
        ZPlus,
        ZMinus,
        Num,
    }

    // 出口の面
    public ExitSurface exitSurface = ExitSurface.XPlus;

    // リスポーン位置
    public Vector3 respawnPosition = Vector3.zero;
}

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

    // パラメーター
    [SerializeField]
    List<WarpAreaParameter> warpAreaParameter = new List<WarpAreaParameter>();

    //鬼のリスト
    List<NavMeshAgent> onis = new List<NavMeshAgent>();

    // 対象の鬼
    NavMeshAgent targetOniAgent = null;

    // 面の向き
    List<System.Tuple<Vector3, WarpAreaParameter.ExitSurface>> surfaceDirs = new List<System.Tuple<Vector3, WarpAreaParameter.ExitSurface>>();

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        surfaceDirs.Add(new System.Tuple<Vector3, WarpAreaParameter.ExitSurface>(new Vector3(+1, 0, 0), WarpAreaParameter.ExitSurface.XPlus));
        surfaceDirs.Add(new System.Tuple<Vector3, WarpAreaParameter.ExitSurface>(new Vector3(-1, 0, 0), WarpAreaParameter.ExitSurface.XMinus));
        surfaceDirs.Add(new System.Tuple<Vector3, WarpAreaParameter.ExitSurface>(new Vector3(0, 0, +1), WarpAreaParameter.ExitSurface.ZMinus));
        surfaceDirs.Add(new System.Tuple<Vector3, WarpAreaParameter.ExitSurface>(new Vector3(0, 0, -1), WarpAreaParameter.ExitSurface.ZPlus));

        // 鬼のナビメッシュ取得
        foreach (OniLoiteringRouteManager routeManager in FindObjectsOfType<OniLoiteringRouteManager>())
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
        Gizmos.DrawWireCube(transform.position + triggerBounds.center, triggerBounds.size);

        // リスポーン位置に球体を表
        warpAreaParameter.ForEach(elem => { Gizmos.DrawSphere(transform.position + elem.respawnPosition, 0.3f); });
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
            var surfaceDir = surfaceDirs.OrderBy(elem => Vector3.Angle(elem.Item1,(transform.position - other.transform.position).normalized));

            var param = warpAreaParameter.Find(elem => elem.exitSurface == surfaceDir.First().Item2);
            if (param != default)
            {
                // 消した鬼を表示
                targetOniAgent?.gameObject.SetActive(true);
                // 表示した鬼をリスポーン位置までワープさせる
                targetOniAgent?.Warp(transform.position + param.respawnPosition);
            }


        }
    }
}
