﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationType = PlayerAnimationContoller.AnimationType;
using KeyType = KeyController.KeyType;
using StickType = KeyController.StickType;

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
    PlayerAnimationContoller animationContoller = default;  // アニメーション管理クラス
    [SerializeField]
    InteractFunction interactController = default;          // インタラクト用関数クラス
    [SerializeField]
    KeyController keyController = default;                  // キー操作クラス
    [SerializeField]
    PlayerStateController stateController = default;        // 状態管理クラス

    DoorController door = default;                          // ドアの管理クラス
    GameObject doorObj = default;                           // 回転対象のドア
    OpenType openType = OpenType.NORMAL;                    // 開けるタイプ
    bool isReverse = false;                                 // ドアを反対側から開けるかどうか


    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // ドアに合わせたポジション合わせ
        transform.position = interactController.InitPosition(door.GetDirType(),transform, doorObj.transform);
        transform.rotation = interactController.InitRotation(door.GetDirType());
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
            case OpenType.NORMAL: animationContoller.AnimStart(AnimationType.OPENDOOR); break;
            case OpenType.DASH: animationContoller.AnimStart(AnimationType.DASHOPENDOOR); break;
        }

        // 触れるドアが逆なら逆向きのアニメーション開始
        if (door.IsReverseOpen(stateController.NowAreaName))
        {
            isReverse = true;
            animationContoller.AnimStart(AnimationType.REVERSEOPENDOOR);
        }
        else
        {
            isReverse = false;
            animationContoller.AnimStop(AnimationType.REVERSEOPENDOOR);
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
    /// 各アクションの終了処理
    /// </summary>
    /// <param name="isIgnore">条件無視フラグ</param>
    public void EndDoorAction(bool isIgnore)
    {
        bool flag = false; // 終了処理をさせるかどうか

        // アニメーションが再生され終わるか、条件無視フラグが立っていたら終了処理開始
        if (animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.DOOR) && !isIgnore)
        {
            flag = true;
        }
        else if(isIgnore)
        {
            flag = true;
        }

        if (flag)
        {
            animationContoller.SetEndAnimationFlag(PlayerAnimationContoller.EndAnimationType.DOOR);
            enabled = false;
        }
    }

    /// <summary>
    /// ドアキーがおされているかどうか
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public bool IsDoorKey()
    {
        return keyController.GetKey(KeyType.INTERACT);
    }

    /// <summary>
    /// ドア開閉時のスティックの入力加減取得
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public Vector2 GetOpenStick()
    {
        return keyController.GetStick(StickType.LEFTSTICK);
    }

    /// <summary>
    /// エリアの変更関数
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public void ChangeAreaName()
    {
        stateController.ChangeAreaName(door.GetRoomName(isReverse));
    }
}
