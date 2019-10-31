﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractController.DirType;

/// <summary>
/// 隠れるアクション管理クラス
/// </summary>
public class PlayerHideController : MonoBehaviour
{
    GameObject targetObj = default;                         // 回転対象のドア

    [SerializeField]
    Animator playerAnim = default;                          // アニメーター
    [SerializeField]    
    InteractController interactController = default;        // インタラクト用関数クラス

    HideObjectController hideObjectController = default;    // 隠れるオブジェクトクラス

    public bool IsWarning { get; private set; } = false;    // 警戒フラグ
    public bool IsHide { get; private set; } = false;       // 隠れているかどうかのフラグ
    public bool IsCanExit { get; private set; } = false;    // ロッカー内からでれるかどうかのフラグ

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
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // オブジェクトに合わせたポジション合わせ
        transform.position = interactController.InitPosition(hideObjectController.GetDirType(), transform, targetObj.transform);
        transform.rotation = interactController.InitRotation(hideObjectController.GetDirType());

        interactController.CommonInit();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetInfo(GameObject hideObj)
    {
        // レイキャストに当たったオブジェクトの情報をもらう
        targetObj = hideObj;
        hideObjectController = targetObj.GetComponent<HideObjectController>();
        hideObjectController.SetActiveCollider(false);

        // オブジェクトに合わせたアニメーションを再生
        switch (LayerMask.LayerToName(hideObj.layer))
        {
            case "Locker":
                playerAnim.SetTrigger("LockerIn");
                hideObjectController.AnimStart("LockerIn"); break;
            case "Bed":
                playerAnim.SetTrigger("BedIn"); break;
        }

        // 隠れる開始
        enabled = true;
        IsCanExit = false;
    }


    /// <summary>
    /// 隠れているかどうかのフラグを切る
    /// </summary>
    public void ExitHideObject()
    {
        IsHide = false;
    }

    /// <summary>
    /// 隠れるオブジェクトから出てれるかどうかのフラグ
    /// </summary>
    public void CanExitHideObject()
    {
        IsCanExit = true;
        IsHide = true;
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndAction()
    {
        interactController.CommonEndAction();
        hideObjectController.SetActiveCollider(true);
        enabled = false;
    }
}
