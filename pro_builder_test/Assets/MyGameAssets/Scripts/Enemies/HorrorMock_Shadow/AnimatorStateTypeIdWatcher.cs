using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateTypeIdWatcher : StateMachineBehaviour
{
    [SerializeField]
    int myAnimationStateTypeId = 0;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var next = animator.GetInteger("NextStateTypeId");
        var anim = animator.GetInteger("AnimatorStateTypeId");
        // 設定中に変更された場合の備え
        if (myAnimationStateTypeId == next)
        {
            animator.SetInteger("AnimatorStateTypeId", next);
        }
        else if (myAnimationStateTypeId != anim)
        {
            animator.SetInteger("NextStateTypeId", next);
        }
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
}
