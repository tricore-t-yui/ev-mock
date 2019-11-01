using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// 隠れるアクション管理クラス
/// </summary>
public class PlayerHideController : MonoBehaviour
{
    /// <summary>
    /// オブジェクトタイプ
    /// </summary>
    public enum HideObjectType
    {
        LOCKER,
        BED,
    }

    [SerializeField]
    Animator playerAnim = default;                              // アニメーター
    [SerializeField]
    InteractFunction interactController = default;              // インタラクト用関数クラス

    GameObject hideObj = default;                               // 回転対象のドア
    HideObjectController hideObjectController = default;        // 隠れるオブジェクトクラス

    public bool IsWarning { get; private set; } = false;        // 警戒フラグ
    public bool IsHide { get; private set; } = false;           // 隠れているかどうかのフラグ
    public DirType HideObjDir { get; private set; } = default;  // 隠れるオブジェクトの向き

    /// <summary>
    /// トリガー内
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    void OnTriggerStay(Collider other)
    {
        // 敵が近くにいたら警戒状態
        if(LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            IsWarning = true;
        }
    }

    /// <summary>
    /// トリガー外
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    void OnTriggerExit(Collider other)
    {
        // 敵が近くからいなくなったら警戒状態解除
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            IsWarning = false;
        }
    }

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // オブジェクトに合わせたポジション合わせ
        transform.position = interactController.InitPosition(hideObjectController.GetDirType(), transform, hideObj.transform);
        transform.rotation = interactController.InitRotation(hideObjectController.GetDirType());

        // 初期化
        interactController.CommonInit();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetInfo(GameObject targetObj)
    {
        // レイキャストに当たったオブジェクトの情報をもらう
        hideObj = targetObj;
        hideObjectController = hideObj.GetComponent<HideObjectController>();
        HideObjDir = hideObjectController.GetDirType();

        // アニメーションの邪魔になるのでコライダーを切る
        hideObjectController.SetActiveCollider(false);

        // オブジェクトに合わせたアニメーションを再生
        switch (LayerMask.LayerToName(hideObj.layer))
        {
            case "Locker":
                playerAnim.SetTrigger("LockerIn");
                hideObjectController.AnimStart("LockerIn"); break;
            case "Bed":
                playerAnim.SetTrigger("BedIn"); break;
        }

        // 隠れる開始
        enabled = true;
    }

    /// <summary>
    /// 隠れているかどうかのフラグを切る
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void ExitHideObject()
    {
        IsHide = false;
    }

    /// <summary>
    /// 隠れたかどうかのフラグを立てる
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void HideObject()
    {
        IsHide = true;
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void EndAction()
    {
        interactController.CommonEndAction();
        hideObjectController.SetActiveCollider(true);
        enabled = false;
    }
}
