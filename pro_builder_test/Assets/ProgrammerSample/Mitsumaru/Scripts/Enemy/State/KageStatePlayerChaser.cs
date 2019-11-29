﻿using System.Collections;
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

    // ダメージを受けたかどうか
    bool isDamage = false;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        isDamage = false;

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
        kageBodyEvent = animator.transform.Find("Collider").Find("KageAttackRange").GetComponent<ColliderEvent>() ?? kageBodyEvent;
        // コールバックをセット
        kageBodyEvent.AddUpdateListener(OnCollisionPlayer);

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
        Debug.DrawRay(animator.transform.position + new Vector3(0, 0.2f, 0), animator.transform.forward);

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

    void OnCollisionPlayer(Transform self,Collider target)
    {
        if (isDamage == true) { return; }
        if (target.tag == "PlayerNoise") { return; }

        // プレイヤーがハイドポイントに隠れているか
        if (playerHideController.enabled)
        {
            // 隠れているオブジェクトを取得
            GameObject hideObject = playerHideController.HideObj;

            // 隠れているオブジェクトと敵が衝突しているオブジェクトが同じか判定する
            // note : 違っていれば関数を抜ける
            if (hideObject.GetInstanceID() != target.gameObject.GetInstanceID()) { return; }

            // 隠れているオブジェクトの種類を判定
            // ロッカー
            if (target.tag == "Locker")
            {
                animParameterList.SetTrigger(KageAnimParameterList.ParameterType.isAttackFromLocker);
            }
            // ベッド
            else if (target.tag == "Bed")
            {
                animParameterList.SetTrigger(KageAnimParameterList.ParameterType.isAttackFromBed);
            }
            // ダメージフラグを立てる
            isDamage = true;
        }
        // プレイヤーに衝突した場合
        else if (target.tag == "Player")
        {
            // 敵からプレイヤーに伸びるベクトルを算出
            Vector3 playerToKageVec = (target.transform.position - self.root.position).normalized;
            // 算出したベクトルとプレイヤーのベクトルの角度を算出
            float vecDot = Vector3.Angle(playerToKageVec, self.root.forward);
            // 角度が視野角の範囲内であれば攻撃する
            if (vecDot < 60)
            {
                // プレイヤーに攻撃
                animParameterList.SetTrigger(KageAnimParameterList.ParameterType.isAttack);
                // ダメージフラグを立てる
                isDamage = true;
            }
        }
    }
}
