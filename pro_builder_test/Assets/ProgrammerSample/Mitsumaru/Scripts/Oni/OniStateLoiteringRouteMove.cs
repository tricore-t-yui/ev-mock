using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OniStateLoiteringRouteMove : StateMachineBehaviour
{
    OniSearchingRouteManager searchingRouteManager = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    int currentCheckPointIndex = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        searchingRouteManager = FindObjectOfType<OniSearchingRouteManager>() ?? searchingRouteManager;

        currentCheckPointIndex = searchingRouteManager.NearCheckPointOfIndex;
    }

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

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("isLoiteringRouteMove", false);
    }
}
