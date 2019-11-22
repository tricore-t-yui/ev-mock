using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態フラグを監視
/// </summary>
public class PlayerStateFlagWatcher : MonoBehaviour
{
    // 隠れているか
    [SerializeField]
    bool isHide = false;
    public bool IsHide { get { return isHide; } }
    // 息を止めているか
    [SerializeField]
    bool isBreathHold = false;
    public bool IsBreathHold { get { return isBreathHold; } }


    /// <summary>
    /// 隠れ開始
    /// </summary>
    public void OnHideStart()
    {
        isHide = true;
    }

    /// <summary>
    /// 隠れ終了
    /// </summary>
    public void OnHideEnd()
    {
        isHide = false;
    }

    /// <summary>
    /// 息止め開始
    /// </summary>
    public void OnBreathHoldStart()
    {
        isBreathHold = true;
    }

    /// <summary>
    /// 息止め終了
    /// </summary>
    public void OnBreathHoldEnd()
    {
        isBreathHold = false;
    }
}
