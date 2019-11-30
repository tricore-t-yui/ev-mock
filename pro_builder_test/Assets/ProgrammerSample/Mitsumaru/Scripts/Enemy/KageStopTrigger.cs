using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KageStopTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject kage = default;

    NavMeshAgent navMeshAgent = null;

    private void Start()
    {
        navMeshAgent = kage.GetComponent<NavMeshAgent>() ?? navMeshAgent;
    }

    /// <summary>
    /// コライダーに触れたら
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kage")
        {
            navMeshAgent.isStopped = true;
        }
    }
}
