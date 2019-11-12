using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimType = PlayerAnimationContoller.AnimationType;

public class PlayerStateController : MonoBehaviour
{
    /// <summary>
    /// ステートタイプ
    /// </summary>
    public enum StateType
    {
        WAIT,           // 待機
        WALK,           // 歩き
        DASH,           // ダッシュ
        STEALTH,        // 忍び歩き
        DOOROPEN,       // ドア開閉
        HIDE,           // 隠れる
        DEEPBREATH,     // 深呼吸
        BREATHLESSNESS, // 息切れ
        DAMAGE,         // ダメージ
    }

    GameObject rayObject = default;                     // レイに当たったオブジェクト

    [SerializeField]
    Transform player = default;                         // プレイヤー
    [SerializeField]
    CapsuleCollider collider = default;                 // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;    // 息管理クラス
    [SerializeField]
    PlayerDoorController doorController = default;      // ドアアクションクラス
    [SerializeField]
    PlayerHideController hideController = default;      // 隠れるアクションクラス
    [SerializeField]
    PlayerDamageController damageController = default;  // ダメージリアクションクラス
    [SerializeField]
    PlayerEventCaller eventCaller = default;            // イベント呼び出しクラス
    [SerializeField]
    SoundAreaController soundArea = default;            // 音管理クラス

    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;                // ダッシュキー
    [SerializeField]
    KeyCode squatKey = KeyCode.LeftCommand;             // しゃがみキー
    [SerializeField]
    KeyCode stealthKey = KeyCode.LeftControl;           // 忍び足キー
    [SerializeField]
    KeyCode deepBreathKey = KeyCode.Space;              // 深呼吸キー

    StateType state = StateType.WAIT;                   // 現在の状態
    bool isDashOpen = false;                            // ダッシュで開けたかどうか

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 各イベント処理再生
        EventPlay();

