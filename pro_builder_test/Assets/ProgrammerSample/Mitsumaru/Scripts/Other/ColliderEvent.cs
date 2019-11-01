using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallbackUnityEvent : UnityEvent<Transform,Transform> { }

/// <summary>
/// コライダーとの衝突でUnityEventを呼ぶ汎用クラス
/// </summary>
[RequireComponent(typeof(Collider))]
public class ColliderEvent : MonoBehaviour
{
    // 衝突したとき
    CallbackUnityEvent onTriggerEnter = new CallbackUnityEvent();

    // 衝突し続けている
    CallbackUnityEvent onTriggerStay = new CallbackUnityEvent();

    // 離れたとき
    CallbackUnityEvent onTriggerExit = new CallbackUnityEvent();

    /// <summary>
    /// Enter時のコールバック追加
    /// </summary>
    /// <param name="call"></param>
    public void AddEnterListener(UnityAction<Transform, Transform> call)
    {
        onTriggerEnter.AddListener(call);
    }

    /// <summary>
    /// Stay時のコールバック追加
    /// </summary>
    /// <param name="call"></param>
    public void AddStayListener(UnityAction<Transform, Transform> call)
    {
        onTriggerStay.AddListener(call);
    }

    /// <summary>
    /// Exit時のコールバック追加
    /// </summary>
    /// <param name="call"></param>
    public void AddExitListener(UnityAction<Transform, Transform> call)
    {
        onTriggerExit.AddListener(call);
    }


    /// <summary>
    /// 衝突した瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(transform, other.transform);
    }

    /// <summary>
    /// 衝突し続けている
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(transform, other.transform);
    }

    /// <summary>
    /// 離れた瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(transform, other.transform);
    }
}
