using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;
using KeyType = KeyController.KeyType;

/// <summary>
/// ロッカークラス
/// </summary>
public class HideObjectController : MonoBehaviour
{
    /// <summary>
    /// 隠れるオブジェクトのタイプ
    /// </summary>
    enum ObjectType
    {
        LOCKER,
        BED,
    }

    [SerializeField]
    Animator lockerAnim = default;                  // ロッカーのアニメーション ベッドの場合は不要
    [SerializeField]
    Transform player = default;                     // プレイヤー

    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス

    [SerializeField]
    ObjectType objType = default;                   // 隠れるオブジェクトのタイプ
    [SerializeField]
    DirType dirType = default;                      // 隠れるオブジェクトの向きのタイプ
    [SerializeField]
    BoxCollider[] objectCollider = default;         // コライダー
    [SerializeField]
    List<DirType> wallContactDir = new List<DirType>();             // 壁に接触している向きのタイプ

    Vector3 rightForward = default;                 // 位置による向きタイプ変更の基準となる右前の角の座標
    Vector3 leftForward = default;                  // 位置による向きタイプ変更の基準となる左前の角の座標

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        rightForward = new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y, transform.position.z - transform.localScale.z / 2);
        leftForward = new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y, transform.position.z - transform.localScale.z / 2);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ベッドは入る場所によって向きのタイプを変える
        if (objType == ObjectType.BED)
        {
            ChandeDirType();
        }
        if (objType == ObjectType.LOCKER)
        {
            if (lockerAnim.GetBool("Close"))
            {
                if (lockerAnim.GetBool("DragOut") || !hideController.IsHideLocker)
                {
                    SetActiveCollider(false);
                }
                else 
                {
                    SetActiveCollider(true);
                }
            }
            if(!lockerAnim.GetBool("DragOut") && !hideController.enabled)
            {
                lockerAnim.SetBool("Close", false);
                SetActiveCollider(true);
            }
        }
    }

    /// <summary>
    /// 位置によって向きタイプの変更
    /// </summary>
    void ChandeDirType()
    {
        if (rightForward.x <= player.position.x && leftForward.x >= player.position.x)
        {
            if (transform.position.z >= player.position.z)
            {
                dirType = DirType.FORWARD;
            }
            else
            {
                dirType = DirType.BACK;
            }
        }
        else if (transform.position.x >= player.position.x)
        {
            dirType = DirType.RIGHT;
        }
        else
        {
            dirType = DirType.LEFT;
        }
    }

    /// <summary>
    /// アニメーション開始
    /// </summary>
    public void AnimStart(string id)
    {
        lockerAnim.SetTrigger(id);
    }

    /// <summary>
    /// コライダーのセットクティブ関数
    /// </summary>
    public void SetActiveCollider(bool flag)
    {
        foreach (var item in objectCollider)
        {
            if (flag)
            {
                item.isTrigger = false;
            }
            else
            {
                item.isTrigger = true;
            }
        }
    }

    /// <summary>
    /// ロッカーの向きタイプのゲット関数
    /// </summary>
    public DirType GetDirType()
    {
        return dirType;
    }

    /// <summary>
    /// ロッカーの向きタイプのゲット関数
    /// </summary>
    public List<DirType> GetWallContactDirType()
    {
        return wallContactDir;
    }

    /// <summary>
    /// 隠れるキーがおされているかどうか
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public bool IsHideKey()
    {
        return hideController.IsHideKey();
    }
}
        