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
    [SerializeField]
    CameraController camera = default;                           // カメラクラス

    HideObjectController hideObjectController = default;        // 隠れるオブジェクトクラス

    public GameObject HideObj { get; private set; } = default;  // 対象のオブジェクト
    public bool IsWarning { get; private set; } = false;        // 警戒フラグ
    public bool IsHideLocker { get; private set; } = false;     // ロッカーに隠れているかどうかのフラグ
    public bool IsHideBed { get; private set; } = false;        // ベッドに隠れているかどうかのフラグ
    public DirType HideObjDir { get; private set; } = default;  // 隠れるオブジェクトの向き

    public bool IsAnimRotation { get; private set; } = true;   // 回転をアニメーションに任せるフラグ

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // オブジェクトに合わせたポジション合わせ
        transform.position = interactController.InitPosition(hideObjectController.GetDirType(), transform, HideObj.transform);
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
        HideObj = targetObj;
        hideObjectController = HideObj.GetComponent<HideObjectController>();
        HideObjDir = hideObjectController.GetDirType();

        // アニメーションの邪魔になるのでコライダーを切る
        hideObjectController.SetActiveCollider(false);

        // オブジェクトに合わせたアニメーションを再生
        switch (LayerMask.LayerToName(HideObj.layer))
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
    /// 隠れたかどうかのフラグを立てる
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void HideObject()
    {
        // カメラの固定を解除し、オブジェクトに合わせたフラグを立てる
        camera.IsRotationCamera(true);
        switch (LayerMask.LayerToName(HideObj.layer))
        {
            // ロッカー
            case "Locker":
                IsHideLocker = true; break;
            // ベッド
            case "Bed":
                IsHideBed = true; break;
        }
    }

    /// <summary>
    /// 隠れているかどうかのフラグを切る
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void ExitHideObject()
    {
        // カメラの固定し、オブジェクトに合わせたフラグを切り、オブジェクトから出る向きを求める
        camera.IsRotationCamera(false);
        switch (LayerMask.LayerToName(HideObj.layer))
        {
            // ロッカー
            case "Locker":
                IsHideLocker = false; break;
            // ベッド
            case "Bed":
                IsHideBed = false; break;
        }
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndHideAction()
    {
        // 閉じられていたら終了処理
        if (playerAnim.GetBool("HideEnd"))
        {
            playerAnim.SetBool("HideEnd", false);
            interactController.CommonEndAction();
            hideObjectController.SetActiveCollider(true);
            enabled = false;
        }
    }

    /// <summary>
    /// 回転フラグのセット関数
    /// </summary>
    /// NOTE: k.oishi
    ///      ステートマシンの方で脱出方向へ回転させているのですが、回転している間は回転をアニメーションに任せる処理を切らないといけません。
    ///      なので、ステートマシンの方で回転が終わったらフラグを切れるように、フラグセット関数を用意しました。
    public void SetIsAnimRotation(bool flag)
    {
        IsAnimRotation = flag;
    }

    /// <summary>
    /// 警戒状態のフラグセット関数
    /// </summary>
    public void CheckWarning(bool flag)
    {
        IsWarning = flag;
    }
}
