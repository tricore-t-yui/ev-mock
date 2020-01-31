using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniDespawnTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject oni = default;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            oni.SetActive(false);
        }
    }
}