        if(Input.GetKey(KeyCode.H))
        {
            ChangeDamageState(Vector3.zero, 50);
        }
    }

    /// <summary>
    /// しゃがみ検知
    /// </summary>
    /// NOTE:k.oishi しゃがみはステートを持っていないので検知と同時に処理
    void CheckSquatState()
    {
        if (Input.GetKey(squatKey))
        {
            soundArea.ChangeSoundLevel(-2);
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUAT);
        }
        else
        {
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUATEND);
        }
    }

    /// <summary>
    /// 歩き検知
    /// </summary>
    void CheckWaitState()
    {
        if (!GetDirectionKey() && !Input.GetMouseButton(0) && !Input.GetKey(stealthKey) && !Input.GetKey(squatKey))
        {
            EventStop();
            state = StateType.WAIT;
        }
    }

    /// <summary>
    /// 歩き検知
    /// </summary>
    void CheckWalkState()
    {
        if (GetDirectionKey() && !Input.GetKey(dashKey) && !Input.GetKey(stealthKey) && state != StateType.DEEPBREATH && state != StateType.BREATHLESSNESS)
        {
            EventStop();
            state = StateType.WALK;
        }
    }

    /// <summary>
    /// ダッシュ検知
    /// </summary>
    void CheckDashState()
    {
        // 方向キーが押されている時
        if (GetDirectionKey() && Input.GetKey(dashKey) && state != StateType.DEEPBREATH && state != StateType.BREATHLESSNESS)
        {
            EventStop();
            state = StateType.DASH;
        }
    }

    /// <summary>
    /// 忍び歩き検知
    /// </summary>
    void CheckStealthState()
    {
        if (!Input.GetKey(dashKey) && Input.GetKey(stealthKey) && state != StateType.DEEPBREATH && state != StateType.BREATHLESSNESS)
        {
            EventStop();
            state = StateType.STEALTH;
        }
    }

    /// <summary>
    /// 深呼吸検知
    /// </summary>
    void CheckDeepBreathState()
    {
        if (!GetDirectionKey() && Input.GetKey(deepBreathKey) && breathController.NowAmount < 100)
        {
            EventStop();
            state = StateType.DEEPBREATH;
        }
    }

    /// <summary>
    /// 息切れ検知
    /// </summary>
    void CheckBrethlessnessState()
    {
        if (breathController.IsBreathlessness)
        {
            EventStop();
            state = StateType.BREATHLESSNESS;
        }
    }

    /// <summary>
    /// 息回復終了検知
    /// </summary>
    void CheckEndBrethlessnessRecovery()
    {
        if (breathController.NowAmount >= 100)
        {
            EventStop();
            if (!breathController.IsBreathlessness)
            {
                state = StateType.WAIT;
            }
        }
    }

    /// <summary>
    /// 隠れる検知
    /// </summary>
    void CheckHideState()
    {
        if (Input.GetMouseButton(0) && (ObjectLayer() == LayerMask.NameToLayer("Locker") || ObjectLayer() == LayerMask.NameToLayer("Bed")))
        {
            EventStop();
            hideController.SetInfo(rayObject);
            state = StateType.HIDE;
        }
    }

    /// <summary>
    /// ドア開閉検知
    /// </summary>
    void CheckDoorOpenState()
    {
        if (Input.GetMouseButton(0) && ObjectLayer() == LayerMask.NameToLayer("Door"))
        {
            EventStop();
            if (state == StateType.DASH)
            {
                doorController.SetInfo(rayObject, PlayerDoorController.OpenType.DASH);
                isDashOpen = true;
            }
            else
            {
                doorController.SetInfo(rayObject, PlayerDoorController.OpenType.NORMAL);
                isDashOpen = false;
            }

            state = StateType.DOOROPEN;
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// NOTE:k.oishi この関数を敵の攻撃のUnityEventに入れてください
    public void ChangeDamageState(Vector3 enemyPos, float damage)
    {
        // ダメージ処理が開始されていないならダメージを食らう
        if (!damageController.enabled)
        {
            EventStop();
            damageController.SetInfo(enemyPos, damage);
            state = StateType.DAMAGE;
        }
    }
    
    /// <summary>
    /// イベント再生
    /// </summary>
    void EventPlay()
    {
        switch (state)
        {
            case StateType.WAIT:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.WAIT);
                soundArea.ChangeSoundLevel(1);

                // 各処理の検知
                CheckSquatState();
                CheckWalkState();
                CheckDashState();
                CheckStealthState();
                CheckDeepBreathState();
                CheckDoorOpenState();
                CheckHideState();
                break;
            case StateType.WALK:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.WALK);
                soundArea.ChangeSoundLevel(5);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckDashState();
                CheckStealthState();
                CheckDoorOpenState();
                CheckHideState();
                break;
            case StateType.DASH:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DASH);
                soundArea.ChangeSoundLevel(7);

                // 各処理の検知
                CheckWaitState();
                CheckWalkState();
                CheckStealthState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                break;
            case StateType.STEALTH:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.STEALTH);
                if(GetDirectionKey())
                {
                    soundArea.ChangeSoundLevel(3);
                }
                else
                {
                    soundArea.ChangeSoundLevel(-3);
                }

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckWalkState();
                CheckDashState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                break;
            case StateType.DOOROPEN:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DOOR);
                if(isDashOpen)
                {
                    soundArea.ChangeSoundLevel(3);
                }
                else
                {
                    soundArea.ChangeSoundLevel(0);
                }

                // ドアアクションクラスが停止しているなら終了し、各処理の検知
                if (!doorController.enabled && state == StateType.DOOROPEN)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckStealthState();
                    CheckDeepBreathState();
                }
                break;
            case StateType.HIDE:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.HIDE);
                soundArea.ChangeSoundLevel(-2);

                // 隠れているオブジェクトがロッカーならしゃがみ検知
                if (LayerMask.LayerToName(hideController.HideObj.layer) == "Locker")
                {
                    CheckSquatState();
                }

                // 隠れるアクションクラスが停止しているなら終了し、各処理の検知
                if (!hideController.enabled && state == StateType.HIDE)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckStealthState();
                    CheckDeepBreathState();
                }
                break;
            case StateType.DEEPBREATH:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DEEPBREATH);
                soundArea.ChangeSoundLevel(3);

                // 各処理の検知
                CheckWaitState();
                CheckWalkState();
                CheckDashState();
                CheckStealthState();
                CheckDeepBreathState();
                CheckDoorOpenState();
                CheckHideState();

                // 息回復終了検知
                CheckEndBrethlessnessRecovery();
                break;
            case StateType.BREATHLESSNESS:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESS);

                // 息回復終了検知
                CheckEndBrethlessnessRecovery();
                break;
            case StateType.DAMAGE:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DAMAGE);

                // ダメージリアクションクラスが停止しているなら終了し、各処理の検知
                if (!damageController.enabled && state == StateType.DAMAGE)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckStealthState();
                    CheckDeepBreathState();
                }
                break;
        }
    }

    /// <summary>
    /// 各イベント終了処理
    /// </summary>
    void EventStop()
    {
        switch (state)
        {
            case StateType.WAIT: eventCaller.Invoke(PlayerEventCaller.EventType.WAITEND); break;
            case StateType.WALK: eventCaller.Invoke(PlayerEventCaller.EventType.WALKEND); break;
            case StateType.DASH: eventCaller.Invoke(PlayerEventCaller.EventType.DASHEND); break;
            case StateType.STEALTH: eventCaller.Invoke(PlayerEventCaller.EventType.STEALTHEND); break;
            case StateType.DOOROPEN: eventCaller.Invoke(PlayerEventCaller.EventType.DOOREND); break;
            case StateType.HIDE: eventCaller.Invoke(PlayerEventCaller.EventType.HIDE); break;
            case StateType.DEEPBREATH: eventCaller.Invoke(PlayerEventCaller.EventType.DEEPBREATHEND); break;
            case StateType.BREATHLESSNESS: eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESSEND); break;
            case StateType.DAMAGE: eventCaller.Invoke(PlayerEventCaller.EventType.DAMAGE); break;
        }
    }

    /// <summary>
    /// 方向キー検知
    /// </summary>
    /// <returns>方向キーのどれか１つが押されたかどうか</returns>
    bool GetDirectionKey()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移動方向のレイに当たったオブジェクトのレイヤーを返す
    /// </summary>
    /// <returns>当たったオブジェクトのレイヤー</returns>
    int ObjectLayer()
    {
        // レイのスタート位置
        Vector3 start = player.position;

        // レイの向き
        Vector3 dir = player.forward;

        // レイの距離
        float distance = collider.radius * 3.5f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = (1 << LayerMask.GetMask(new string[] { "Player","Stage" }));
        layerMask = ~layerMask;

        // レイ作成
        Ray ray = new Ray(start, dir);
        RaycastHit hit = default;

        // デバック用ライン
        Debug.DrawLine(start, start + (dir * distance), Color.red);

        // レイに当たったらオブジェクトのレイヤー、外れていたら-1を返す
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            rayObject = hit.collider.gameObject;
            return hit.collider.gameObject.layer;
        }
        else
        {
            return -1;
        }
    }
}
