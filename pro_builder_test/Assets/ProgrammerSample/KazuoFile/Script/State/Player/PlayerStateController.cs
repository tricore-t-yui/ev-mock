using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimType = PlayerAnimationContoller.AnimationType;

public class PlayerStateController : MonoBehaviour
{
    /// <summary>
    /// ステートタイプ
    /// </summary>
    public enum ActionStateType
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
        SHOES,          // 靴
    }

    GameObject rayObject = default;                         // レイに当たったオブジェクト

    [SerializeField]
    Transform player = default;                             // プレイヤー
    [SerializeField]
    CapsuleCollider collider = default;                     // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;      // 息管理クラス
    [SerializeField]
    PlayerDoorController doorController = default;          // ドアアクションクラス
    [SerializeField]
    PlayerHideController hideController = default;          // 隠れるアクションクラス
    [SerializeField]
    PlayerDamageController damageController = default;      // ダメージリアクションクラス
    [SerializeField]
    PlayerEventCaller eventCaller = default;                // イベント呼び出しクラス
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;  // アニメーション管理クラス

    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;                    // ダッシュキー
    [SerializeField]
    KeyCode squatKey = KeyCode.LeftCommand;                 // しゃがみキー
    [SerializeField]
    KeyCode stealthKey = KeyCode.LeftControl;               // 忍び足キー
    [SerializeField]
    KeyCode deepBreathKey = KeyCode.Space;                  // 深呼吸キー
    [SerializeField]
    KeyCode shoeshKey = KeyCode.V;                          // 靴着脱キー

    public bool IsDashOpen { get; private set; } = false;   // ダッシュで開けたかどうか
    public bool IsShoes { get; private set; } = true;       // 靴を表示するかどうか
    public bool IsSquat { get; private set; } = false;      // しゃがんでいるかどうか
    public ActionStateType State { get; private set; } = ActionStateType.WAIT;  // 現在の状態

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 各ニメーションがちゃんと終わっているなら各イベント処理再生
        if (animationContoller.IsEndAnim)
        {
            EventPlay();
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
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUAT);
            IsSquat = true;
        }
        else
        {
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUATEND);
            IsSquat = false;
        }
    }

    /// <summary>
    /// 靴着脱検知
    /// </summary>
    /// NOTE:k.oishi 靴着脱はステートを持っていないので検知と同時に処理
    void CheckShooesState()
    {
        if (Input.GetKey(shoeshKey))
        {
            eventCaller.Invoke(PlayerEventCaller.EventType.BAREFOOT);
            IsShoes = false;
        }
        else
        {
            eventCaller.Invoke(PlayerEventCaller.EventType.BAREFOOTEND);
            IsShoes = true;
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
            State = ActionStateType.WAIT;
        }
    }

    /// <summary>
    /// 歩き検知
    /// </summary>
    void CheckWalkState()
    {
        if (GetDirectionKey() && !Input.GetKey(dashKey) && !Input.GetKey(stealthKey) && State != ActionStateType.DEEPBREATH && State != ActionStateType.BREATHLESSNESS)
        {
            EventStop();
            State = ActionStateType.WALK;
        }
    }

    /// <summary>
    /// ダッシュ検知
    /// </summary>
    void CheckDashState()
    {
        // 方向キーが押されている時
        if (GetDirectionKey() && Input.GetKey(dashKey) && State != ActionStateType.DEEPBREATH && State != ActionStateType.BREATHLESSNESS)
        {
            EventStop();
            State = ActionStateType.DASH;
        }
    }

    /// <summary>
    /// 忍び歩き検知
    /// </summary>
    void CheckStealthState()
    {
        if (!Input.GetKey(dashKey) && Input.GetKey(stealthKey) && State != ActionStateType.DEEPBREATH && State != ActionStateType.BREATHLESSNESS)
        {
            EventStop();
            State = ActionStateType.STEALTH;
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
            State = ActionStateType.DEEPBREATH;
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
            State = ActionStateType.BREATHLESSNESS;
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
                State = ActionStateType.WAIT;
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
            State = ActionStateType.HIDE;
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
            if (State == ActionStateType.DASH)
            {
                doorController.SetInfo(rayObject, PlayerDoorController.OpenType.DASH);
                IsDashOpen = true;
            }
            else
            {
                doorController.SetInfo(rayObject, PlayerDoorController.OpenType.NORMAL);
                IsDashOpen = false;
            }

            State = ActionStateType.DOOROPEN;
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
            State = ActionStateType.DAMAGE;
        }
    }
    
    /// <summary>
    /// イベント再生
    /// </summary>
    void EventPlay()
    {
        switch (State)
        {
            case ActionStateType.WAIT:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.WAIT);

                // 各処理の検知
                CheckSquatState();
                CheckWalkState();
                CheckDashState();
                CheckStealthState();
                CheckDeepBreathState();
                CheckDoorOpenState();
                CheckHideState();
                CheckShooesState();
                break;
            case ActionStateType.WALK:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.WALK);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckDashState();
                CheckStealthState();
                CheckDoorOpenState();
                CheckHideState();
                CheckShooesState();
                break;
            case ActionStateType.DASH:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DASH);

                // 各処理の検知
                CheckWaitState();
                CheckWalkState();
                CheckStealthState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                break;
            case ActionStateType.STEALTH:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.STEALTH);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckWalkState();
                CheckDashState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                CheckShooesState();
                break;
            case ActionStateType.DOOROPEN:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DOOR);

                // ドアアクションクラスが停止しているなら終了し、各処理の検知
                if (!doorController.enabled && State == ActionStateType.DOOROPEN)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckStealthState();
                    CheckDeepBreathState();
                }
                break;
            case ActionStateType.HIDE:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.HIDE);

                // 隠れているオブジェクトがロッカーならしゃがみ検知
                if (LayerMask.LayerToName(hideController.HideObj.layer) == "Locker")
                {
                    CheckSquatState();
                }
                CheckShooesState();

                // 隠れるアクションクラスが停止しているなら終了し、各処理の検知
                if (!hideController.enabled && State == ActionStateType.HIDE)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckStealthState();
                    CheckDeepBreathState();
                }
                break;
            case ActionStateType.DEEPBREATH:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DEEPBREATH);

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
            case ActionStateType.BREATHLESSNESS:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESS);

                // 息回復終了検知
                CheckEndBrethlessnessRecovery();
                break;
            case ActionStateType.DAMAGE:
                // 各イベント処理
                eventCaller.Invoke(PlayerEventCaller.EventType.DAMAGE);

                // ダメージリアクションクラスが停止しているなら終了し、各処理の検知
                if (!damageController.enabled && State == ActionStateType.DAMAGE)
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
        switch (State)
        {
            case ActionStateType.WAIT: eventCaller.Invoke(PlayerEventCaller.EventType.WAITEND); break;
            case ActionStateType.WALK: eventCaller.Invoke(PlayerEventCaller.EventType.WALKEND); break;
            case ActionStateType.DASH: eventCaller.Invoke(PlayerEventCaller.EventType.DASHEND); break;
            case ActionStateType.STEALTH: eventCaller.Invoke(PlayerEventCaller.EventType.STEALTHEND); break;
            case ActionStateType.DOOROPEN: eventCaller.Invoke(PlayerEventCaller.EventType.DOOREND); break;
            case ActionStateType.HIDE: eventCaller.Invoke(PlayerEventCaller.EventType.HIDE); break;
            case ActionStateType.DEEPBREATH: eventCaller.Invoke(PlayerEventCaller.EventType.DEEPBREATHEND); break;
            case ActionStateType.BREATHLESSNESS: eventCaller.Invoke(PlayerEventCaller.EventType.BREATHLESSNESSEND); break;
            case ActionStateType.DAMAGE: eventCaller.Invoke(PlayerEventCaller.EventType.DAMAGE); break;
        }
    }

    /// <summary>
    /// 方向キー検知
    /// </summary>
    /// <returns>方向キーのどれか１つが押されたかどうか</returns>
    public bool GetDirectionKey()
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
