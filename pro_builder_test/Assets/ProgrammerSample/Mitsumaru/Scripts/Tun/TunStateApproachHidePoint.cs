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

    TunAreaData areaData;
    GameObject firstCheckingHide = null;
    GameObject currentCheckingHide = null;
    int currentHideIndex = 0;
    int checkedHideCount = 0;

    /// <summary>
    /// ステート開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;

        areaDataManager = FindObjectOfType<TunAreaDataManager>() ?? areaDataManager;
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;

        firstCheckingHide = hideController.HideObj ?? firstCheckingHide;

        areaData = areaDataManager.GetTunAreaData(firstCheckingHide);
        List<GameObject> hideObjectToList = areaData.HideObject.ToList();
        int firstHideIndex = hideObjectToList.IndexOf(firstCheckingHide);
        currentHideIndex = firstHideIndex + checkedHideCount;
        if (currentHideIndex > hideObjectToList.Count -1) { currentHideIndex = 0; }
        currentCheckingHide = hideObjectToList[currentHideIndex];

        navMesh.SetDestination(currentCheckingHide.transform.position + (currentCheckingHide.transform.forward *-1) * hideToTaegetDistance);
    }

    /// <summary>
    /// ステートの更新
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!(navMesh.remainingDistance < navMesh.stoppingDistance)) { return; }
        Vector3 hideObjPos = currentCheckingHide.transform.position;
        animator.transform.LookAt(new Vector3(hideObjPos.x,animator.transform.position.y,hideObjPos.z));

        if (Vector3.Angle(animator.transform.forward, currentCheckingHide.transform.forward) < 0.1f)
        {
            animator.SetBool("isApproachingHide", false);

            if (hideController.enabled)
            {
                if (currentCheckingHide.GetInstanceID() == hideController.HideObj.GetInstanceID())
                {
                    animator.SetBool("isPlayerDiscover", true);
                    animator.SetTrigger("attackStart");
                }
            }
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        checkedHideCount++;
        if (checkedHideCount == areaData.HideObject.Count)
        {
            animator.SetBool("isHideCheckEnd",true);
            checkedHideCount = 0;
        }
    }
}
