using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れるアクション管理クラス
/// </summary>
public class PlayerHideController : MonoBehaviour
{
    GameObject targetObj = default;                         // 回転対象のドア

    public bool IsWarning { get; private set; } = false;    // 警戒フラグ

    /// <summary>
    /// トリガー内
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    void OnTriggerStay(Collider other)
    {
        // 敵が近くにいたら警戒状態
        if(LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            IsWarning = true;
        }
    }

    /// <summary>
    /// トリガー外
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    void OnTriggerExit(Collider other)
    {
        // 敵が近くからいなくなったら警戒状態解除
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            IsWarning = false;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(GameObject doorObj)
    {
        enabled = true;

        // レイキャストに当たったドアの情報をもらう
        targetObj = doorObj;
    }
}
