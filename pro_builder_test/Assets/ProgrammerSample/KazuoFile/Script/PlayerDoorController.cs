using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractController.DirType;

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
    InteractController interactController = default;    // インタラクト用関数クラス

    DoorController door = default;                      // ドアの管理クラス
    GameObject targetObj = default;                     // 回転対象のドア
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
        transform.position = interactController.InitPosition(door.GetDirType(),transform, targetObj.transform);
        transform.rotation = interactController.InitRotation(door.GetDirType());

        interactController.CommonInit();
    }

    /// <summary>
    /// ドア情報の登録
    /// </summary>
    public void SetInfo(GameObject doorObj, OpenType type)
    {
        // レイキャストに当たったドアの情報をもらう
        targetObj = doorObj;
        door = targetObj.GetComponent<DoorController>();

        openType = type;

        // タイプに合わせてトリガーオン
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

        enabled = true;
    }



    /// <summary>
    /// ドアの開閉アニメーション開始
    /// </summary>
    public void DoorOpenStart()
    {
        door.RotationStart(openType);
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndAction()
    {
        interactController.CommonEndAction();
        enabled = false;
    }
}
