using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// トリガーイベントを他のイベントに通知する人
/// </summary>
public class TriggerEventDispatcher : MonoBehaviour
{
    // コライダー用のUnityEvent
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    // スフィアコライダー
    SphereCollider sphereCollider = default;

    // 範囲
    [SerializeField]
    [Range(0.1f, 100)]
    float radius = 1;
    public float Radius { get { return radius; } set { radius = value; } }

    // 対象のレイヤーマスク
    [SerializeField]
    LayerMask targetLayerMask = default;

    [Space(15)]

    [SerializeField]
    ColliderEvent enterEvent = default;

    [SerializeField]
    ColliderEvent stayEvent = default;

    [SerializeField]
    ColliderEvent exitEvent = default;

    void Start()
    {
        if (Radius > 0)
        {
            GameObject colliderObject = new GameObject("Collider");
            colliderObject.transform.parent = transform;
            colliderObject.transform.localPosition = Vector3.zero;
            Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            sphereCollider = colliderObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = radius;
        }
    }

    void Update()
    {
        if (Radius > 0)
        {
            sphereCollider.radius = radius;
        }
    }

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
