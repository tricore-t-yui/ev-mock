using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : StateMachineBehaviour
{
    [System.NonSerialized]
    bool isEnter = false;

    [SerializeField]
    int stateId = 0;
    public int StateId { get; }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (!isEnter)
        {
            isEnter = true;
        }
    }
}
