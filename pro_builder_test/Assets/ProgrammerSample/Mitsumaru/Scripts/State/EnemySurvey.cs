using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySurvey : StateMachineBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navMeshAgent.isStopped = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navMeshAgent.isStopped = false;
    }
}
