using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class TunStateApproachHidePoint : StateMachineBehaviour
{
    [SerializeField]
    float hideToTaegetDistance = 0;

    NavMeshAgent navMesh = null;

    TunAreaDataManager areaDataManager = null;

    PlayerHideController hideController = null;

    GameObject checkingHide = null;
    int hidePointIdListIndex = 0;
    int checkedHideCount = 0;

    /// <summary>
    /// ステート開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        areaDataManager = FindObjectOfType<TunAreaDataManager>() ?? areaDataManager;
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;

        TunAreaData areaData = areaDataManager.GetTunAreaData(hideController.HideObj);
        List<GameObject> hideObjectToList = areaData.HideObject.ToList();
        checkingHide = hideController.HideObj;
        hidePointIdListIndex = hideObjectToList.IndexOf(checkingHide);
        navMesh.SetDestination(checkingHide.transform.position + checkingHide.transform.forward * hideToTaegetDistance);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!(navMesh.remainingDistance < navMesh.stoppingDistance)) { return; }

        if (Vector3.Angle(animator.transform.forward, checkingHide.transform.forward * -1) < 0.1f)
        {
            animator.SetBool("isApproachingHide", false);

            if (hideController.enabled)
            {
                if (checkingHide.GetInstanceID() == hideController.HideObj.GetInstanceID())
                {
                    animator.SetBool("isPlayerDiscover", true);
                    animator.SetTrigger("attackStart");
                }
            }
        }

    }
}
