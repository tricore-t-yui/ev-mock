using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OniStateMoveNearCheckPoint : StateMachineBehaviour
{
    OniSearchingRouteManager searchingRouteManager = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュのコンポーネントを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        searchingRouteManager = FindObjectOfType<OniSearchingRouteManager>() ?? searchingRouteManager;
        searchingRouteManager.UpdateNearRoute();
        navMesh.SetDestination(searchingRouteManager.NearCheckPointOfRoute.CheckPoints[searchingRouteManager.NearCheckPointOfIndex]);
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            animator.SetBool("isLoiteringRouteMove", true);
        }
    }
}
