using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 開始イベント呼び出しクラス
/// </summary>
public class PlayerStartEventCaller : MonoBehaviour
{
    /// <summary>
    /// イベントのタイプ
    /// </summary>
    public enum EventType
    {
        WAITSTART,              // 待機
        WALKSTART,              // 歩く
        DASHSTART,              // ダッシュ
        SQUATSTART,             // しゃがみ
        BREATHHOLDSTART,        // 息止め
        BREATHHOLDSMOVETART,    // 息止め移動
        DOORSTART,              // ドア
        HIDESTART,              // 隠れる
        DEEPBREATHSTART,        // 深呼吸
        BREATHLESSNESSSTART,    // 息切れ
        DAMAGESTART,            // ダメージ
        BAREFOOTSTART,          // 裸足
        GETDOLLSTART,           // 人形ゲット
        TRAPSTART,              // 罠
    }

    [SerializeField]
    UnityEvent waitStart = new UnityEvent();            // 待機イベント
    [SerializeField]
    UnityEvent walkStart = new UnityEvent();            // 歩きイベント
    [SerializeField]
    UnityEvent dashStart = new UnityEvent();            // ダッシュイベント
    [SerializeField]
    UnityEvent squatStart = new UnityEvent();           // しゃがみイベント
    [SerializeField]
    UnityEvent breathHoldStart = new UnityEvent();      // 息止めイベント
    [SerializeField]
    UnityEvent breathHoldMoveStart = new UnityEvent();  // 息止め移動イベント
    [SerializeField]
    UnityEvent doorOpenStart = new UnityEvent();        // ドア開閉イベント
    [SerializeField]
    UnityEvent hideStart = new UnityEvent();            // 隠れるイベント
    [SerializeField]
    UnityEvent deepBreathStart = new UnityEvent();      // 深呼吸イベント
    [SerializeField]
    UnityEvent brethressnessStart = new UnityEvent();   // 息切れイベント
    [SerializeField]
    UnityEvent damageStart = new UnityEvent();          // ダメージイベント
    [SerializeField]
    UnityEvent barefootStart = new UnityEvent();        // 裸足イベント
    [SerializeField]
    UnityEvent getDollStart = new UnityEvent();         // 人形ゲットイベント
    [SerializeField]
    UnityEvent trapStart = new UnityEvent();            // 罠イベント

    /// <summary>
    /// 各タイプのイベント呼び出し
    /// </summary>
    /// <param name="type">イベントのタイプ</param>
    public void Invoke(EventType type)
    {
        switch (type)
        {
            case EventType.WAITSTART: waitStart.Invoke(); break;
            case EventType.WALKSTART: walkStart.Invoke(); break;
            case EventType.DASHSTART: dashStart.Invoke(); break;
            case EventType.SQUATSTART: squatStart.Invoke(); break;
            case EventType.BREATHHOLDSTART: breathHoldStart.Invoke(); break;
            case EventType.BREATHHOLDSMOVETART: breathHoldMoveStart.Invoke(); break;
            case EventType.DOORSTART: doorOpenStart.Invoke(); break;
            case EventType.HIDESTART: hideStart.Invoke(); break;
            case EventType.DEEPBREATHSTART: deepBreathStart.Invoke(); break;
            case EventType.BREATHLESSNESSSTART: brethressnessStart.Invoke(); break;
            case EventType.DAMAGESTART: damageStart.Invoke(); break;
            case EventType.BAREFOOTSTART: barefootStart.Invoke(); break;
            case EventType.GETDOLLSTART: getDollStart.Invoke(); break;
            case EventType.TRAPSTART: trapStart.Invoke(); break;
        }
    }
}
