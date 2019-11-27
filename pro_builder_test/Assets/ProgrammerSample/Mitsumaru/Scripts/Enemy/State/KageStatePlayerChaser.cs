using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Linq;

/// <summary>
/// 影人間のステート：プレイヤーの追跡
/// </summary>
public class KageStatePlayerChaser : StateMachineBehaviour
{
    // 移動速度
    [SerializeField]
    float speed = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;
    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 視野の範囲
    KageFieldOfView fieldOfView = null;
    // 警戒範囲
    KageVigilanceRange vigilanceRange = null;
    // 警戒範囲用コライダーのイベント
    ColliderEvent vigilanceRangeEvent = null;
    // 影人間自信のコライダーイベント
    ColliderEvent kageBodyEvent = null;

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    // 視野の範囲内にいるかどうか
    bool isInFieldOfView = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 移動速度をセット
        navMesh.speed = speed;

        navMesh.isStopped = false;

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

        // 影人間の自信のコライダーイベントを取得
        kageBodyEvent = animator.transform.Find("Collider").Find("KageBody").GetComponent<ColliderEvent>() ?? kageBodyEvent;
        // コールバックをセット
        kageBodyEvent.AddEnterListener(OnCollisionPlayer);

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
        if (navMesh.remainingDistance < 0.1f)
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
    /// 影人間の視野の範囲外にいる
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    void OnOutFieldOfView(Transform self, Collider target)
    {

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

    void OnCollisionPlayer(Transform self,Collision target)
    {
        // 影人間の前方にレイを飛ばす
        Ray forwardRay = new Ray(self.transform.position + new Vector3(0, 0.1f, 0), self.transform.forward);
        Debug.DrawRay(self.transform.position + new Vector3(0, 0.1f, 0), self.transform.forward);
        RaycastHit[] raycastHitAll = Physics.RaycastAll(forwardRay);

        bool isHitPlayer = System.Array.Exists(raycastHitAll, elem => elem.collider.gameObject.layer == LayerMask.NameToLayer("Player"));
        bool isHitLocker = System.Array.Exists(raycastHitAll, elem => elem.collider.gameObject.layer == LayerMask.NameToLayer("Locker"));
        bool isHitBed    = System.Array.Exists(raycastHitAll, elem => elem.collider.gameObject.layer == LayerMask.NameToLayer("Bed"));

        // プレイヤーに衝突
        if (isHitPlayer)
        { 
            // ロッカーに衝突
            if (isHitLocker)
            {
                animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttackFromLocker, true);
            }
            // ベッドに衝突
            else if (isHitBed)
            {
                animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttackFromBed, true);
            }
            // プレイヤーのみ
            else
            {
                animParameterList.SetBool(KageAnimParameterList.ParameterType.isAttack, true);
            }

        }
    }
}
