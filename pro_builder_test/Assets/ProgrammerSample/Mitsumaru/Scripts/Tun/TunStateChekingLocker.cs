using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunStateChekingLocker : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animator.GetBool("isHideCheckEnd"))
        {
            animator.gameObject.SetActive(false);
        }
        else
        {
            animator.SetBool("isApproachingHide", true);
        }
    }
}
