using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 終了イベント呼び出しクラス
/// </summary>
public class PlayerEndEventCaller : MonoBehaviour
{
    /// <summary>
    /// イベントのタイプ
    /// </summary>
    public enum EventType
    {
        WAITEND,            // 歩く終了
        WALKEND,            // 歩く終了
        DASHEND,            // ダッシュ終了
        SQUATEND,           // しゃがみ終了
        STEALTHEND,         // 忍び歩き終了
        DOOREND,            // ドア終了
        HIDEEND,            // 隠れる終了
        DEEPBREATHEND,      // 深呼吸終了
        BREATHLESSNESSEND,  // 息切れ終了
        DAMAGEEND,          // ダメージ終了
        BAREFOOTEND,        // 裸足終了
        GETDOOLEND,         // 人形ゲット終了
    }

    [SerializeField]
    UnityEvent waitEnd = new UnityEvent();          // 待機終了イベント
    [SerializeField]
    UnityEvent walkEnd = new UnityEvent();          // 歩き終了イベント
    [SerializeField]
    UnityEvent dashEnd = new UnityEvent();          // ダッシュ終了イベント
    [SerializeField]
    UnityEvent squatEnd = new UnityEvent();         // しゃがみ終了イベント
    [SerializeField]
    UnityEvent stealthEnd = new UnityEvent();       // 忍び歩き終了イベント
    [SerializeField]
    UnityEvent doorOpenEnd = new UnityEvent();      // ドア開閉終了イベント
    [SerializeField]
    UnityEvent hideEnd = new UnityEvent();          // 隠れる終了イベント
    [SerializeField]
    UnityEvent deepBreathEnd = new UnityEvent();    // 深呼吸終了イベント
    [SerializeField]
    UnityEvent brethressnessEnd = new UnityEvent(); // 息切れ終了イベント
    [SerializeField]
    UnityEvent damageEnd = new UnityEvent();        // ダメージ終了イベント
    [SerializeField]
    UnityEvent barefootEnd = new UnityEvent();      // 裸足終了イベント
    [SerializeField]
    UnityEvent getdollEnd = new UnityEvent();       // 人形ゲット終了イベント

    /// <summary>
    /// 各タイプのイベント呼び出し
    /// </summary>
    /// <param name="type">イベントのタイプ</param>
    public void Invoke(EventType type)
    {
        switch (type)
        {
            case EventType.WAITEND: waitEnd.Invoke(); break;
            case EventType.WALKEND: walkEnd.Invoke(); break;
            case EventType.DASHEND: dashEnd.Invoke(); break;
            case EventType.SQUATEND: squatEnd.Invoke(); break;
            case EventType.STEALTHEND: stealthEnd.Invoke(); break;
            case EventType.DOOREND: doorOpenEnd.Invoke(); break;
            case EventType.HIDEEND: hideEnd.Invoke(); break;
            case EventType.DEEPBREATHEND: deepBreathEnd.Invoke(); break;
            case EventType.BREATHLESSNESSEND: brethressnessEnd.Invoke(); break;
            case EventType.DAMAGEEND: damageEnd.Invoke(); break;
            case EventType.BAREFOOTEND: barefootEnd.Invoke(); break;
            case EventType.GETDOOLEND: getdollEnd.Invoke(); break;
        }
    }
}
