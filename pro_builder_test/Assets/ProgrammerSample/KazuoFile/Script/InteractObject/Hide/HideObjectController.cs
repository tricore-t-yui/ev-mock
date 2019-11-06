using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// 隠れるオブジェクトクラス
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
    BoxCollider collider = default;                 // コライダー
    [SerializeField]
    ObjectType objType = default;                   // 隠れるオブジェクトのタイプ
    
    DirType dirType = default;                      // 隠れるオブジェクトの向きのタイプ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 角度によって向きタイプを決める
        switch (transform.eulerAngles.y)
        {
            case 0: dirType = DirType.FORWARD; break;
            case 90: dirType = DirType.RIGHT; break;
            case 180: dirType = DirType.BACK; break;
            case 270: dirType = DirType.LEFT; break;
        }
    }

    /// <summary>
    /// アニメーション開始
    /// </summary>
    public void AnimStart(string id)
    {
        // ロッカーしかアニメーションは付いていないのでロッカーのみ再生
        if (objType == ObjectType.LOCKER)
        {
            lockerAnim.SetTrigger(id);
        }
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
