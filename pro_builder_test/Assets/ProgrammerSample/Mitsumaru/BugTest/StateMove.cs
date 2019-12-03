using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : StateMachineBehaviour
{
    [SerializeField]
    StateIdle stateIdle = default;

    [SerializeField]
    StateWalk stateWalk = default;

    StateBase state = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        TestState testState = animator.GetBehaviour<TestState>();

        if (testState.StateId == 1)
        {
            state = stateIdle;
        }
        else if (testState.StateId == 2)
        {
            state = stateWalk;
        }
        
        state.OnStateEnter(animator, animatorStateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        state.OnStateUpdate(animator, animatorStateInfo, layerIndex);
    }
}
