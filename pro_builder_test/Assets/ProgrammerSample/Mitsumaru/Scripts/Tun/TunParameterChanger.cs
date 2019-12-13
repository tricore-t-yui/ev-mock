using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunParameterChanger : StateMachineBehaviour
{
    PlayerHideController hideController = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        hideController = FindObjectOfType<PlayerHideController>() ?? hideController;
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
    }
}
