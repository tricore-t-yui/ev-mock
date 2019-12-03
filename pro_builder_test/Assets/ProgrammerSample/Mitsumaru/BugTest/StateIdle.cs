using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : StateBase
{
    bool isEnter = false;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!isEnter)
        {
            Debug.Log(animator.gameObject.name);
            isEnter = true;
        }
    }
}
