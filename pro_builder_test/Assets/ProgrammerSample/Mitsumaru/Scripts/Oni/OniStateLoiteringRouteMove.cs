using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 徘徊ルートを移動
/// </summary>
public class OniStateLoiteringRouteMove : StateMachineBehaviour
{
    // 徘徊ルートマネージャー
    OniLoiteringRouteManager searchingRouteManager = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // 現在のチェックポイントのインデックス
    int currentCheckPointIndex = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        // ルートマネージャーを取得
        searchingRouteManager = FindObjectOfType<OniLoiteringRouteManager>() ?? searchingRouteManager;

        // 最初のチェックポイントのインデックスをセット
        currentCheckPointIndex = searchingRouteManager.NearCheckPointOfIndex;
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 目標のチェックポイントに着いたら
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            // 次のチェックポイントを設定する
            currentCheckPointIndex = GetNextCheckPointIndex();
            navMesh.SetDestination(searchingRouteManager.NearCheckPointOfRoute.CheckPoints[currentCheckPointIndex]);
        }
    }

    /// <summary>
    /// 次のチェックポイントのインデックスを取得
    /// </summary>
    /// <returns></returns>
    int GetNextCheckPointIndex()
    {
        if (currentCheckPointIndex >= searchingRouteManager.NearCheckPointOfRoute.CheckPoints.Count - 1)
        {
            return 0;
        }
        else
        {
            return currentCheckPointIndex + 1;
        }
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // トリガーをリセット
        animator.SetBool("isLoiteringRouteMove", false);
    }
}
