﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのドア開閉クラス
/// </summary>
public class PlayerDoorController : MonoBehaviour
{
    /// <summary>
    /// 開けるタイプ
    /// </summary>
    public enum OpenType
    {
        NORMAL,
        DASH,
    }

    [SerializeField]
    Animator playerAnim = default;                      // アニメーター
    [SerializeField]
    InteractFunction interactController = default;      // インタラクト用関数クラス

    DoorController door = default;                      // ドアの管理クラス
    GameObject doorObj = default;                       // 回転対象のドア
    OpenType openType = OpenType.NORMAL;                // 開けるタイプ

    [SerializeField]
    AreaManager areaManager = default;                  // エリアマネージャー

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // ドアに合わせたポジション合わせ
        transform.position = interactController.InitPosition(door.GetDirType(),transform, doorObj.transform);
        transform.rotation = interactController.InitRotation(door.GetDirType());

        // 初期化
        interactController.CommonInit();
    }

    /// <summary>
    /// ドア情報の登録
    /// </summary>
    public void SetInfo(GameObject targetObj, OpenType type)
    {
        // レイキャストに当たったドアの情報をもらう
        doorObj = targetObj;
        door = doorObj.GetComponent<DoorController>();
        openType = type;

        // タイプに合わせたアニメーションを再生
        switch (openType)
        {
            case OpenType.NORMAL: playerAnim.SetTrigger("DoorOpen"); break;
            case OpenType.DASH: playerAnim.SetTrigger("DashDoorOpen"); break;
        }

        // NOTE:k.oishi 個人の作業シーンで作業をすると、
        //      AreaManagerが無いってと怒られるのでとりあえずコメントアウトしています。
        //
        // 触れるドアが逆なら逆向きのアニメーション開始
        string areaName = areaManager.GetExistAreaToCharacter("Player");
        if (door.IsReverseOpen(areaName))
        {
            playerAnim.SetBool("ReverseDoorOpen",true);
        }
        else
        {
            playerAnim.SetBool("ReverseDoorOpen", false);
        }

        // ドア開け開始
        enabled = true;
    }

    /// <summary>
    /// ドアの開閉アニメーション開始
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void DoorOpenStart()
    {
        door.RotationStart(openType);
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndDoorAction()
    {
        // 閉じられていたら終了処理
        if (playerAnim.GetBool("DoorEnd"))
        {
            playerAnim.SetBool("DoorEnd", false);
            interactController.CommonEndAction();
            enabled = false;
        }
    }
}
