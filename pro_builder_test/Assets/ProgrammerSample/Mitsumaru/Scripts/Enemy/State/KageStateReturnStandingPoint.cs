using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間のステート：最初の待機位置に戻る
/// </summary>
public class KageStateReturnStandingPoint : StateMachineBehaviour
{
    // 待機型のステート
    KageStateStanding stateStanding = null;

    // 影人間のパラメータークラス
    KageAnimParameterList animParameterList = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // 待機型のステートを取得
        stateStanding = animator.GetBehaviour<KageStateStanding>() ?? stateStanding;

        // パラメータクラスを取得
        animParameterList = animator.GetComponent<KageAnimParameterList>() ?? animParameterList;

        // 移動目標位置をセット
        stateStanding.ReturnStandingPosition();
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置に着いたら、待機モードに変更
        if (navMesh.remainingDistance < 0.3f)
        {
            animParameterList.SetBool(KageAnimParameterList.ParameterType.isVigilanceMode, false);
        }
    }
}
