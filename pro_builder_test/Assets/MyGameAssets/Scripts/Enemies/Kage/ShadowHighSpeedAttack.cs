using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 影人間：超高速で攻撃してきて消える
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class ShadowHighSpeedAttack : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    [SerializeField]
    NavMeshAgent navMeshAgent = default;

    [SerializeField]
    Transform player = default;

    [SerializeField]
    float runnungSpeed = 0;

    void OnEnable()
    {
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsRunning", true);
        navMeshAgent.speed = runnungSpeed;
        animator.SetFloat("RunningSpeed", runnungSpeed);
    }

    void Update()
    {
        transform.LookAt(new Vector3(player.position.x,transform.position.y,player.position.z));
        navMeshAgent.SetDestination(player.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            navMeshAgent.isStopped = true;
            animator.SetTrigger("Attack");
            animator.SetTrigger("Despawn");
        }
    }
}
