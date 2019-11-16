using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 警戒状態時のイベント群
/// </summary>
public class VigilanceEvent : MonoBehaviour
{
    // 警戒状態の開始時
    [SerializeField]
    UnityEvent onVigilanceEnter = default;

    // 警戒状態の更新時
    [SerializeField]
    UnityEvent onVigilanceUpdate = default;

    // 警戒状態の終了時
    [SerializeField]
    UnityEvent onVigilanceExit = default;

    /// <summary>
    /// 開始時のコールバックを呼ぶ
    /// </summary>
    public void OnVigilanceEnter()
    {
        onVigilanceEnter.Invoke();
    }

    /// <summary>
    /// 更新時のコールバックを呼ぶ
    /// </summary>
    public void OnVigilanceUpdate()
    {
        onVigilanceUpdate.Invoke();
    }

    /// <summary>
    /// 終了時のコールバックを呼ぶ
    /// </summary>
    public void OnVigilanceExit()
    {
        onVigilanceExit.Invoke();
    }
}
