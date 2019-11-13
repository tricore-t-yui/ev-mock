using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

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
    BoxCollider collider = default;                 // コライダー
    [SerializeField]
    ObjectType objType = default;                   // 隠れるオブジェクトのタイプ

    [SerializeField]
    DirType dirType = default;                      // 隠れるオブジェクトの向きのタイプ

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
        if(flag)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }

    /// <summary>
    /// ロッカーの向きタイプのゲット関数
    /// </summary>
    public DirType GetDirType()
    {
        return dirType;
    }
}
        