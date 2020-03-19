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

    bool isOperate = false;                             // グループの中の罠ツンが作動したかどうか

    [SerializeField]
    PlayerMoveController player = default;


    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        isOperate = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (!isOperate)
        {
            TrapTunController hit = null;
            foreach (var item in tuns)
            {
                if (item.IsHit && !item.IsStop)
                {
                    hit = item;
                    break;
                }
            }
            if(hit != null)
            {
                foreach (var item in tuns)
                {
                    if (hit == item)
                    {
                        item.transform.LookAt(new Vector3(player.transform.position.x, item.gameObject.transform.position.y, player.transform.position.z));
                        item.TrapOperate();
                        stateController.CheckTrapState(item.gameObject.transform);
                        isOperate = true;
                    }
                    else
                    {
                        item.Stop();
                    }
                }
            }
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetTrapTun()
    {
        isOperate = false;
        foreach (var item in tuns)
        {
            if(item)
                item.ResetTrapTun();
        }
    }
}
