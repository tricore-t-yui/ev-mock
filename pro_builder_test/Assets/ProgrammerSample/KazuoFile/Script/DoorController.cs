using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractController.DirType;

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
    DirType type = default;                     // ドアの開ける方向のタイプ
    [SerializeField]
    Animator doorAnim = default;                // ドアのアニメーター

    // ドアを逆に開けることができる部屋番号
    // NOTE:k.oishi 各ドアのこの変数に、この扉を逆側から開くことになる部屋番号を入力してください
    [SerializeField]
    int reverseRoomNum = default;               

    bool isAutoClose = false;                   // 自動でドアを閉めるフラグ
    DirType firstType = default;                // 開始時のドアのタイプ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
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
            doorAnim.SetTrigger("Close");
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
            case PlayerDoorController.OpenType.NORMAL: doorAnim.SetTrigger("Open"); break;
            case PlayerDoorController.OpenType.DASH: doorAnim.SetTrigger("DashOpen"); break;
        }

        isAutoClose = false;
    }

    /// <summary>
    /// 回転スピードの変更
    /// </summary>
    /// <param name="speed">回転スピード</param>
    public void SetAnimSpeed(float speed)
    {
        doorAnim.SetFloat("Speed", speed);
    }

    /// <summary>
    /// ドアが逆側に開くかどうか
    /// </summary>
    /// <param name="num">今の部屋番号</param>
    public bool IsReverseOpen(int num)
    {
        // 今の部屋番号が逆の部屋番号と一致したなら
        if(num == reverseRoomNum)
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
    /// ドアのタイプ
    /// </summary>
    public DirType GetDirType()
    {
        return type;
    }
}
