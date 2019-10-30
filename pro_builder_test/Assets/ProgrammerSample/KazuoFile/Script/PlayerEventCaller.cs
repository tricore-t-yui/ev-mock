using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーのイベント呼び出しクラス
/// </summary>
public class PlayerEventCaller : MonoBehaviour
{
    /// <summary>
    /// イベントのタイプ
    /// </summary>
    public enum EventType
    {
        MOVE,       // 移動時
        WALK,       // 歩く時
        DASH,       // ダッシュ時
        SQUAT,      // しゃがみ時
        STEALTH,    // 忍び歩き時
        DOOR,       // ドア時
        HIDE,       // 隠れる時
        DEEPBREATH, // 深呼吸時

        MOVEEND,        // 移動終了時
        WALKEND,        // 歩く終了時
        DASHEND,        // ダッシュ終了時
        SQUATEND,       // しゃがみ終了時
        STEALTHEND,     // 忍び歩き終了時
        DOOREND,        // ドア終了時
        HIDEEND,        // 隠れる終了時
        DEEPBREATHEND,  // 深呼吸終了時
    }

    [SerializeField]
    UnityEvent move = new UnityEvent();         // 移動イベント
    [SerializeField]
    UnityEvent walk = new UnityEvent();         // 歩きイベント
    [SerializeField]
    UnityEvent dash = new UnityEvent();         // ダッシュイベント
    [SerializeField]
    UnityEvent squat = new UnityEvent();        // しゃがみイベント
    [SerializeField]
    UnityEvent stealth = new UnityEvent();      // 忍び歩きイベント
    [SerializeField]
    UnityEvent doorOpen = new UnityEvent();     // ドア開閉イベント
    [SerializeField]
    UnityEvent hide = new UnityEvent();         // 隠れるイベント
    [SerializeField]
    UnityEvent deepBreath = new UnityEvent();   // 深呼吸イベント

    [SerializeField]
    UnityEvent moveEnd = new UnityEvent();      // 移動終了イベント
    [SerializeField]
    UnityEvent walkEnd = new UnityEvent();      // 歩き終了イベント
    [SerializeField]
    UnityEvent dashEnd = new UnityEvent();      // ダッシュ終了イベント
    [SerializeField]
    UnityEvent squatEnd = new UnityEvent();     // しゃがみ終了イベント
    [SerializeField]
    UnityEvent stealthEnd = new UnityEvent();   // 忍び歩き終了イベント
    [SerializeField]
    UnityEvent doorOpenEnd = new UnityEvent();  // ドア開閉終了イベント
    [SerializeField]
    UnityEvent hideEnd = new UnityEvent();      // 隠れる終了イベント
    [SerializeField]
    UnityEvent deepBreathEnd = new UnityEvent();// 深呼吸イベント

    /// <summary>
    /// 各タイプのイベント呼び出し
    /// </summary>
    /// <param name="type">イベントのタイプ</param>
    public void Invoke(EventType type)
    {
        switch(type)
        {
            case EventType.MOVE:       move.Invoke();       break;
            case EventType.WALK:       walk.Invoke();       break;
            case EventType.DASH:       dash.Invoke();       break;
            case EventType.SQUAT:      squat.Invoke();      break;
            case EventType.STEALTH:    stealth.Invoke();    break;
            case EventType.DOOR:       doorOpen.Invoke();   break;
            case EventType.HIDE:       hide.Invoke();       break;
            case EventType.DEEPBREATH: deepBreath.Invoke(); break;

            case EventType.MOVEEND:       moveEnd.Invoke();       break;
            case EventType.WALKEND:       walkEnd.Invoke();       break;
            case EventType.DASHEND:       dashEnd.Invoke();       break;
            case EventType.SQUATEND:      squatEnd.Invoke();      break;
            case EventType.STEALTHEND:    stealthEnd.Invoke();    break;
            case EventType.DOOREND:       doorOpenEnd.Invoke();   break;
            case EventType.HIDEEND:       hideEnd.Invoke();       break;
            case EventType.DEEPBREATHEND: deepBreathEnd.Invoke(); break;
        }
    }
}
