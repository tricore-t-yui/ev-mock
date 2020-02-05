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
        WAIT,               // 待機
        WALK,               // 歩く
        DASH,               // ダッシュ
        SQUAT,              // しゃがみ
        BREATHHOLD,         // 息止め
        BREATHHOLDMOVE,     // 息止め歩き
        DOOR,               // ドア
        HIDE,               // 隠れる
        DEEPBREATH,         // 深呼吸
        BREATHLESSNESS,     // 息切れ
        DAMAGE,             // ダメージ
        BAREFOOT,           // 裸足
        GETDOLL,            // 人形ゲット
        TRAP,               // 罠
    }

    [SerializeField]
    UnityEvent wait             = new UnityEvent();     // 待機イベント
    [SerializeField]
    UnityEvent walk             = new UnityEvent();     // 歩きイベント
    [SerializeField]
    UnityEvent dash             = new UnityEvent();     // ダッシュイベント
    [SerializeField]
    UnityEvent squat            = new UnityEvent();     // しゃがみイベント
    [SerializeField]
    UnityEvent breathHold       = new UnityEvent();     // 息止めイベント
    [SerializeField]
    UnityEvent breathHoldMove  = new UnityEvent();      // 息止め歩きイベント
    [SerializeField]
    UnityEvent doorOpen         = new UnityEvent();     // ドア開閉イベント
    [SerializeField]
    UnityEvent hide             = new UnityEvent();     // 隠れるイベント
    [SerializeField]
    UnityEvent deepBreath       = new UnityEvent();     // 深呼吸イベント
    [SerializeField]
    UnityEvent brethressness    = new UnityEvent();     // 息切れイベント
    [SerializeField]
    UnityEvent damage           = new UnityEvent();     // ダメージイベント
    [SerializeField]
    UnityEvent barefoot         = new UnityEvent();     // 裸足イベント
    [SerializeField]
    UnityEvent getDoll          = new UnityEvent();     // 人形ゲットイベント
    [SerializeField]
    UnityEvent trap             = new UnityEvent();     // 罠イベント

    /// <summary>
    /// 各タイプのイベント呼び出し
    /// </summary>
    /// <param name="type">イベントのタイプ</param>
    public void Invoke(EventType type)
    {
        switch(type)
        {
            case EventType.WAIT:              wait.Invoke();             break;
            case EventType.WALK:              walk.Invoke();             break;
            case EventType.DASH:              dash.Invoke();             break;
            case EventType.SQUAT:             squat.Invoke();            break;
            case EventType.BREATHHOLD:        breathHold.Invoke();       break;
            case EventType.BREATHHOLDMOVE:    breathHoldMove.Invoke();   break;
            case EventType.DOOR:              doorOpen.Invoke();         break;
            case EventType.HIDE:              hide.Invoke();             break;
            case EventType.DEEPBREATH:        deepBreath.Invoke();       break;
            case EventType.BREATHLESSNESS:    brethressness.Invoke();    break;
            case EventType.DAMAGE:            damage.Invoke();           break;
            case EventType.BAREFOOT:          barefoot.Invoke();         break;
            case EventType.GETDOLL:           getDoll.Invoke();          break;
            case EventType.TRAP:              trap.Invoke();             break;
        }
    }
}
