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
    /// 息の消費タイプ
    /// </summary>
    public enum ConsumeType
    {
        HOLD,       // 息止め 
        PATIENCE,   // 隠れる
    }

    [SerializeField]
    PlayerMoveController playerController = default; // プレイヤーの状態管理クラス

    [SerializeField]
    KeyCode ReductionKey = KeyCode.Q;       // 息の消費軽減キー

    [SerializeField]
    float RecoveryAmount = 0.5f;            // 息の回復量
    [SerializeField]
    float holdDecrement = 0.15f;            // 息止め時の息消費量
    [SerializeField]
    float patienceDecrement = 0.25f;        // 息我慢時(連打なし)の息消費量
    [SerializeField]
    float buttonPatienceDecrement = 0.1f;   // 息我慢時(連打あり)の息消費量
    [SerializeField]
    int durationPlus = 5;                   // 1回のボタンで追加される連打処理の継続フレームの値 (詳細は165行のNOTE)

    int duration = 0;                       // 連打処理の継続フレーム (詳細は165行のNOTE)

    public bool IsBreathlessness { get; private set; } = false; // 息切れフラグ
    public float ResidualAmount { get; private set; } = 100;    //　息の残量

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 初期化
        ResidualAmount = 100;
        duration = 0;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //　息切れした時か、息を使っていない時は
        if (IsBreathlessness || (!playerController.IsStealth && !(playerController.IsHide && playerController.IsWarning)))
        {
            // 息の回復
            ResidualAmount += RecoveryAmount;
        }

        // 息消費処理
        BreathConsumption();
    }

    /// <summary>
    /// 息消費処理
    /// </summary>
    void BreathConsumption()
    {
        // 息切れ検知
        Breathlessness();

        // 息切れしていない状態で、息止めキーが押されたら
        if (!IsBreathlessness && playerController.IsStealth)
        {
            // 息止め処理
            BrathHold();
        }

        // 隠れている時
        if (playerController.IsHide && playerController.IsWarning)
        {
            // 隠れる処理
            Hide();
        }

        // 値補正
        ResidualAmount = clamp(ResidualAmount, 0, 100);
    }

    /// <summary>
    /// 息切れ検知
    /// </summary>
    void Breathlessness()
    {
        // まだ息切れ状態になっていないが、息の残量が0になったら
        if (!IsBreathlessness && ResidualAmount <= 0)
        {
            // 息切れ開始
            IsBreathlessness = true;
        }
        // 息切れ状態時に息が100まで回復したら
        else if (ResidualAmount >= 100)
        {
            // 息切れ解除
            IsBreathlessness = false;
        }
    }

    /// <summary>
    /// 息止め処理
    /// </summary>
    void BrathHold()
    {
        // 息消費(息止め)
        ConsumeBreath(ConsumeType.HOLD);
    }

    /// <summary>
    /// 息消費
    /// </summary>
    /// <param name="type">息の消費タイプ</param>
    public void ConsumeBreath(ConsumeType type)
    {
        switch(type)
        {
            // 息止め時
            case ConsumeType.HOLD: ResidualAmount -= holdDecrement; break;

            // 息我慢時
            case ConsumeType.PATIENCE:

                // 連打処理の継続時間続いている間
                if (duration > 0)
                {
                    ResidualAmount -= buttonPatienceDecrement;
                }
                // 続いていなかったら
                else
                {
                    ResidualAmount -= patienceDecrement;
                }
                break;
        }
    }

    /// <summary>
    /// 隠れる処理
    /// </summary>
    void Hide()
    {
        // 連打処理
        StrikeButtonRepeatedly();

        // 息消費(隠れる)
        ConsumeBreath(ConsumeType.PATIENCE);
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
        duration = (int)clamp(duration, 0, 100);
    }

    /// <summary>
    /// 上限、下限補正
    /// </summary>
    /// <param name="value">補正する値</param>
    /// <param name="min">下限値</param>
    /// <param name="max">上限値</param>
    /// <returns>補正した値</returns>
    float clamp(float value, float min, float max)
    {
        return Mathf.Min(Mathf.Max(value, min),max);
    }
}
