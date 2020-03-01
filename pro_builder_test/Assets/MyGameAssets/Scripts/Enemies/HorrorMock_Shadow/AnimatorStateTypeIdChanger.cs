using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateTypeIdChanger : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetInteger("AnimatorStateTypeId", animator.GetInteger("NextStateTypeId"));
    }
}
