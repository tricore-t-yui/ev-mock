using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoveType = PlayerStateController.ActionStateType;
using ActionSoundType = SoundAreaSpawner.ActionSoundType;
using HeartSoundType = HideStateController.HeartSoundType;

/// <summary>
/// プレイヤーの息管理クラス
/// </summary>
public class PlayerBreathController : MonoBehaviour
{
    /// <summary>
    /// 息の状態
    /// </summary>
    public enum BrethState
    {
        NOTCONFUSION,       // 乱れなし
        SMALLCONFUSION,     // 小さな乱れ
        MEDIUMCONFUSION,    // 乱れ
        LARGECONFUSION,     // 大きな乱れ
        BREATHLESSNESS,     // 息切れ
    }

    [SerializeField]
    KeyCode ReductionKey = KeyCode.Q;                           // 息の消費軽減キー
    [SerializeField]
    PlayerHideController hideController = default;              // 隠れるクラス
    [SerializeField]
    SoundAreaSpawner soundArea = default;                       // 音管理クラス
    [SerializeField]
    HideStateController hideState = default;                    // 隠れる状態管理クラス

    [SerializeField]
    float normalRecovery = 0.5f;                                // 通常の息の回復量
    [SerializeField]
    float breathlessnessRecovery = 0.2f;                        // 息切れ時の息の回復量

    [SerializeField]
    float DashDecrement = 0.2f;                                 // ダッシュ時の息消費量
    [SerializeField]
    float stealthDecrement = 0.15f;                             // 息止め時の息消費量
    [SerializeField]
    float hideSmallDecrement = 0.15f;                           // 隠れる＋息止め時の息消費量(小)
    [SerializeField]
    float hideMediumDecrement = 0.25f;                          // 隠れる＋息止め時の息消費量(中)
    [SerializeField]
    float hideLargeDecrement = 0.35f;                           // 隠れる＋息止め時の息消費量(大)
    [SerializeField]
    float buttonPatienceDecrement = 0.1f;                       // 息我慢時(連打あり)の息消費量

    [SerializeField]
    float smallDisturbance = 75;                                // 息の乱れ(小)の基準値
    [SerializeField]
    float mediumDisturbance = 50;                               // 息の乱れ(中)の基準値
    [SerializeField]
    float largeDisturbance = 20;                                // 息の乱れ(大)の基準値

    [SerializeField]
    int durationPlus = 5;                                       // 1回のボタンで追加される連打処理の継続フレームの値 (詳細は165行のNOTE)
    int duration = 0;                                           // 連打処理の継続フレーム (詳細は165行のNOTE)

    float hideDecrement = 0;                                    // 隠れているときの息の消費量
    public bool IsBreathlessness { get; private set; } = false; // 息切れフラグ
    public float NowAmount { get; private set; } = 100;         // 息の残量
    public BrethState state { get; private set; } = BrethState.NOTCONFUSION;      // 息の状態

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsBreathlessness = false;
        NowAmount = 100;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 息切れ検知
        if (!IsBreathlessness && NowAmount <= 0)
        {
            IsBreathlessness = true;
        }

        // 息の残量による音の発生
        BreathSound();
    }

    /// <summary>
    /// 息の残量による音の発生
    /// </summary>
    void BreathSound()
    {
        if (IsBreathlessness)
        {
            // 息切れ
            soundArea.AddSoundLevel(ActionSoundType.BREATHLESSNESS);
            state = BrethState.BREATHLESSNESS;
        }
        else
        {
            if (NowAmount <= smallDisturbance)
            {
                // 小さな乱れ
                soundArea.AddSoundLevel(ActionSoundType.SMALLCONFUSION);
                state = BrethState.SMALLCONFUSION;

                if (NowAmount <= mediumDisturbance)
                {
                    // 乱れ
                    soundArea.AddSoundLevel(ActionSoundType.MEDIUMCONFUSION);
                    state = BrethState.MEDIUMCONFUSION;

                    if (NowAmount <= largeDisturbance)
                    {
                        // 大きな乱れ
                        soundArea.AddSoundLevel(ActionSoundType.LARGECONFUSION);
                        state = BrethState.LARGECONFUSION;
                    }
                }
            }
            else
            {
                // 乱れ無し
                state = BrethState.NOTCONFUSION;
            }
        }
    }

    /// <summary>
    /// 息切れ解除
    /// </summary>
    /// NOTE:k.oishi アニメーション用関数
    public void RecoveryBreathlessness()
    {
        IsBreathlessness = false;
    }

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType state)
    {
        // 各ステートに合わせた処理を実行
        switch (state)
        {
            case MoveType.WAIT: NowAmount += normalRecovery; break;
            case MoveType.WALK: NowAmount += normalRecovery; break;
            case MoveType.DASH: ConsumeBreath(state); break;
            case MoveType.STEALTH: ConsumeBreath(state); break;
            case MoveType.HIDE: ConsumeBreath(state); break;
            case MoveType.DEEPBREATH: NowAmount += normalRecovery * 2; break;
            case MoveType.BREATHLESSNESS: NowAmount += breathlessnessRecovery; break;
            default: break;
        }

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);
    }

    /// <summary>
    /// 息消費
    /// </summary>
    public void ConsumeBreath(MoveType state)
    {
        switch (state)
        {
            case MoveType.DASH: NowAmount -= DashDecrement; break;
            case MoveType.STEALTH:NowAmount -= stealthDecrement;break;
            case MoveType.HIDE:
                // 息を止めていなかったら
                if (hideController.IsHideStealth())
                {
                    // 連打処理
                    StrikeButtonRepeatedly();

                    // 連打処理の継続時間続いている間
                    if (duration > 0)
                    {
                        NowAmount -= buttonPatienceDecrement;
                    }
                    // 連打していない場合
                    else
                    {
                        // 心音に合わせた息消費
                        NowAmount -= hideDecrement;
                    }
                }
                else
                {
                    NowAmount += normalRecovery;
                }
                break;
            default: break;
        }
    }

    /// <summary>
    /// ボタン連打処理
    /// </summary>
    /// NOTE:k.oishi
    ///      GetKeyDownのみでやると、GetKeyDownに入らなさすぎて全然息消費が軽減されないので、
    ///      連打処理の継続時間(duration)を用意し、キーを押すと値(durationPlus)がプラスされ、
    ///      継続時間が残っている限り、息消費を軽減し続けるようにしました。
    void StrikeButtonRepeatedly()
    {
        // 消費軽減キーを押したら
        if (Input.GetKeyDown(ReductionKey))
        {
            // 連打処理の継続時間にプラス
            duration += durationPlus;
        }

        // 継続時間をマイナス
        duration--;

        // 値が0以下ににならないように補正
        duration = Mathf.Clamp(duration, 0, 100);
    }

    /// <summary>
    /// 心音に合わせた息消費
    /// </summary>
    public void ChangeHideDecrement(HeartSoundType type)
    {
        switch (type)
        {
            case HeartSoundType.NORMAL: hideDecrement = hideSmallDecrement; break;
            case HeartSoundType.MEDIUM: hideDecrement = hideMediumDecrement; break;
            case HeartSoundType.LARGE: hideDecrement = hideLargeDecrement; break;
        }
    }
}
