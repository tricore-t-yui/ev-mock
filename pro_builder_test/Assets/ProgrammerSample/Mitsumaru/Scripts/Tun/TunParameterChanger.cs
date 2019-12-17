using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ツンのパラメータを制御する
/// </summary>
public class TunParameterChanger : StateMachineBehaviour
{
    // ハイドコントローラー
    PlayerHideController hideController = null;
    // 鬼の視界の制御
    KageFieldOfView fieldOfView = null;
    // エリアデータ管理クラス
    TunAreaDataManager areaDataManager = null;
    // エリア管理クラス
    AreaManager areaManager = null;
    // 鬼の聴こえる範囲
    ColliderEvent vigilanceRangeEvent = null;

    // ツンの現在のエリアデータ
    TunAreaData areaData;

    // プレイヤーをとらえたかどうか
    bool isInViewRange = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ハイドコントローラを取得
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;
        // 視界の制御クラスを取得
        fieldOfView = animator.GetComponentInChildren<KageFieldOfView>() ?? fieldOfView;
        // エリアデータ管理クラスを取得
        areaDataManager = FindObjectOfType<TunAreaDataManager>() ?? areaDataManager;
        // エリア管理クラスを取得
        areaManager = FindObjectOfType<AreaManager>() ?? areaManager;
        // 聴こえる範囲の制御クラスを取得
        vigilanceRangeEvent = animator.transform.Find("Collider").Find("OniVigilanceRange").GetComponent<ColliderEvent>() ?? vigilanceRangeEvent;
        // 視界にとらえたときのイベントを追加
        fieldOfView.SetOnInViewRangeEvent(OnInFieldOfView);
        // 何かしらの物音を聴いたとき
        vigilanceRangeEvent.AddEnterListener(OnInFieldOfView);

        // ハイドポイントから属しているエリアデータを取得
        areaData = areaDataManager.GetTunAreaData(hideController.HideObj.GetInstanceID());
        // ハイドポイントのインスタンスIDをセット
        animator.SetInteger("hideObjectInstanceId",hideController.HideObj.GetInstanceID());
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // プレイヤーが隠れているとき
        if (hideController.enabled)
        {
            // プレイヤーが隠れいているときのフラグをオンにする
            animator.SetBool("isPlayerHide", true);

            // 隠れているオブジェクトによってintパラメータにIDをセット
            // ロッカー
            if (hideController.HideObj.tag == "Locker")
            {
                animator.SetInteger("CheckHideType", 0);
            }
            // ベッド
            else
            {
                animator.SetInteger("CheckHideType", 1);
            }
        }
        // 隠れていない
        else
        {
            // フラグをオフに
            animator.SetBool("isPlayerHide", false);
        }

        // プレイヤーを視界にとらえている場合
        if (isInViewRange)
        {
            // ハイドポイント接近フラグをオフに
            animator.SetBool("isApproachingHide", false);
            // プレイヤーを見つけているときのフラグをオンに
            animator.SetBool("isPlayerDiscover", true);
        }
        // プレイヤーを見つけていないとき
        else
        {
            animator.SetBool("isPlayerDiscover", false);
        }
    }

    /// <summary>
    /// 影人間の視界の中にいる
    /// </summary>
    void OnInFieldOfView(Transform self, Collider target)
    {
        // プレイヤーを見つけているフラグをオンに
        isInViewRange = true;
    }
}
