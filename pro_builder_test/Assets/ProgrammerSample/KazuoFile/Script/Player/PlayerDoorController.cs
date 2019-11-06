using System.Collections;
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

    // 仮の部屋番号
    // NOTE:k.oishi 仮のプレイヤーの部屋番号なので、この変数が使われているところをあとで変える必要があります。
    [SerializeField]
    int roomNum = 0;

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
            case OpenType.NORMAL: playerAnim.SetTrigger("Open"); break;
            case OpenType.DASH: playerAnim.SetTrigger("DashOpen"); break;
        }

        // 触れるドアが逆なら逆向きのアニメーション開始
        if (door.IsReverseOpen(roomNum))
        {
            playerAnim.SetBool("Reverse",true);
        }
        else
        {
            playerAnim.SetBool("Reverse", false);
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
    /// NOTE:k.oishi アニメーションイベント用関数
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
