using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationType = PlayerAnimationContoller.AnimationType;

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

    DoorController door = default;                          // ドアの管理クラス
    GameObject doorObj = default;                           // 回転対象のドア
    OpenType openType = OpenType.NORMAL;                    // 開けるタイプ

    [SerializeField]
    AreaManager areaManager = default;                      // エリアマネージャー

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
            case OpenType.NORMAL: animationContoller.AnimStart(AnimationType.OPENDOOR); break;
            case OpenType.DASH: animationContoller.AnimStart(AnimationType.DASHOPENDOOR); break;
        }

        //// NOTE:k.oishi 個人の作業シーンで作業をすると、
        ////      AreaManagerが無いってと怒られるのでとりあえずコメントアウトしています。
        ////
        //// 触れるドアが逆なら逆向きのアニメーション開始
        //string areaName = areaManager.GetExistAreaToCharacter("Player");
        //if (door.IsReverseOpen(areaName))
        //{
        //    animationContoller.AnimStart(AnimationType.REVERSEOPENDOOR);
        //}
        //else
        //{
        //    animationContoller.AnimStop(AnimationType.REVERSEOPENDOOR);
        //}

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
        // アニメーションが再生され終わったら終了処理
        if (animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.DOOR))
        {
            interactController.CommonEndAction();
            enabled = false;
        }
    }
}
