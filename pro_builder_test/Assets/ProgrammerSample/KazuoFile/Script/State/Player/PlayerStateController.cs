using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimType = PlayerAnimationContoller.AnimationType;
using EventStartType = PlayerStartEventCaller.EventType;
using EventType = PlayerEventCaller.EventType;
using EventEndType = PlayerEndEventCaller.EventType;

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
    CapsuleCollider playerCollider = default;               // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;      // 息管理クラス
    [SerializeField]
    PlayerMoveController moveController = default;          // 座標移動クラス
    [SerializeField]
    PlayerDoorController doorController = default;          // ドアアクションクラス
    [SerializeField]
    PlayerHideController hideController = default;          // 隠れるアクションクラス
    [SerializeField]
    PlayerDamageController damageController = default;      // ダメージリアクションクラス
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;  // アニメーション管理クラス
    [SerializeField]
    PlayerStartEventCaller eventStartCaller = default;      // 開始イベント呼び出しクラス
    [SerializeField]
    PlayerEventCaller eventCaller = default;                // イベント呼び出しクラス
    [SerializeField]
    PlayerEndEventCaller eventEndCaller = default;          // 終了イベント呼び出しクラス
    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;      // ダメージリアクションクラス

    // プレイヤーのダメージイベント（Added by Mitsumaru）
    // note : このクラスから受け取ったダメージのイベント関数を保存しておく
    [SerializeField]
    PlayerDamageEvent playerDamageEvent = default;

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
    public bool IsShoes { get; private set; } = true;       // 靴を履いているかどうか
    public bool IsSquat { get; private set; } = false;      // しゃがんでいるかどうか
    public ActionStateType State { get; private set; } = ActionStateType.WAIT;  // 現在の状態

    private void Start()
    {
        playerDamageEvent.Add(PlayerDamageEvent.DamageType.Normal, ChangeDamageState);
    }

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
            eventCaller.Invoke(EventType.SQUAT);
            IsSquat = true;
        }
        else
        {
            eventEndCaller.Invoke(EventEndType.SQUATEND);
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
            if (IsShoes)
            {
                eventStartCaller.Invoke(EventStartType.BAREFOOTSTART);
                IsShoes = false;
            }
            eventCaller.Invoke(EventType.BAREFOOT);
        }
        else
        {
            if (!IsShoes)
            {
                eventEndCaller.Invoke(EventEndType.BAREFOOTEND);
                IsShoes = true;
            }
        }
    }

    /// <summary>
    /// 歩き検知
    /// </summary>
    void CheckWaitState()
    {
        if (!GetDirectionKey() && !Input.GetMouseButton(0) && !Input.GetKey(stealthKey))
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
        if (GetDirectionKey() && !Input.GetKey(dashKey) && !Input.GetKey(stealthKey) && State != ActionStateType.BREATHLESSNESS)
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
        if (GetDirectionKey() && Input.GetKey(dashKey) && State != ActionStateType.BREATHLESSNESS)
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
        if (!Input.GetKey(dashKey) && Input.GetKey(stealthKey) && State != ActionStateType.BREATHLESSNESS)
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
        if ((!GetDirectionKey() && Input.GetKey(deepBreathKey)) || objectDamageController.IsDeepBreath)
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
    public void ChangeDamageState(Transform enemyPos, float damage)
    {
        // ダメージ処理が開始されていないならダメージを食らう
        if (!damageController.enabled && !damageController.IsInvincible)
        {  
            if (State == ActionStateType.HIDE)
            {
                switch (hideController.type)
                {
                    case PlayerHideController.HideObjectType.BED:
                        damageController.SetInfo(enemyPos, damage, PlayerDamageController.DamageType.HIDEBED);
                        break;
                    case PlayerHideController.HideObjectType.LOCKER:
                        damageController.SetInfo(enemyPos, damage, PlayerDamageController.DamageType.HIDELOCKER);
                        break;
                }
            }
            else
            {
                damageController.SetInfo(enemyPos, damage, PlayerDamageController.DamageType.NORMAL);
            }
            EventStop();
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
                eventCaller.Invoke(EventType.WAIT);

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
                eventCaller.Invoke(EventType.WALK);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckDashState();
                CheckStealthState();
                CheckDoorOpenState();
                CheckHideState();
                CheckShooesState();
                CheckDeepBreathState();
                break;
            case ActionStateType.DASH:
                // 各イベント処理
                eventCaller.Invoke(EventType.DASH);

                // 各処理の検知
                CheckWaitState();
                CheckWalkState();
                CheckStealthState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                CheckDeepBreathState();
                break;
            case ActionStateType.STEALTH:
                // 各イベント処理
                eventCaller.Invoke(EventType.STEALTH);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckWalkState();
                CheckDashState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                CheckShooesState();
                CheckDeepBreathState();
                break;
            case ActionStateType.DOOROPEN:
                // 各イベント処理
                eventCaller.Invoke(EventType.DOOR);

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
                eventCaller.Invoke(EventType.HIDE);

                // 隠れているオブジェクトがロッカーならしゃがみ検知
                if (hideController.type == PlayerHideController.HideObjectType.LOCKER)
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
                eventCaller.Invoke(EventType.DEEPBREATH);

                if (!Input.GetKey(deepBreathKey) && !objectDamageController.IsDeepBreath)
                {
                    // 各処理の検知
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckStealthState();
                    CheckDoorOpenState();
                    CheckHideState();
                }
                break;
            case ActionStateType.BREATHLESSNESS:
                // 各イベント処理
                eventCaller.Invoke(EventType.BREATHLESSNESS);

                // 息回復終了検知
                CheckEndBrethlessnessRecovery();
                break;
            case ActionStateType.DAMAGE:
                // 各イベント処理
                eventCaller.Invoke(EventType.DAMAGE);

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
            case ActionStateType.WALK: eventEndCaller.Invoke(EventEndType.WALKEND); break;
            case ActionStateType.DASH: eventEndCaller.Invoke(EventEndType.DASHEND); break;
            case ActionStateType.STEALTH: eventEndCaller.Invoke(EventEndType.STEALTHEND); break;
            case ActionStateType.DOOROPEN: eventEndCaller.Invoke(EventEndType.DOOREND); break;
            case ActionStateType.HIDE: eventEndCaller.Invoke(EventEndType.HIDEEND); break;
            case ActionStateType.DEEPBREATH: eventEndCaller.Invoke(EventEndType.DEEPBREATHEND); break;
            case ActionStateType.BREATHLESSNESS: eventEndCaller.Invoke(EventEndType.BREATHLESSNESSEND); break;
            case ActionStateType.DAMAGE: eventEndCaller.Invoke(EventEndType.DAMAGEEND); break;
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
        float distance = playerCollider.radius * 3.5f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = (1 << LayerMask.GetMask(new string[] { "Player", "Stage" }));
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