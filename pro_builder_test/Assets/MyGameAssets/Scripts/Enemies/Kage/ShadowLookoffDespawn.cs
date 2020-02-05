using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowLookoffDespawn : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer meshRenderer = default;

    [SerializeField]
    Transform player = default;

    [SerializeField]
    float distance = 0;

    void Update()
    {
        if (!meshRenderer.isVisible)
        {
            float currentDistance = (player.position - transform.position).magnitude;
            if (currentDistance < distance)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
