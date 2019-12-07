using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 探索を諦めて、一番近いチェックポイントに移動
/// </summary>
public class OniStateMoveNearCheckPoint : StateMachineBehaviour
{
    // ルートマネージャー
    OniLoiteringRouteManager searchingRouteManager = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // ルートマネージャーを取得
        searchingRouteManager = FindObjectOfType<OniLoiteringRouteManager>() ?? searchingRouteManager;
        // 登録されている一番近いチェックポイントを更新する
        searchingRouteManager.UpdateNearRoute();
        // 一番近いチェックポイントを移動目標位置として設定
        navMesh.SetDestination(searchingRouteManager.NearCheckPointOfRoute.CheckPoints[searchingRouteManager.NearCheckPointOfIndex]);
        
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標位置に着いたら、徘徊フラグをセット
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            animator.SetBool("isLoiteringRouteMove", true);
        }
    }
}
