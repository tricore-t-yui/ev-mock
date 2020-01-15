using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 移動できなくなったときの壁抜け処理
/// </summary>
public class OniObstacleThrough : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navMeshAgent = default;
    [SerializeField]
    int stopTime = 0;
    int stopCounter = 0;

    private void Update()
    {
        bool isPathPartial = (navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial);
        bool isStopped = (navMeshAgent.velocity.magnitude < 0.1f);

        if (isPathPartial && isStopped)
        {
            stopCounter++;

            if (stopCounter > stopTime)
            {
                if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    stopCounter = 0;
                    return;
                }

                Vector3 newPos = transform.position + transform.forward * 1.5f;
                navMeshAgent.Warp(newPos);
            }
        }
    }
}
