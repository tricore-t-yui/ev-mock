using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KageSpawnTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject kage = default;

    NavMeshAgent navMeshAgent = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            kage.SetActive(true);
        }
    }
}
