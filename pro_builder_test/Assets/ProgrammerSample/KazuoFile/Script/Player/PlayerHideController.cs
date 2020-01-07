using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;
using AnimationType = PlayerAnimationContoller.AnimationType;
using KeyType = KeyController.KeyType;

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
    PlayerAnimationContoller animationContoller = default;      // アニメーション管理クラス
    [SerializeField]
    InteractFunction interactController = default;              // インタラクト用関数クラス
    [SerializeField]
    PlayerMoveController moveController = default;              // 移動クラス
    [SerializeField]
    CameraController moveCamera = default;                      // 移動カメラクラス
    [SerializeField]
    HideStateController hideStateController = default;          // 隠れた時の状態変化クラス
    [SerializeField]
    PlayerBreathController breathController = default;          // 息クラス
    [SerializeField]
    KeyController keyController = default;                      // キー操作クラス

    HideObjectController hideObjectController = default;        // 隠れるオブジェクトクラス

    public bool IsHideLocker { get; private set; } = false;     // ロッカーに隠れているかどうかのフラグ
    public bool IsHideBed { get; private set; } = false;        // ベッドに隠れているかどうかのフラグ
    public bool IsInteractArea{ get; private set; } = false;    // インタラクト可能エリアに触れているかどうかのフラグ
    public bool IsAnimRotation { get; private set; } = true;    // 回転をアニメーションに任せるフラグ
    public HideObjectType type { get; private set; } = default; // 隠れているオブジェクトのタイプ
    public GameObject HideObj { get; private set; } = default;  // 対象のオブジェクト
    public DirType HideObjDir { get; private set; } = default;  // 隠れるオブジェクトの向き
    List<DirType> hideObjWallContactDir = new List<DirType>();  // 隠れるオブジェクトに接触している壁の向き

    bool isStealth = false;                                     // 息止めフラグ
    int keyInputStage = 0;                                      // キーの入力段階

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // オブジェクトに合わせたポジション合わせ
        transform.position = interactController.InitPosition(hideObjectController.GetDirType(), transform, HideObj.transform);
        transform.rotation = interactController.InitRotation(hideObjectController.GetDirType());
        keyInputStage = 1;
    }

    /// <summary>
    /// トリガーに触れたら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interact"))
        {
            IsInteractArea = true;
        }
    }
    /// <summary>
    /// トリガーから離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interact"))
        {
            IsInteractArea = false;
        }
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
        hideObjWallContactDir = hideObjectController.GetWallContactDirType();

        // アニメーションの邪魔になるのでコライダーを切る
        hideObjectController.SetActiveCollider(false);

        // オブジェクトに合わせたアニメーションを再生
        switch (LayerMask.LayerToName(HideObj.layer))
        {
            case "Locker":
                animationContoller.AnimStart(AnimationType.HIDELOCKER);
                hideObjectController.AnimStart("LockerIn");
                type = HideObjectType.LOCKER; break;
            case "Bed":
                animationContoller.AnimStart(AnimationType.HIDEBED);
                type = HideObjectType.BED; break;
        }

        SetIsAnimRotation(true);

        // 隠れる開始
        hideStateController.enabled = true;
        enabled = true;
    }

    /// <summary>
    /// 隠れている時のカメラ移動
    /// </summary>
    public void HideCameraMove()
    {
        if (IsHideBed)
        {
            moveCamera.Rotation(CameraController.RotationType.HIDEBED);
        }
        if (IsHideLocker)
        {
            moveCamera.Rotation(CameraController.RotationType.HIDELOCKER);
        }
    }

    /// <summary>
    /// 隠れたかどうかのフラグを立てる
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void HideObject()
    {
        // カメラの固定を解除し、オブジェクトに合わせたフラグを立てる
        IsAnimRotation = false;
        switch (type)
        {
            // ロッカー
            case HideObjectType.LOCKER:
                moveCamera.Rotation(CameraController.RotationType.HIDELOCKER);
                IsHideLocker = true; break;
            // ベッド
            case HideObjectType.BED:
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
        switch (type)
        {
            // ロッカー
            case HideObjectType.LOCKER:
                IsHideLocker = false; break;
            // ベッド
            case HideObjectType.BED:
                IsHideBed = false; break;
        }
    }

    /// <summary>
    /// アニメーションんに座標移動、回転を任せるかどうか
    /// </summary>
    public void ChangeRootMotion()
    {
        switch (LayerMask.LayerToName(HideObj.layer))
        {
            case "Locker":
                if (IsAnimRotation)
                {
                    moveController.IsRootMotion(true, true);
                }
                else
                {
                    if (IsHideLocker)
                    {
                        moveController.IsRootMotion(false, false);
                    }
                    else
                    {
                        moveController.IsRootMotion(true, false);
                    }
                }
                break;
            case "Bed":
                if (IsAnimRotation)
                {
                    moveController.IsRootMotion(false, true);
                }
                else
                {
                    moveController.IsRootMotion(false, false);
                }
                break;
        }
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndHideAction(bool isIgnore)
    {
        bool flag = false; // 終了処理をさせるかどうか

        // アニメーションが再生され終わるか、条件無視フラグが立っていたら終了処理開始
        if (animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.HIDE) && !isIgnore)
        {
            flag = true;
        }
        else if (isIgnore)
        {
            flag = true;
        }

        // アニメーションが再生され終わったら終了処理
        if (flag && enabled)
        {
            keyInputStage = 0;
            animationContoller.SetEndAnimationFlag(PlayerAnimationContoller.EndAnimationType.HIDE);
            hideObjectController.SetActiveCollider(true);
            IsHideLocker = false;
            IsHideBed = false;
            hideStateController.enabled = false;
            enabled = false;
        }
    }

    /// <summary>
    /// 停止処理
    /// </summary>
    void OnDisable()
    {
        hideStateController.enabled = false;
    }

    /// <summary>
    /// 隠れている状態で息を止めいてるかどうか
    /// </summary>
    public bool IsHideStealth()
    {
        if(isStealth && (IsHideLocker || IsHideBed))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // NOTE:k.oishi
    //      隠れているか+息止めしているかを調べれるようにIsHideStealth()を変更しましたが、
    //      IsHideStealth()を影人間、鬼、ツンのスクリプトでも使っているのでコンフリクト防止のために変更せずにコメント化しています。
    ///// <summary>
    ///// 隠れている状態かどうか
    ///// </summary>
    ///// <param name="isHold">息を止めいてるかどうか</param>
    ///// <returns></returns>
    //public bool IsHide(HideObjectType type, bool isHold)
    //{
    //    // 隠れているかどうかのフラグ
    //    bool ishide = false;
    //
    //    // タイプによって隠れているかどうかのフラグを立てる
    //    switch (type)
    //    {
    //        case HideObjectType.LOCKER: if (isHideLocker) { ishide = true; } break;
    //        case HideObjectType.BED: if (isHideBed) { ishide = true; } break;
    //        case HideObjectType.BOTH: if (isHideLocker || isHideBed) { ishide = true; } break;
    //    }
    //
    //    // 隠れているかどうかと、息を止めているかどうかも確認
    //    if (isHold)
    //    {
    //        if (isStealth && ishide)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    else
    //    {
    //        if (ishide)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}

    /// <summary>
    /// 息切れしてしまったかどうか
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public bool IsBreathlessness()
    {
        return breathController.IsDisappear;
    }

    /// <summary>
    /// 回転フラグのセット関数
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    ///      ステートマシンの方で脱出方向へ回転させているのですが、回転している間は回転をアニメーションに任せる処理を切らないといけません。
    ///      なので、ステートマシンの方で回転が終わったらフラグを切れるように、フラグセット関数を用意しました。
    public void SetIsAnimRotation(bool flag)
    {
        IsAnimRotation = flag;
    }

    /// <summary>
    /// 息止めフラグのセット関数
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public void SetIsStealth(bool flag)
    {
        isStealth = flag;
    }

    /// <summary>
    /// 息止めキーがおされているかどうか
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public bool IsHoldBreathKey()
    {
        return keyController.GetKey(KeyType.HOLDBREATH);
    }

    /// <summary>
    /// 隠れるキーの入力段階で隠れているかどうかを取得
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public bool IsHide()
    {
        switch (keyInputStage)
        {
            case 1:
                if (!keyController.GetKey(KeyType.INTERACT))
                {
                    keyInputStage = 2;
                }
                return true;
            case 2:
                if (keyController.GetKeyDown(KeyType.INTERACT))
                {
                    keyInputStage = 3;
                    return false;
                }
                return true;
            default: return false;
        }
    }

    /// <summary>
    /// ロッカーの向きタイプのゲット関数
    /// </summary>
    /// NOTE: k.oishi ステートマシン用関数
    public List<DirType> GetHideObjWallContactDir()
    {
        return hideObjWallContactDir;
    }
}
