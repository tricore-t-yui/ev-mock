using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの息管理クラス
/// </summary>
public class PlayerBrethController : MonoBehaviour
{
    /// <summary>
    /// 息の状態
    /// </summary>
    public enum BrethState
    {
        WAIT,                   // 何もしていない状態
        WALK,                   // 歩いている状態
        DASH,                   // ダッシュしている状態
        STEALTH,                // 忍び歩きしている状態
        HIDE,                   // 隠れている状態
        DEEPBREATH,             // 深呼吸している状態
        BREATHLESSNESS,         // 息切れしている状態
    }

    [SerializeField]
    KeyCode ReductionKey = KeyCode.Q;               // 息の消費軽減キー
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるクラス

    [SerializeField]
    float normalRecoveryAmount = 0.5f;              // 通常の息の回復量
    [SerializeField]
    float breathlessnessRecoveryAmount = 0.2f;      // 息切れ時の息の回復量
    [SerializeField]
    float DashDecrement = 0.2f;                    // 息止め時の息消費量
    [SerializeField]
    float stealthDecrement = 0.15f;                    // 息止め時の息消費量
    [SerializeField]
    float patienceDecrement = 0.25f;                // 息我慢時(連打なし)の息消費量
    [SerializeField]
    float buttonPatienceDecrement = 0.1f;           // 息我慢時(連打あり)の息消費量
    [SerializeField]
    int durationPlus = 5;                           // 1回のボタンで追加される連打処理の継続フレームの値 (詳細は165行のNOTE)

    int duration = 0;                               // 連打処理の継続フレーム (詳細は165行のNOTE)

    public float NowAmount { get; private set; } = 100;             // 息の残量

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(BrethState state)
    {
        // 各ステートに合わせた処理を実行
        switch (state)
        {
            case BrethState.WAIT: NowAmount += normalRecoveryAmount; break;
            case BrethState.WALK: NowAmount += normalRecoveryAmount; break;
            case BrethState.DASH: ConsumeBreath(state); break;
            case BrethState.STEALTH: ConsumeBreath(state); break;
            case BrethState.HIDE: ConsumeBreath(state); break;
            case BrethState.DEEPBREATH: NowAmount += normalRecoveryAmount * 2; break;
            case BrethState.BREATHLESSNESS: NowAmount += breathlessnessRecoveryAmount; break;
            default: break;
        }
    }

    /// <summary>
    /// 息消費
    /// </summary>
    public void ConsumeBreath(BrethState state)
    {
        switch (state)
        {
            case BrethState.DASH: NowAmount -= DashDecrement; break;
            case BrethState.STEALTH:NowAmount -= stealthDecrement;break;
            case BrethState.HIDE:
                // 警戒状態じゃなかったら
                if (hideController.IsWarning && hideController.IsHide)
                {
                    // 連打処理
                    StrikeButtonRepeatedly();

                    // 連打処理の継続時間続いている間
                    if (duration > 0)
                    {
                        NowAmount -= buttonPatienceDecrement;
                    }
                    // 続いていなかったら
                    else
                    {
                        NowAmount -= patienceDecrement;
                    }
                }
                break;
            default: break;
        }

        // 値補正
        NowAmount = Mathf.Clamp(NowAmount, 0, 100);
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
}
