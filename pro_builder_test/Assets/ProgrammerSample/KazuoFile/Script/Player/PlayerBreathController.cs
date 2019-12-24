using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoveType = PlayerStateController.ActionStateType;
using ActionSoundType = SoundAreaSpawner.ActionSoundType;
using HeartSoundType = HideStateController.HeartSoundType;
using KeyType = KeyController.KeyType;

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
    PlayerHideController hideController = default;              // 隠れるクラス
    [SerializeField]
    SoundAreaSpawner soundArea = default;                       // 音管理クラス
    [SerializeField]
    KeyController keyController = default;                      // キー操作クラス

    [SerializeField]
    float normalRecovery = 0.3f;                               // 通常の息の回復量
    [SerializeField]
    float deepBreathRecovery = 0.5f;                            // 深呼吸時の息の回復量
    [SerializeField]
    float breathlessnessRecovery = 0.3f;                       // 息切れ時の息の回復量

    [SerializeField]
    float stealthDecrement = 0.175f;                             // 息止め時の息消費量
    [SerializeField]
    float hideSmallDecrement = 0.3f;                           // 隠れる＋息止め時の息消費量(小)
    [SerializeField]
    float hideMediumDecrement = 0.3f;                           // 隠れる＋息止め時の息消費量(中)
    [SerializeField]
    float hideLargeDecrement = 0.3f;                            // 隠れる＋息止め時の息消費量(大)
    [SerializeField]
    float buttonPatienceDecrement = 0.01f;                      // 息我慢時(連打あり)の息消費量

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
    public bool IsDisappear { get; private set; } = false;      // 息切れフラグ
    public float NowAmount { get; private set; } = 100;         // 息の残量
    public BrethState State { get; private set; } = BrethState.NOTCONFUSION;      // 息の状態

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsDisappear = false;
        NowAmount = 100;
    }

    /// <summary>
    /// 息の残量による音の発生
    /// </summary>
    void BreathSound(MoveType type)
    {
        if (IsDisappear)
        {
            // 息切れ
            soundArea.AddSoundLevel(ActionSoundType.BREATHLESSNESS);
            State = BrethState.BREATHLESSNESS;
        }
        else if((type != MoveType.HIDE && type != MoveType.BREATHHOLD && type != MoveType.BREATHHOLDMOVE)
             || (type == MoveType.HIDE && !hideController.IsHideStealth()))
        {
            if (NowAmount <= smallDisturbance)
            {
                // 小さな乱れ
                soundArea.AddSoundLevel(ActionSoundType.SMALLCONFUSION);
                State = BrethState.SMALLCONFUSION;

                if (NowAmount <= mediumDisturbance)
                {
                    // 乱れ
                    soundArea.AddSoundLevel(ActionSoundType.MEDIUMCONFUSION);
                    State = BrethState.MEDIUMCONFUSION;

                    if (NowAmount <= largeDisturbance)
                    {
                        // 大きな乱れ
                        soundArea.AddSoundLevel(ActionSoundType.LARGECONFUSION);
                        State = BrethState.LARGECONFUSION;
                    }
                }
            }
            else
            {
                // 乱れ無し
                State = BrethState.NOTCONFUSION;
            }
        }
        else
        {
            // 乱れ無し
            State = BrethState.NOTCONFUSION;
        }
    }

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType type)
    {
        // 各ステートに合わせた処理を実行
        switch (type)
        {
            case MoveType.WAIT: NowAmount += normalRecovery; break;
            case MoveType.WALK: NowAmount += normalRecovery; break;
            case MoveType.BREATHHOLD: NowAmount -= stealthDecrement; break;
            case MoveType.BREATHHOLDMOVE: NowAmount -= stealthDecrement; break;
            case MoveType.HIDE: ConsumeHideBreath(); break;
            case MoveType.BREATHLESSNESS: NowAmount += breathlessnessRecovery; break;
            default: break;
        }

        // 息切れ検知
        if (NowAmount <= 0)
        {
            IsDisappear = true;
        }
        if(IsDisappear && NowAmount >= 100)
        {
            IsDisappear = false;
        }

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);

        // 息の残量による音の発生
        BreathSound(type);
    }

    /// <summary>
    /// 隠れた時の息消費
    /// </summary>
    public void ConsumeHideBreath()
    {
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
        if (keyController.GetKey(KeyType.ENDUREBREATH))
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

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void DeepBreathRecovery()
    {
        NowAmount += deepBreathRecovery;
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetAmount()
    {
        IsDisappear = false;
        NowAmount = 100;
    }
}
