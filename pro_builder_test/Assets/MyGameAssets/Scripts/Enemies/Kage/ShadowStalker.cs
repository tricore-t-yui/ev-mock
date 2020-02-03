using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowStalker : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    [SerializeField]
    Transform player = default;

    [SerializeField]
    float walkingSpeed = 0;

    [SerializeField]
    float stopDistance = 0;

    [SerializeField]
    float walkingDistance = 0;

    float currentWalkingDistance = 0;

    void OnEnable()
    {
        animator.SetBool("IsMoving", true);
        navMeshAgent.speed = walkingSpeed;
        animator.SetFloat("WalkingSpeed", walkingSpeed);
        navMeshAgent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        navMeshAgent.SetDestination(player.position);
        currentWalkingDistance += Mathf.Round(navMeshAgent.velocity.magnitude) / 60;

        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            animator.SetBool("IsMoving", false);
        }
        else
        {
            animator.SetBool("IsMoving", true);
        }

        if (currentWalkingDistance > walkingDistance)
        {
            animator.SetTrigger("Despawn");
        }
        Debug.Log(currentWalkingDistance);
    }

    void OnDisable()
    {
        navMeshAgent.stoppingDistance = 0.1f;
    }
}
