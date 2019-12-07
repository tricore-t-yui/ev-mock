using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 鬼の体のコライダー
/// </summary>
public class OniBodyCollider : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    class BodyColliderEvent : UnityEvent<Transform,Collision,Animator> { }
    BodyColliderEvent colliderEvent = new BodyColliderEvent();

    private void OnCollisionEnter(Collision collision)
    {
        colliderEvent.Invoke(transform.parent.parent, collision, animator);
    }

    public void Add(UnityAction<Transform,Collision,Animator> call)
    {
        colliderEvent.AddListener(call);
    }
}
