using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateColliderEvent : MonoBehaviour
{
    // コライダー用のUnityEvent
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }

    [SerializeField]
    LayerMask targetLayerMask = default;

    [Space(15)]

    [SerializeField]
    ColliderEvent enterEvent = default;

    [SerializeField]
    ColliderEvent stayEvent = default;

    [SerializeField]
    ColliderEvent exitEvent = default;

    void OnTriggerEnter(Collider other)
    {
        Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray,out raycastHit,Mathf.Infinity,targetLayerMask))
        {
            Debug.DrawRay(ray.origin,ray.direction,Color.yellow);
            enterEvent.Invoke(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, targetLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            stayEvent.Invoke(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, targetLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            exitEvent.Invoke(other);
        }
    }
}
