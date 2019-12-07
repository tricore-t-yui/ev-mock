using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OniStateNearTheHideMove : StateMachineBehaviour
{
    int moveTimeCounter = 0;

    [SerializeField]
    int otherActionTime = 0;
    int otherActionTimeCounter = 0;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    Vector3 center = Vector3.zero;

    // 基準位置をセットしたかどうか
    bool isSetCenterPos = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // ナビメッシュを取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        navMesh.speed = animator.GetFloat("moveSpeed");

        if (!isSetCenterPos)
        {
            // 到着した位置を基準位置に設定
            center = animator.transform.position;
            isSetCenterPos = true;
        }

        float searchingRange = animator.GetFloat("searchingRange");
        // ランダムで位置をセット
        Vector3 targetPos = new Vector3(Random.Range(center.x - searchingRange, center.x + searchingRange),
                                        Random.Range(center.y - searchingRange, center.y + searchingRange),
                                        Random.Range(center.z - searchingRange, center.z + searchingRange));

        // 目標位置をセット
        navMesh.SetDestination(targetPos);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (navMesh.remainingDistance < navMesh.stoppingDistance)
        {
            animator.SetTrigger("moveEndContinue");
        }

        otherActionTimeCounter++;
        if (otherActionTimeCounter >= otherActionTime)
        {
            animator.SetTrigger("lotteryStart");
            otherActionTimeCounter = 0;
        }

        moveTimeCounter++;
        if (moveTimeCounter >= animator.GetInteger("searchingTime"))
        {
            moveTimeCounter = 0;
            animator.SetBool("isNearTheHideMove", false);
        }
    }
}
