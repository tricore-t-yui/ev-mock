using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunParameterChanger : StateMachineBehaviour
{
    PlayerHideController hideController = null;
    KageFieldOfView fieldOfView = null;
    ColliderEvent vigilanceRangeEvent = null;

    bool isInViewRange = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;
        fieldOfView = animator.GetComponentInChildren<KageFieldOfView>() ?? fieldOfView;
        vigilanceRangeEvent = animator.transform.Find("Collider").Find("OniVigilanceRange").GetComponent<ColliderEvent>() ?? vigilanceRangeEvent;
        fieldOfView.SetOnInViewRangeEvent(OnInFieldOfView);
        fieldOfView.SetOnOutViewRangeEvent(OnOutFieldOfView);
        vigilanceRangeEvent.AddEnterListener(OnInFieldOfView);
        vigilanceRangeEvent.AddExitListener(OnOutFieldOfView);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (hideController.enabled)
        {
            animator.SetBool("isPlayerHide", true);

            if (hideController.HideObj.tag == "Locker")
            {
                animator.SetInteger("CheckHideType", 0);
            }
            else
            {
                animator.SetInteger("CheckHideType", 1);
            }
        }
        else
        {
            animator.SetBool("isPlayerHide", false);
        }

        if (isInViewRange)
        {
            animator.SetBool("isApproachingHide", false);
            animator.SetBool("isPlayerDiscover", true);
        }
        else
        {
            animator.SetBool("isPlayerDiscover", false);
        }
    }

    /// <summary>
    /// 影人間の視界の中にいる
    /// </summary>
    void OnInFieldOfView(Transform self, Collider target)
    {
        isInViewRange = true;
    }

    /// <summary>
    /// 影人間の視野の範囲外にいる
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    void OnOutFieldOfView(Transform self, Collider target)
    {
        isInViewRange = false;
    }
}
