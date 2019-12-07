using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 鬼の体のコライダー
/// </summary>
public class OniBodyCollider : MonoBehaviour
{
    // アニメーター
    [SerializeField]
    Animator animator = default;

    // ボディコライダーイベント
    class BodyColliderEvent : UnityEvent<Transform,Collision,Animator> { }
    BodyColliderEvent colliderEvent = new BodyColliderEvent();

    /// <summary>
    /// 衝突した瞬間
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        colliderEvent.Invoke(transform.parent.parent, collision, animator);
    }

    /// <summary>
    /// コライダーイベントを追加
    /// </summary>
    /// <param name="call"></param>
    public void Add(UnityAction<Transform,Collision,Animator> call)
    {
        colliderEvent.AddListener(call);
    }
}
