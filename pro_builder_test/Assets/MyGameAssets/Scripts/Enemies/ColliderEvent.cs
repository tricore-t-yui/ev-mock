using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// コライダーとの衝突でUnityEventを呼ぶ汎用クラス
/// </summary>
[RequireComponent(typeof(Collider))]
public class ColliderEvent : MonoBehaviour
{
    class TriggerUnityEvent : UnityEvent<Transform, Collider> { }
    class CollisionUnityEvent : UnityEvent<Transform, Collision> { }

    [SerializeField]
    List<string> targetTagNames = default;

    TriggerUnityEvent onTriggerEnter = new TriggerUnityEvent();
    TriggerUnityEvent onTriggerUpdate = new TriggerUnityEvent();
    TriggerUnityEvent onTriggerExit = new TriggerUnityEvent();
    CollisionUnityEvent onCollisionEnter = new CollisionUnityEvent();
    CollisionUnityEvent onCollisionUpdate = new CollisionUnityEvent();
    CollisionUnityEvent onCollisionExit = new CollisionUnityEvent();

    /// <summary>
    /// 触れた瞬間のコールバック追加
    /// </summary>
    public void AddEnterListener(UnityAction<Transform,Collider> call)
    {
        onTriggerEnter?.AddListener(call);
    }

    /// <summary>
    /// 触れ続けているときののコールバック追加
    /// </summary>
    public void AddUpdateListener(UnityAction<Transform, Collider> call)
    {
        onTriggerUpdate?.AddListener(call);
    }

    /// <summary>
    /// 離れたのコールバック追加
    /// </summary>
    public void AddExitListener(UnityAction<Transform, Collider> call)
    {
        onTriggerExit?.AddListener(call);
    }

    /// <summary>
    /// 衝突した瞬間のコールバック追加
    /// </summary>
    public void AddEnterListener(UnityAction<Transform, Collision> call)
    {
        onCollisionEnter?.AddListener(call);
    }

    /// <summary>
    /// 衝突した瞬間のコールバック追加
    /// </summary>
    public void AddUpdateListener(UnityAction<Transform, Collision> call)
    {
        onCollisionUpdate?.AddListener(call);
    }

    /// <summary>
    /// 衝突した瞬間のコールバック追加
    /// </summary>
    public void AddExitListener(UnityAction<Transform, Collision> call)
    {
        onCollisionExit?.AddListener(call);
    }

    /// <summary>
    /// 触れた瞬間のコールバックを削除
    /// </summary>
    public void RemoveEnterListener(UnityAction<Transform, Collider> call)
    {
        onTriggerEnter?.RemoveListener(call);
    }

    /// <summary>
    /// 触れ続けているときののコールバック削除
    /// </summary>
    public void RemoveUpdateListener(UnityAction<Transform, Collider> call)
    {
        onTriggerUpdate?.RemoveListener(call);
    }

    /// <summary>
    /// 離れたのコールバック削除
    /// </summary>
    public void RemoveExitListener(UnityAction<Transform, Collider> call)
    {
        onTriggerExit?.RemoveListener(call);
    }

    /// <summary>
    /// 衝突した瞬間のコールバック削除
    /// </summary>
    public void RemoveEnterListener(UnityAction<Transform, Collision> call)
    {
        onCollisionEnter?.RemoveListener(call);
    }

    /// <summary>
    /// 衝突した瞬間のコールバック削除
    /// </summary>
    public void RemoveUpdateListener(UnityAction<Transform, Collision> call)
    {
        onCollisionUpdate?.RemoveListener(call);
    }

    /// <summary>
    /// 衝突した瞬間のコールバック削除
    /// </summary>
    public void RemoveExitListener(UnityAction<Transform, Collision> call)
    {
        onCollisionExit?.RemoveListener(call);
    }

    /// <summary>
    /// 触れた瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (targetTagNames.Find(x => x == other.tag) != default)
        {
            onTriggerEnter?.Invoke(transform, other);
        }
    }

    /// <summary>
    /// 触れ続けている間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (targetTagNames.Find(x => x == other.tag) != default)
        {
            onTriggerUpdate?.Invoke(transform, other);
        }
    }

    /// <summary>
    /// 離れた瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (targetTagNames.Find(x => x == other.tag) != default)
        {
            onTriggerExit?.Invoke(transform, other);
        }
    }

    /// <summary>
    /// 衝突した瞬間
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (targetTagNames.Find(x => x == collision.transform.tag) != default)
        {
            onCollisionEnter?.Invoke(transform, collision);
        }
    }

    /// <summary>
    /// 衝突し続けている間
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        if (targetTagNames.Find(x => x == collision.transform.tag) != default)
        {
            onCollisionUpdate?.Invoke(transform, collision);
        }
    }

    /// <summary>
    /// 離れた瞬間
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionExit(Collision collision)
    {
        if (targetTagNames.Find(x => x == collision.transform.tag) != default)
        {
            onCollisionExit?.Invoke(transform, collision);
        }
    }
}
