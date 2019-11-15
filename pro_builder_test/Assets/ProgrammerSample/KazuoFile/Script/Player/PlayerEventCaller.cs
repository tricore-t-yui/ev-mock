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
        STEALTH,            // 忍び歩き
        DOOR,               // ドア
        HIDE,               // 隠れる
        DEEPBREATH,         // 深呼吸
        BREATHLESSNESS,     // 息切れ
        DAMAGE,             // ダメージ
        BAREFOOT,           // 裸足

        WAITEND,            // 待機終了
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
    UnityEvent stealth          = new UnityEvent();     // 忍び歩きイベント
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
    UnityEvent waitEnd          = new UnityEvent();     // 待機終了イベント
    [SerializeField]
    UnityEvent walkEnd          = new UnityEvent();     // 歩き終了イベント
    [SerializeField]
    UnityEvent dashEnd          = new UnityEvent();     // ダッシュ終了イベント
    [SerializeField]
    UnityEvent squatEnd         = new UnityEvent();     // しゃがみ終了イベント
    [SerializeField]
    UnityEvent stealthEnd       = new UnityEvent();     // 忍び歩き終了イベント
    [SerializeField]
    UnityEvent doorOpenEnd      = new UnityEvent();     // ドア開閉終了イベント
    [SerializeField]
    UnityEvent hideEnd          = new UnityEvent();     // 隠れる終了イベント
    [SerializeField]
    UnityEvent deepBreathEnd    = new UnityEvent();     // 深呼吸終了イベント
    [SerializeField]
    UnityEvent brethressnessEnd = new UnityEvent();     // 息切れ終了イベント
    [SerializeField]
    UnityEvent damageEnd        = new UnityEvent();     // ダメージ終了イベント
    [SerializeField]
    UnityEvent barefootEnd      = new UnityEvent();     // 裸足終了イベント

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
            case EventType.STEALTH:           stealth.Invoke();          break;
            case EventType.DOOR:              doorOpen.Invoke();         break;
            case EventType.HIDE:              hide.Invoke();             break;
            case EventType.DEEPBREATH:        deepBreath.Invoke();       break;
            case EventType.BREATHLESSNESS:    brethressness.Invoke();    break;
            case EventType.DAMAGE:            damage.Invoke();           break;
            case EventType.BAREFOOT:          barefoot.Invoke();         break;

            case EventType.WAITEND:           waitEnd.Invoke();          break;
            case EventType.WALKEND:           walkEnd.Invoke();          break;
            case EventType.DASHEND:           dashEnd.Invoke();          break;
            case EventType.SQUATEND:          squatEnd.Invoke();         break;
            case EventType.STEALTHEND:        stealthEnd.Invoke();       break;
            case EventType.DOOREND:           doorOpenEnd.Invoke();      break;
            case EventType.HIDEEND:           hideEnd.Invoke();          break;
            case EventType.DEEPBREATHEND:     deepBreathEnd.Invoke();    break;
            case EventType.BREATHLESSNESSEND: brethressnessEnd.Invoke(); break;
            case EventType.DAMAGEEND:         damageEnd.Invoke();        break;
            case EventType.BAREFOOTEND:       barefootEnd.Invoke();      break;
        }
    }
}
