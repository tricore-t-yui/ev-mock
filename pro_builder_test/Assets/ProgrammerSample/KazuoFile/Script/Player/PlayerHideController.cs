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
    CameraController camra = default;                           // カメラクラス

    GameObject hideObj = default;                               // 回転対象のドア
    HideObjectController hideObjectController = default;        // 隠れるオブジェクトクラス

    public bool IsWarning { get; private set; } = false;        // 警戒フラグ
    public bool IsHideLocker { get; private set; } = false;     // ロッカーに隠れているかどうかのフラグ
    public bool IsHideBed { get; private set; } = false;        // ベッドに隠れているかどうかのフラグ
    public DirType HideObjDir { get; private set; } = default;  // 隠れるオブジェクトの向き

    [SerializeField]
    float exitRotationSpeed = 2;                                // 脱出方向へ向くスピード

    Quaternion exitRotation = default;                          // 脱出方向
    bool isExitRotation = false;                                // 脱出時の向き変更フラグ
    bool isExitPosition = false;                                // 脱出時のポジション変更フラグ

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
        isExitPosition = false;
        isExitRotation = false;
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
    /// 更新処理
    /// </summary>
    void Update()
    {
        // ベッドの場合は座標移動
        if (LayerMask.LayerToName(hideObj.layer) == "Bed")
        {
            BedHideMove();
        }

        // 脱出するとき、オブジェクトから出る向きに向いていなかったら回転させる
        if (transform.rotation != exitRotation && isExitRotation)
        {
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, exitRotation, exitRotationSpeed);
            transform.rotation = rotation;
        }
        // 回転し終わったらフラグを切る
        else
        {
            isExitRotation = false;
        }
    }

    /// <summary>
    /// ベッドに隠れるための座標移動
    /// </summary>
    void BedHideMove()
    {
        // まだ隠れていないときは
        if (!IsHideBed)
        {
            // ベッドの下に向かって座標移動
            switch (HideObjDir)
            {
                case DirType.FORWARD: transform.position += Vector3.forward * 0.0125f; break;
                case DirType.BACK: transform.position += Vector3.back * 0.0125f; break;
                case DirType.RIGHT: transform.position += Vector3.right * 0.0125f; break;
                case DirType.LEFT: transform.position += Vector3.left * 0.0125f; break;
            }
        }

        // 脱出フラグがたったら
        if (isExitPosition)
        {
            // ベッドの外に向かって座標移動
            switch (HideObjDir)
            {
                case DirType.FORWARD: transform.position += Vector3.back * 0.03f; break;
                case DirType.BACK: transform.position += Vector3.forward * 0.03f; break;
                case DirType.RIGHT: transform.position += Vector3.left * 0.03f; break;
                case DirType.LEFT: transform.position += Vector3.right * 0.03f; break;
            }
        }
    }

    /// <summary>
    /// 隠れたかどうかのフラグを立てる
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void HideObject()
    {
        // カメラの固定を解除し、オブジェクトに合わせたフラグを立てる
        camra.enabled = true;
        switch (LayerMask.LayerToName(hideObj.layer))
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
        camra.enabled = false;
        switch (LayerMask.LayerToName(hideObj.layer))
        {
            // ロッカー
            case "Locker":
                switch (HideObjDir)
                {
                    case DirType.FORWARD: exitRotation = Quaternion.Euler(0, 180, 0); break;
                    case DirType.BACK: exitRotation = Quaternion.Euler(0, 0, 0); break;
                    case DirType.RIGHT: exitRotation = Quaternion.Euler(0, 90, 0); break;
                    case DirType.LEFT: exitRotation = Quaternion.Euler(0, 270, 0); break;
                }
                IsHideLocker = false;
                break;
            // ベッド
            case "Bed":
                switch (HideObjDir)
                {
                    case DirType.FORWARD: exitRotation = Quaternion.Euler(90, 0, -180); break;
                    case DirType.BACK: exitRotation = Quaternion.Euler(90, 0, 0); break;
                    case DirType.RIGHT: exitRotation = Quaternion.Euler(90, 0, -90); break;
                    case DirType.LEFT: exitRotation = Quaternion.Euler(90, 0, -270); break;
                }
                IsHideBed = false;
                break;
        }

        // 脱出フラグを立てる
        isExitPosition = true;
        isExitRotation = true;
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
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
    /// 警戒状態のフラグセット関数
    /// </summary>
    /// <param name="flag"></param>
    public void CheckWarning(bool flag)
    {
        IsWarning = flag;
    }
}
