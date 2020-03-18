using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventStartType = PlayerStartEventCaller.EventType;
using EventType = PlayerEventCaller.EventType;
using EventEndType = PlayerEndEventCaller.EventType;
using KeyType = KeyController.KeyType;
using UnityEngine.SceneManagement;

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
        BREATHHOLD,     // 息止め
        BREATHHOLDMOVE, // 息止め歩き
        DOOROPEN,       // ドア開閉
        HIDE,           // 隠れる
        DEEPBREATH,     // 深呼吸
        BREATHLESSNESS, // 息切れ
        DAMAGE,         // ダメージ
        SHOES,          // 靴
        DOLLGET,        // 人形ゲット
        TRAP,           // 罠
    }


    [SerializeField]
    Transform player = default;                             // プレイヤー
    [SerializeField]
    CapsuleCollider playerCollider = default;               // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;      // 息管理クラス
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
    playerStaminaController staminaController = default;    // スタミナクラス
    [SerializeField]
    DollGetController dollGetController = default;          // 人形ゲットクラス
    [SerializeField]
    KeyController keyController = default;                  // キー操作クラス
    [SerializeField]
    PlayerTrapController trapController = default;          // トラップアクションクラス
    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;      // ダメージリアクションクラス
    [SerializeField]
    GameController gameController = default;                // ゲーム管理クラス
    [SerializeField]
    PlayerMoveController moveController = default;                // 移動クラス

    // プレイヤーのダメージイベント（Added by Mitsumaru）
    // note : このクラスから受け取ったダメージのイベント関数を保存しておく
    [SerializeField]
    PlayerDamageEvent playerDamageEvent = default;

    public bool IsBreathHold { get; private set; } = false; 
    public bool IsDashOpen { get; private set; } = false;   // ダッシュで開けたかどうか
    public bool IsShoes { get; private set; } = true;       // 靴を履いているかどうか
    public bool IsSquat { get; private set; } = false;      // しゃがんでいるかどうか
    public string NowAreaName { get; private set; } = "Area01"; // エリアの番号
    public ActionStateType State { get; private set; } = ActionStateType.WAIT;  // 現在の状態

    //  無敵モード変更用変数
    public bool IsInvincible { get; private set; } = false;

    GameObject rayObject = default;                         // レイに当たったオブジェクト
    int rayObjectLayer = default;                           // レイに当たったオブジェクトのレイヤー

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsInvincible = (PlayerPrefs.GetInt("isInvincibleKey") == 1) ? true : false;
        playerDamageEvent.Add(ChangeDamageState);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // レイの情報を更新
        rayObjectLayer = ObjectLayer();

        // 各ニメーションがちゃんと終わっているなら各イベント処理再生
        if (animationContoller.IsEndAnim)
        {
            EventPlay();
        }

        // 無敵モード変更
        Invincible();

        // シーン選択画面に戻る
        if (Input.GetKeyDown(KeyCode.K))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Select");
        }
    }

    /// <summary>
    /// 無敵モード変更
    /// </summary>
    /// NOTE:k.oishi 無敵モードに変更するための関数です
    void Invincible()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!IsInvincible)
            {
                IsInvincible = true;
                PlayerPrefs.SetInt("isInvincibleKey", IsInvincible ? 1 : 0);
            }
            else
            {
                IsInvincible = false;
                PlayerPrefs.SetInt("isInvincibleKey", IsInvincible ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// しゃがみ検知
    /// </summary>
    /// NOTE:k.oishi しゃがみはステートを持っていないので検知と同時に処理
    void CheckSquatState()
    {
        if (keyController.GetKeyDown(KeyType.SQUAT))
        {
            if (!IsSquat)
            {
                IsSquat = true;
            }
            else if (!moveController.IsDuct)
            {
                IsSquat = false;
            }
        }

        if (!IsSquat)
        {
            eventEndCaller.Invoke(EventEndType.SQUATEND);
        }
        else
        {
            eventCaller.Invoke(EventType.SQUAT);
        }
    }

    /// <summary>
    /// 靴着脱検知
    /// </summary>
    /// NOTE:k.oishi 靴着脱はステートを持っていないので検知と同時に処理
    void CheckShooesState()
    {
        if (keyController.GetKey(KeyType.SHOES))
        {
            if (IsShoes)
            {
                eventStartCaller.Invoke(EventStartType.BAREFOOTSTART);
                IsShoes = false;
            }
            else
            {
                eventEndCaller.Invoke(EventEndType.BAREFOOTEND);
                IsShoes = true;
            }
        }

        if (!IsShoes)
        {
            eventCaller.Invoke(EventType.BAREFOOT);
        }
    }

    /// <summary>
    /// 歩き検知
    /// </summary>
    void CheckWaitState()
    {
        if (!keyController.GetKey(KeyType.MOVE) && !keyController.GetKey(KeyType.HOLDBREATH) || (keyController.GetKey(KeyType.LOOKINTO) && State == ActionStateType.WALK))
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
        if (!keyController.GetKey(KeyType.LOOKINTO) && keyController.GetKey(KeyType.MOVE) && (staminaController.IsDisappear || !keyController.GetKey(KeyType.DASH)) && !keyController.GetKey(KeyType.HOLDBREATH) && State != ActionStateType.BREATHLESSNESS)
        {
            EventStop();
            State = ActionStateType.WALK;
            EventStart();
        }
    }

    /// <summary>
    /// ダッシュ検知
    /// </summary>
    void CheckDashState()
    {
        // 方向キーが押されている時
        if (!moveController.IsDuct && keyController.GetKey(KeyType.MOVE) && keyController.GetKey(KeyType.DASH) && State != ActionStateType.BREATHLESSNESS && !staminaController.IsDisappear)
        {
            EventStop();
            State = ActionStateType.DASH;
            EventStart();
        }
    }

    /// <summary>
    /// 息止め検知
    /// </summary>
    void CheckBreathHoldState()
    {
        if (!keyController.GetKey(KeyType.DASH) && keyController.GetKey(KeyType.HOLDBREATH) && State != ActionStateType.BREATHLESSNESS &&
            (State != ActionStateType.BREATHHOLDMOVE || State == ActionStateType.BREATHHOLDMOVE && !keyController.GetKey(KeyType.MOVE)))
        {
            EventStop();
            State = ActionStateType.BREATHHOLD;
            EventStart();
        }
    }

    /// <summary>
    /// 息止め検知
    /// </summary>
    void CheckBreathHoldMoveState()
    {
        if (keyController.GetKey(KeyType.MOVE) && !keyController.GetKey(KeyType.LOOKINTO) && State != ActionStateType.BREATHHOLDMOVE)
        {
            State = ActionStateType.BREATHHOLDMOVE;
            EventStart();
        }
    }

    /// <summary>
    /// 深呼吸検知
    /// </summary>
    void CheckDeepBreathState()
    {
        if ((!keyController.GetKey(KeyType.MOVE) && keyController.GetKey(KeyType.DEEPBREATH)) || objectDamageController.IsDeepBreath)
        {
            EventStop();
            State = ActionStateType.DEEPBREATH;
            EventStart();
        }
    }

    /// <summary>
    /// 息切れ検知
    /// </summary>
    void CheckBrethlessnessState()
    {
        if (breathController.IsDisappear)
        {
            EventStop();
            //State = ActionStateType.BREATHLESSNESS;
            State = ActionStateType.WALK;   // 息切れではなく歩きに
            EventStart();
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
            if (!breathController.IsDisappear)
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
        if (keyController.GetKeyDown(KeyType.INTERACT) && IsCanHide())
        {
            EventStop();
            State = ActionStateType.HIDE;
            EventStart();
            hideController.SetInfo(rayObject);
        }
    }

    /// <summary>
    /// ドア開閉検知
    /// </summary>
    void CheckDoorOpenState()
    {
        if (keyController.GetKeyDown(KeyType.INTERACT) && rayObjectLayer == LayerMask.NameToLayer("Door"))
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
            EventStart();
        }
    }

    /// <summary>
    /// ダメージ検知
    /// </summary>
    /// NOTE:k.oishi この関数を敵の攻撃のUnityEventに入れてください
    public void ChangeDamageState(Transform enemyPos, float damage)
    {
        if (!IsInvincible)
        {
            // ダメージ処理が開始されていないならダメージを食らう
            if (!damageController.enabled && !damageController.IsInvincible)
            {
                if (State == ActionStateType.HIDE)
                {
                    EventStop();
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
                    EventStop();
                    damageController.SetInfo(enemyPos, damage, PlayerDamageController.DamageType.NORMAL);
                }
                State = ActionStateType.DAMAGE;
                EventStart();
            }
        }
    }

    /// <summary>
    /// 罠検知
    /// </summary>
    public void CheckTrapState(Transform trapPos)
    {
        // 罠アクションが行われていないならアクション開始
        if(!trapController.enabled)
        {
            EventStop();
            trapController.SetInfo(trapPos);
            State = ActionStateType.TRAP;
            EventStart();
        }
    }

    /// <summary>
    /// 罠停止
    /// </summary>
    public void CheckEndTrapState()
    {
        EventStop();
        State = ActionStateType.WAIT;
    }

    /// <summary>
    /// 人形ゲット検知
    /// </summary>
    void CheckIDollGet()
    {
        if (keyController.GetKeyDown(KeyType.INTERACT) && rayObjectLayer == LayerMask.NameToLayer("Doll"))
        {
            EventStop();
            dollGetController.SetInfo(rayObject);
            State = ActionStateType.DOLLGET;
            EventStart();
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
                CheckBreathHoldState();
                CheckDeepBreathState();
                CheckDoorOpenState();
                CheckHideState();
                CheckShooesState();
                CheckBrethlessnessState();
                CheckIDollGet();
                break;
            case ActionStateType.WALK:
                // 各イベント処理
                eventCaller.Invoke(EventType.WALK);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckDashState();
                CheckBreathHoldState();
                CheckDoorOpenState();
                CheckHideState();
                CheckShooesState();
                CheckDeepBreathState();
                CheckIDollGet();
                break;
            case ActionStateType.DASH:
                // 各イベント処理
                eventCaller.Invoke(EventType.DASH);

                // 各処理の検知
                CheckWaitState();
                CheckWalkState();
                CheckBreathHoldState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                CheckDeepBreathState();
                CheckIDollGet();
                break;
            case ActionStateType.BREATHHOLD:
                // 各イベント処理
                eventCaller.Invoke(EventType.BREATHHOLD);

                // 各処理の検知
                CheckSquatState();
                CheckWaitState();
                CheckWalkState();
                CheckDashState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                CheckDeepBreathState();
                CheckBreathHoldMoveState();
                CheckIDollGet();
                break;
            case ActionStateType.BREATHHOLDMOVE:
                // 各イベント処理
                eventCaller.Invoke(EventType.BREATHHOLDMOVE);

                // 各処理の検知
                CheckSquatState();
                CheckBreathHoldState();
                CheckWaitState();
                CheckWalkState();
                CheckDashState();
                CheckDoorOpenState();
                CheckHideState();
                CheckBrethlessnessState();
                CheckDeepBreathState();
                CheckIDollGet();
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
                    CheckBreathHoldState();
                    CheckDeepBreathState();
                }
                break;
            case ActionStateType.HIDE:
                // 各イベント処理
                eventCaller.Invoke(EventType.HIDE);

                // 隠れているオブジェクトがロッカーならしゃがみ検知
                if (hideController.type == PlayerHideController.HideObjectType.LOCKER && hideController.IsHideLocker)
                {
                    CheckSquatState();
                }

                // 隠れるアクションクラスが停止しているなら終了し、各処理の検知
                if (!hideController.enabled && State == ActionStateType.HIDE)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckBreathHoldState();
                    CheckDeepBreathState();
                }
                break;
            case ActionStateType.DEEPBREATH:
                // 各イベント処理
                eventCaller.Invoke(EventType.DEEPBREATH);

                if (!keyController.GetKey(KeyType.DEEPBREATH) && !objectDamageController.IsDeepBreath)
                {
                    // 各処理の検知
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckBreathHoldState();
                    CheckDoorOpenState();
                    CheckHideState();
                    CheckIDollGet();
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
                    CheckBreathHoldState();
                    CheckDeepBreathState();
                }
                break;
            case ActionStateType.DOLLGET:
                // 各イベント処理
                eventCaller.Invoke(EventType.GETDOLL);

                // 人形ゲットクラスが停止しているなら終了し、各処理の検知
                if (!dollGetController.enabled && State == ActionStateType.DOLLGET)
                {
                    CheckWaitState();
                }
                break;
            case ActionStateType.TRAP:
                // 各イベント処理
                eventCaller.Invoke(EventType.TRAP);

                // 人形ゲットクラスが停止しているなら終了し、各処理の検知
                if (!trapController.enabled && State == ActionStateType.TRAP)
                {
                    CheckWaitState();
                    CheckWalkState();
                    CheckDashState();
                    CheckBreathHoldState();
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
            case ActionStateType.BREATHHOLD: eventEndCaller.Invoke(EventEndType.BREATHHOLDEND); IsBreathHold = false; break;
            case ActionStateType.BREATHHOLDMOVE: eventEndCaller.Invoke(EventEndType.BREATHHOLDMOVEEND); IsBreathHold = false; break;
            case ActionStateType.DOOROPEN: eventEndCaller.Invoke(EventEndType.DOOREND); break;
            case ActionStateType.HIDE: eventEndCaller.Invoke(EventEndType.HIDEEND); break;
            case ActionStateType.DEEPBREATH: eventEndCaller.Invoke(EventEndType.DEEPBREATHEND); break;
            case ActionStateType.BREATHLESSNESS: eventEndCaller.Invoke(EventEndType.BREATHLESSNESSEND); break;
            case ActionStateType.DAMAGE: eventEndCaller.Invoke(EventEndType.DAMAGEEND); break;
            case ActionStateType.DOLLGET: eventEndCaller.Invoke(EventEndType.GETDOOLEND); break;
            case ActionStateType.TRAP: eventEndCaller.Invoke(EventEndType.TRAPEND); break;
        }
    }

    /// <summary>
    /// 各イベント開始処理
    /// </summary>
    void EventStart()
    {
        switch (State)
        {
            case ActionStateType.WALK: eventStartCaller.Invoke(EventStartType.WALKSTART); break;
            case ActionStateType.DASH: eventStartCaller.Invoke(EventStartType.DASHSTART); break;
            case ActionStateType.BREATHHOLD: eventStartCaller.Invoke(EventStartType.BREATHHOLDSTART); IsBreathHold = true; break;
            case ActionStateType.BREATHHOLDMOVE: eventStartCaller.Invoke(EventStartType.BREATHHOLDSMOVETART); IsBreathHold = true; break;
            case ActionStateType.DOOROPEN: eventStartCaller.Invoke(EventStartType.DOORSTART); break;
            case ActionStateType.HIDE: eventStartCaller.Invoke(EventStartType.HIDESTART); break;
            case ActionStateType.DEEPBREATH: eventStartCaller.Invoke(EventStartType.DEEPBREATHSTART); break;
            case ActionStateType.BREATHLESSNESS: eventStartCaller.Invoke(EventStartType.BREATHLESSNESSSTART); break;
            case ActionStateType.DAMAGE: eventStartCaller.Invoke(EventStartType.DAMAGESTART); break;
            case ActionStateType.DOLLGET: eventStartCaller.Invoke(EventStartType.GETDOLLSTART); break;
            case ActionStateType.TRAP: eventStartCaller.Invoke(EventStartType.TRAPSTART); break;
        }
    }

    /// <summary>
    /// ステートのリセット
    /// </summary>
    public void ResetState()
    {
        EventStop();
        doorController.enabled = false;
        hideController.enabled = false;
        damageController.enabled = false;
        dollGetController.enabled = false;
        State = ActionStateType.WAIT;
        EventStart();
    }

    /// <summary>
    /// 移動方向のレイに当たったオブジェクトのレイヤーを返す
    /// </summary>
    /// <returns>当たったオブジェクトのレイヤー</returns>
    int ObjectLayer()
    {
        // レイのスタート位置
        Vector3 start = player.position;

        // しゃがんでいる場合は少し高めに
        if (IsSquat)
        {
            start = player.position + Vector3.up * playerCollider.height;
        }

        // レイの向き
        Vector3 dir = player.forward;

        // レイの距離
        float distance = playerCollider.radius * 7.5f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = 1 << LayerMask.GetMask(new string[] { "Player", "Stage", "SafetyArea" });
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

    /// <summary>
    /// 隠れることが可能な状態かどうか
    /// </summary>
    public bool IsCanHide()
    {
        // 隠れることができる状態ならテキスト表示
        if (!hideController.enabled && hideController.IsInteractArea && (rayObjectLayer == LayerMask.NameToLayer("Locker") || rayObjectLayer == LayerMask.NameToLayer("Bed")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 部屋番号リセット
    /// </summary>
    public void ResetAreaName()
    {
        if (gameController.IsReturn)
        {
            NowAreaName = "Area11";
        }
        else
        {
            NowAreaName = "Area01";
        }
    }

    /// <summary>
    /// 部屋番号変更
    /// </summary>
    /// <param name="AreaName">部屋番号</param>
    public void ChangeAreaName(string AreaName)
    {
        NowAreaName = AreaName;
    }

    /// <summary>
    /// しゃがみ解除
    /// </summary>
    public void SquatEnd()
    {
        IsSquat = false;
    }
}