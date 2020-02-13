using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateTypeIdChanger : StateMachineBehaviour
{
    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animatorStateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetInteger("AnimatorStateTypeId", animator.GetInteger("NextStateTypeId"));
    }
}
