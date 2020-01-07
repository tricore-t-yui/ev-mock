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
    Animator lockerAnim = default;                  // ロッカーのアニメーション
    [SerializeField]
    Transform player = default;                     // プレイヤー
    [SerializeField]
    BoxCollider interactCollider = default;         // インタラクト可能エリア 
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    BoxCollider[] objectCollider = default;         // コライダー
    [SerializeField]
    [Tooltip("壁に接触している面の配列です。 Z軸の正を向いている面がFORWARDとして、反対がBACK、右がRIGHT、左がLEFTとしています。 壁に接触している面を追加してあげるとそっち側に行かないようになります。")]
    List<DirType> wallContactDir = new List<DirType>();// 壁に接触している向きのタイプ

    ObjectType objType = default;                   // 隠れるオブジェクトのタイプ
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

        ChangeObjType();
        ChangeDirType(objType);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ベッドは入る場所によって向きのタイプを変える
        if (objType == ObjectType.BED)
        {
            ChangeDirType(objType);
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
    /// オブジェクトタイプの変更
    /// </summary>
    void ChangeObjType()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Bed"))
        {
            objType = ObjectType.BED;
        }
        else
        {
            objType = ObjectType.LOCKER;
        }
    }

    /// <summary>
    /// 向きタイプの変更
    /// </summary>
    void ChangeDirType(ObjectType type)
    {
        if (type == ObjectType.LOCKER)
        {
            if (transform.localEulerAngles.y < 45 || transform.localEulerAngles.y > 315)
            {
                dirType = DirType.FORWARD;
            }
            else if (transform.localEulerAngles.y > 135 && transform.localEulerAngles.y < 225)
            {
                dirType = DirType.BACK;
            }
            else if (transform.localEulerAngles.y < 180)
            {
                dirType = DirType.RIGHT;
            }
            else
            {
                dirType = DirType.LEFT;
            }
        }
        else
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

            // インタラクト可能エリアの移動
            ChangeColliderPosition();
        }
    }

    /// <summary>
    /// インタラクト可能エリアの移動
    /// </summary>
    void ChangeColliderPosition()
    {
        switch (dirType)
        {
            case DirType.FORWARD:
                interactCollider.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - PositionShift());
                break;
            case DirType.BACK:
                interactCollider.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + PositionShift());
                break;
            case DirType.RIGHT:
                interactCollider.gameObject.transform.position = new Vector3(transform.position.x - PositionShift(), transform.position.y, transform.position.z);
                break;
            case DirType.LEFT:
                interactCollider.gameObject.transform.position = new Vector3(transform.position.x + PositionShift(), transform.position.y, transform.position.z);
                break;
        }
    }

    /// <summary>
    /// ポジションずらし
    /// </summary>
    float PositionShift()
    {
        if (dirType == DirType.FORWARD || dirType == DirType.BACK)
        {
            interactCollider.gameObject.transform.rotation  = Quaternion.Euler(0, 90, 0);
            if ((transform.localEulerAngles.y > 225 && transform.localEulerAngles.y < 315) || (transform.localEulerAngles.y > 45 && transform.localEulerAngles.y < 135))
            {
                return transform.localScale.x / 1.25f;
            }

            return transform.localScale.z / 1.25f;
        }
        else
        {
            interactCollider.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            if ((transform.localEulerAngles.y > 225 && transform.localEulerAngles.y < 315) || (transform.localEulerAngles.y > 45 && transform.localEulerAngles.y < 135))
            {
                return transform.localScale.z / 1.25f;
            }

            return transform.localScale.x / 1.25f;
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
    public bool IsHide()
    {
        return hideController.IsHide();
    }
}
        