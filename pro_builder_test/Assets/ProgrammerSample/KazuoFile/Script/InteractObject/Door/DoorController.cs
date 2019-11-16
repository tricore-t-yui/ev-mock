using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// ドア管理クラス
/// </summary>
public class DoorController : MonoBehaviour
{
    [SerializeField]
    Transform player = default;                 // プレイヤー
    [SerializeField]
    CapsuleCollider playerCollider = default;   // プレイヤー
    [SerializeField]
    Animator doorAnim = default;                // ドアのアニメーター

    bool isAutoClose = false;                   // 自動でドアを閉めるフラグ

    DirType type = default;                     // 現在のドアの開ける方向のタイプ
    DirType firstType = default;                // 開始時のドアの開ける方向のタイプ

    // ドアを逆に開けることができる部屋番号
    // NOTE:k.oishi 各ドアのこの変数に、この扉を逆側から開くことになる部屋番号を入力してください
    [SerializeField]
    string reverseRoomName = null;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // ドアの角度によって開けるタイプを決める
        switch(transform.eulerAngles.y)
        {
            case 0: type = DirType.FORWARD; break;
            case 90: type = DirType.RIGHT; break;
            case 180: type = DirType.BACK; break;
            case 270: type = DirType.LEFT; break;
        }

        // 開始時のドアのタイプを取っておき、閉められている状態にする
        firstType = type;
        isAutoClose = true;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 扉を閉めるラインを超えたら自動で閉める
        if (isCloseLine() && !isAutoClose)
        {
            doorAnim.SetTrigger("DoorClose");
            isAutoClose = true;
        }
    }

    /// <summary>
    /// 扉を閉めるラインを超えたら
    /// </summary>
    bool isCloseLine()
    {
        switch (type)
        {
            case DirType.FORWARD:
                if(player.position.z >= transform.position.z + playerCollider.radius)
                {
                    return true;
                }
                break;
            case DirType.BACK:
                if (player.position.z <= transform.position.z - playerCollider.radius)
                {
                    return true;
                }
                break; 
            case DirType.RIGHT:
                if (player.position.x >= transform.position.x + playerCollider.radius)
                {
                    return true;
                }
                break; 
            case DirType.LEFT:
                if (player.position.x <= transform.position.x - playerCollider.radius)
                {
                    return true;
                }
                break; 
        }

        return false;
    }

    /// <summary>
    /// タイプ別回転開始
    /// </summary>
    public void RotationStart(PlayerDoorController.OpenType type)
    {
        // タイプに合わせてトリガーオン
        switch (type)
        {
            case PlayerDoorController.OpenType.NORMAL: doorAnim.SetTrigger("DoorOpen"); break;
            case PlayerDoorController.OpenType.DASH: doorAnim.SetTrigger("DashDoorOpen"); break;
        }

        isAutoClose = false;
    }

    /// <summary>
    /// 回転スピードの変更
    /// </summary>
    /// <param name="speed">回転スピード</param>
    public void SetAnimSpeed(float speed)
    {
        doorAnim.SetFloat("DoorOpenSpeed", speed);
    }

    /// <summary>
    /// ドアが逆側に開くかどうか
    /// </summary>
    public bool IsReverseOpen(string name)
    {
        // 今の部屋番号が逆の部屋番号と一致したなら
        if(name == reverseRoomName)
        {
            // ドアのタイプを変えておく
            ChangeOpenType(true);

            return true;
        }
        // 一致しなかったら
        else
        {
            // ドアのタイプを戻しておく
            ChangeOpenType(false);

            return false;
        }
    }

    /// <summary>
    /// ドアのタイプの切り替え
    /// </summary>
    /// <param name="isReverse">タイプが逆になるかどうか</param>
    void ChangeOpenType(bool isReverse)
    {
        // フラグが立っているなら
        if (isReverse)
        {
            // 扉のタイプを逆に変える
            switch (firstType)
            {
                case DirType.FORWARD: type = DirType.BACK; break;
                case DirType.BACK: type = DirType.FORWARD; break;
                case DirType.RIGHT: type = DirType.LEFT; break;
                case DirType.LEFT: type = DirType.RIGHT; break;
            }
        }
        else
        {
            //扉のタイプを元に戻す
            type = firstType;
        }
    }

    /// <summary>
    /// ドアの向きタイプのゲット関数
    /// </summary>
    public DirType GetDirType()
    {
        return type;
    }
}
