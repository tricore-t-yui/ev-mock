using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEvent : UnityEvent<Transform,Collider> { }

/// <summary>
/// コライダーとの衝突でUnityEventを呼ぶ汎用クラス
/// </summary>
[RequireComponent(typeof(Collider))]
public class ColliderTriggerEvent : MonoBehaviour
{
    // 衝突したとき
    ColliderEvent onTriggerEnter = new ColliderEvent();

    // 衝突し続けている
    ColliderEvent onTriggerStay = new ColliderEvent();

    // 離れたとき
    ColliderEvent onTriggerExit = new ColliderEvent();

    /// <summary>
    /// Enter時のコールバック追加
    /// </summary>
    /// <param name="call"></param>
    public void AddEnterListener(UnityAction<Transform, Collider> call)
    {
        onTriggerEnter.AddListener(call);
    }

    /// <summary>
    /// Stay時のコールバック追加
    /// </summary>
    /// <param name="call"></param>
    public void AddStayListener(UnityAction<Transform, Collider> call)
    {
        onTriggerStay.AddListener(call);
    }

    /// <summary>
    /// Exit時のコールバック追加
    /// </summary>
    /// <param name="call"></param>
    public void AddExitListener(UnityAction<Transform, Collider> call)
    {
        onTriggerExit.AddListener(call);
    }


    /// <summary>
    /// 衝突した瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(transform, other);
    }

    /// <summary>
    /// 衝突し続けている
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(transform, other);
    }

    /// <summary>
    /// 離れた瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(transform, other);
    }
}
