using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート：最初の待機位置に戻る
/// </summary>
public class KageStateReturnStandingPoint : StateMachineBehaviour
{
    // 移動速度
    [SerializeField]
    float speed = 0;

    // 待機型のステート
    KageStateStanding stateStanding = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 警戒範囲
    KageVigilanceRange vigilanceRange = null;

    // ワープする目標位置までの距離
    [SerializeField]
    float warpDistance = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 移動速度をセット
        navMesh.speed = speed;

        // 待機型のステートを取得
        stateStanding = animator.GetBehaviour<KageStateStanding>() ?? stateStanding;

        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 警戒範囲
        vigilanceRange = animator.transform.Find("Collider").Find("KageVigilanceRange").GetComponent<KageVigilanceRange>() ?? vigilanceRange;

        // 移動目標位置をセット
        stateStanding.ReturnStandingPosition();

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
        // 目標位置に着いたら、待機モードに変更
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            animParameterList.SetBool(KageAnimParameterList.ParameterType.isVigilanceMode, false);
            // 警戒範囲を通常状態に変更
            vigilanceRange.ChangeRadius(KageState.Kind.Normal);

            animator.SetBool("isReturnPoint", false);

            // 状態レベルを変更
            animator.SetInteger("currentStateLevel", 0);
        }
    }
}
