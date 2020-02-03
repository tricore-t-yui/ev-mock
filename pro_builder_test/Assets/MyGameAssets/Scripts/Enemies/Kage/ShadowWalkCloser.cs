using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間：歩いて近づいて消える
/// </summary>
public class ShadowWalkCloser : MonoBehaviour
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
    float distance = 0;

    void OnEnable()
    {
        animator.SetBool("IsMoving", true);
        navMeshAgent.speed = walkingSpeed;
        animator.SetFloat("WalkingSpeed",walkingSpeed);
    }

    void Update()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        navMeshAgent.SetDestination(player.position);

        float currentDistance = (player.position - transform.position).magnitude;
        if (currentDistance < distance)
        {
            animator.SetTrigger("Despawn");
        }
    }
}
