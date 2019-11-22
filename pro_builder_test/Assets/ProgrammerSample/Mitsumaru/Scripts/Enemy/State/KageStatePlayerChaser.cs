using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート：プレイヤーの追跡
/// </summary>
public class KageStatePlayerChaser : StateMachineBehaviour
{
    // ナビメッシュ
    NavMeshAgent navMesh = null;
    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 視野の範囲
    KageFieldOfView fieldOfView = null;
    // 警戒範囲
    KageVigilanceRange vigilanceRange = null;
    // 警戒範囲用コライダーのイベント
    ColliderEvent vigilanceRangeEvent = default;

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    // プレイヤーを見つけている
    bool isDiscover = false;
    // 警戒範囲内にいる
    bool isInVigilanceRange = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 視野の範囲
        fieldOfView = animator.transform.Find("Collider").Find("KageFeildOfView").GetComponent<KageFieldOfView>() ?? fieldOfView;
        // 視野の範囲を設定する
        fieldOfView.ChangeDistance(KageState.Kind.Fight);

        // 警戒範囲
        vigilanceRange = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<KageVigilanceRange>() ?? vigilanceRange;
        //警戒範囲の設定を行う
        vigilanceRange.ChangeRadius(KageState.Kind.Fight);

        // 警戒範囲用コライダーのイベントクラスを取得
        vigilanceRangeEvent = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<ColliderEvent>() ?? vigilanceRangeEvent;

        // ハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;

        // 視界の中にいるときのコールバックをセット
        fieldOfView.SetOnInViewRangeEvent(OnInFieldOfView);
        // 警戒範囲内にいるときのコールバックをセット
        vigilanceRangeEvent.AddUpdateListener(OnInVigilanceRange);
    }

    /// <summary>
    /// プレイヤーの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 影人間の移動が停止したら、見失ったとみなす
        if (navMesh.remainingDistance < 0.3f)
        {
            animParameterList.SetBool(KageAnimParameterList.ParameterType.isFightingMode, false);
            animParameterList.SetBool(KageAnimParameterList.ParameterType.isVigilanceMode, true);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 視野の範囲と警戒範囲ともに警戒状態時のものに変更する
        fieldOfView.ChangeDistance(KageState.Kind.Vigilance);
        vigilanceRange.ChangeRadius(KageState.Kind.Fight);

        vigilanceRangeEvent.RemoveUpdateListener(OnInVigilanceRange);
    }

    /// <summary>
    /// 影人間の視界の中にいる
    /// </summary>
    void OnInFieldOfView(Transform self,Collider target)
    {
        // プレイヤーであれば
        if (target.tag == "Player")
        {
            // プレイヤーの位置を取得する
            navMesh.SetDestination(target.transform.position);
        }
    }

    /// <summary>
    /// 影人間の警戒範囲内にいる
    /// <summary>
    void OnInVigilanceRange(Transform self, Collider target)
    {
        // 隠れていて、かつ息を止めていればスキップ
        if (playerHideController.IsHideStealth())
        {
            Debug.Log("year!!!");
            return;
        }

        // プレイヤーであれば
        if (target.tag == "Player")
        {
            // プレイヤーの位置を取得する
            navMesh.SetDestination(target.transform.position);
        }
    }
}
