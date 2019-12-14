using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 罠ツンのグループ管理クラス
/// </summary>
public class TrapTunGroupController : MonoBehaviour
{
    [SerializeField]
    PlayerStateController stateController = default;    // プレイヤーの状態クラス
    [SerializeField]
    TrapTunController[] tuns = default;                 // 罠ツン

    TrapTunController operateTun = default;             // 作動しているツン
    bool isPlayerHit = false;                           // プレイヤーがエリア内に入ったかどうか
    bool isDemonHit = false;                            // 鬼がエリア内に入ったかどうか
    bool isOperate = false;                             // グループの中の罠ツンが作動したかどうか

    Transform player = default;

    /// <summary>
    /// トリガーが当たったら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            isPlayerHit = true;
            player = other.gameObject.transform;
        }
        if (LayerMask.LayerToName(other.gameObject.layer) == "Demon")
        {
            isDemonHit = true;
        }
    }

    /// <summary>
    /// トリガーから離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            isPlayerHit = false;
        }
        if (LayerMask.LayerToName(other.gameObject.layer) == "Demon")
        {
            isDemonHit = false;
        }
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        operateTun = null;
        isOperate = false;
        isPlayerHit = false;
        isDemonHit = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (isPlayerHit && !isDemonHit && !isOperate)
        {
            foreach (var item in tuns)
            {
                if (item.IsHit)
                {
                    item.transform.LookAt(new Vector3(player.position.x, item.gameObject.transform.position.y, player.position.z));
                    item.TrapOperate();
                    stateController.CheckTrapState(item.gameObject.transform);
                    operateTun = item;
                    isOperate = true;
                }
            }
        }
    }
}
