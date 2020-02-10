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
        int flag = 1 << other.gameObject.layer; 
        if ((targetLayerMask.value & flag) != 0)
        {
            enterEvent.Invoke(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        int flag = 1 << other.gameObject.layer;
        if ((targetLayerMask.value & flag) != 0)
        {
            stayEvent.Invoke(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        int flag = 1 << other.gameObject.layer;
        if ((targetLayerMask.value & flag) != 0)
        {
            exitEvent.Invoke(other);
        }
    }
}
