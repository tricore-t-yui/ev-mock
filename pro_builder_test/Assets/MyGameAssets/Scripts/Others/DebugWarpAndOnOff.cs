using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugWarpAndOnOff : MonoBehaviour
{
    public Transform warpPoint;
    public Transform target;
    public List<GameObject> onObj;
    public List<GameObject> offObj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        target.transform.position = warpPoint.position;
        foreach (var item in onObj)
        {
            item.SetActive(true);
        }
        foreach (var item in offObj)
        {
            item.SetActive(false);
        }
    }
}
