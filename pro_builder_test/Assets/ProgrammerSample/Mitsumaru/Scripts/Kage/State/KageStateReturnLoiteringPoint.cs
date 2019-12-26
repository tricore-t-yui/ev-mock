using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 元の徘徊ポイントに戻る
/// </summary>
public class KageStateReturnLoiteringPoint : StateMachineBehaviour
{
    // 移動速度
    [SerializeField]
    float speed = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 徘徊
    KageStateLoitering stateLoitering = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // 視野の範囲
    KageFieldOfView fieldOfView = null;
    // 警戒範囲
    KageVigilanceRange vigilanceRange = null;

    KageStateMoveAtRoute stateMoveAtRoute = null;
    KageStateMoveAtRandom stateMoveAtRandom = null;

    // ワープする目標位置までの距離
    [SerializeField]
    float warpDistance = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュ取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動速度をセット
        navMesh.speed = speed;

        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        stateMoveAtRoute = animator.GetBehaviour<KageStateMoveAtRoute>() ?? stateMoveAtRoute;
        stateMoveAtRandom = animator.GetBehaviour<KageStateMoveAtRandom>() ?? stateMoveAtRandom;

        if (animParameterList.GetInteger(KageAnimParameterList.ParameterType.loiteringKindId) == 1)
        {
            stateMoveAtRoute.ReturnVigilancePoint(animator);
        }
        else if (animParameterList.GetInteger(KageAnimParameterList.ParameterType.loiteringKindId) == 2)
        {
            stateMoveAtRandom.ReturnLoiteringPoint(animator);
        }

        // 視野の範囲
        fieldOfView = animator.transform.Find("Collider").Find("KageFeildOfView").GetComponent<KageFieldOfView>() ?? fieldOfView;
        // 視野の範囲を設定する
        fieldOfView.ChangeDistance(KageState.Kind.Vigilance);

        // 警戒範囲
        vigilanceRange = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<KageVigilanceRange>() ?? vigilanceRange;
        //警戒範囲の設定を行う
        vigilanceRange.ChangeRadius(KageState.Kind.Vigilance);

        // 帰るフラグをオンにする
        animator.SetBool("isReturnPoint", true);

        // 目標位置が一定以上離れていたら
        if ((animator.transform.position - navMesh.destination).magnitude > warpDistance)
        {
            // 目標位置にワープする
            navMesh.Warp(navMesh.destination);
        }
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置に着いたらそのまま徘徊を再開
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            animParameterList.SetBool(KageAnimParameterList.ParameterType.isVigilanceMode, false);
            // 警戒範囲を通常状態に変更
            vigilanceRange.ChangeRadius(KageState.Kind.Normal);

            animator.SetBool("isReturnPoint", false);
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 視野の範囲
        fieldOfView = animator.transform.Find("Collider").Find("KageFeildOfView").GetComponent<KageFieldOfView>() ?? fieldOfView;
        // 視野の範囲を設定する
        fieldOfView.ChangeDistance(KageState.Kind.Normal);

        // 警戒範囲
        vigilanceRange = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<KageVigilanceRange>() ?? vigilanceRange;
        //警戒範囲の設定を行う
        vigilanceRange.ChangeRadius(KageState.Kind.Normal);
    }
}
