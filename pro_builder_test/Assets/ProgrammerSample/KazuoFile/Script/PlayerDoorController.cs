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
        WALK,
        DASH,
    }

    [SerializeField]
    Rigidbody rigid = default;              // プレイヤーの座標移動クラス
    [SerializeField]
    CameraController camera = default;      // カメラクラス
    [SerializeField]
    Animator playerAnim = default;          // アニメーター

    DoorController door = default;          // ドアの管理クラス
    GameObject targetObj = default;         // 回転対象のドア

    // 仮の部屋番号
    // NOTE:k.oishi 仮のプレイヤーの部屋番号なので、この変数が使われているところをあとで変える必要があります。
    [SerializeField]
    int roomNum = 0; 

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // アニメーション設定
        playerAnim.enabled = true;

        // 重力を切る
        rigid.useGravity = false;

        // カメラをアニメーションに固定させる
        camera.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        camera.enabled = false;
    }

    /// <summary>
    /// ドア情報の登録
    /// </summary>
    public void SetInfo(GameObject doorObj, bool isDash)
    {
        // レイキャストに当たったドアの情報をもらう
        targetObj = doorObj;
        door = targetObj.GetComponent<DoorController>();

        // ダッシュなら自動ドア
        if (isDash)
        {
            playerAnim.SetTrigger("DashOpen");
        }
        // ダッシュじゃなかったら普通の開閉
        else
        {
            playerAnim.SetTrigger("Open");
        }

        // 触れるドアが逆なら逆向きのアニメーション開始
        if (door.IsReverseOpen(roomNum))
        {
            playerAnim.SetBool("Reverse",true);
        }

        // ドアに合わせたポジション合わせ
        InitPosition(door.GetDoorType());

        // 登録したらドア開閉開始
        enabled = true;
    }

    /// <summary>
    /// ドアのタイプに合わせたポジション合わせ
    /// </summary>
    /// <param name="type">ドアのタイプ</param>
    void InitPosition(DoorController.DirType type)
    {
        switch (type)
        {
            case DoorController.DirType.FORWARD:
                transform.position = new Vector3(targetObj.transform.position.x, transform.position.y, targetObj.transform.position.z - 0.84f);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DoorController.DirType.BACK:
                transform.position = new Vector3(targetObj.transform.position.x, transform.position.y, targetObj.transform.position.z + 0.84f);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case DoorController.DirType.RIGHT:
                transform.position = new Vector3(targetObj.transform.position.x - 0.84f, transform.position.y, targetObj.transform.position.z);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case DoorController.DirType.LEFT:
                transform.position = new Vector3(targetObj.transform.position.x + 0.84f, transform.position.y, targetObj.transform.position.z);
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }
    }

    /// <summary>
    /// ドアの開閉アニメーション開始
    /// </summary>
    public void DoorOpenStart(OpenType type)
    {
        door.RotationStart(type);
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndAction()
    {
        camera.enabled = true;
        rigid.useGravity = true;
        playerAnim.SetBool("Reverse", false);
        playerAnim.enabled = false;
        enabled = false;
    }
}
