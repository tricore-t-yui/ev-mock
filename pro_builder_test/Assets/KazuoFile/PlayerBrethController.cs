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
        HOLD,
        PATIENCE,
    }

    [SerializeField]
    PlayerMoveController playerController = default; // プレイヤーの状態管理クラス

    [SerializeField]
    KeyCode ReductionKey = default;     // 息の消費軽減キー
    [SerializeField]
    KeyCode brethHoldKey = default;         // 息止めキー

    [SerializeField]
    float holdDecrement = 0;            // 息止め時の息消費量
    [SerializeField]
    float patienceDecrement = 0;        // 息我慢時(連打なし)の息消費量
    [SerializeField]
    float buttonPatienceDecrement = 0;  // 息我慢時(連打あり)の息消費量

    public bool IsBrethHold { get; private set; } = false;      // 息止めフラグ
    public bool IsBreathlessness { get; private set; } = false; // 息切れフラグ

    public float ResidualAmount { get; private set; } = 100;               //　息の残量

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 息残量の初期化
        ResidualAmount = 100;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //　息切れした時か、息を使っていない時は
        if (IsBreathlessness || (!IsBrethHold && !playerController.IsHide))
        {
            // 息の回復
            ResidualAmount++;
        }

        // 息消費処理
        BreathConsumption();
    }

    /// <summary>
    /// 息消費処理
    /// </summary>
    void BreathConsumption()
    {
        // 息止め検知処理
        BrathHold();

        // 隠れている間は息消費
        if (playerController.IsHide)
        {
            ConsumeBreath(ConsumeType.PATIENCE);
        }

        // 値補正
        ResidualAmount = clamp(ResidualAmount, 0, 100);

        // 息切れ検知処理
        Breathlessness();
    }

    /// <summary>
    /// 息止め検知処理
    /// </summary>
    void BrathHold()
    {
        // 息止めキーが押されたら
        if (!playerController.IsDash && Input.GetKey(brethHoldKey))
        {
            // 息止め開始
            IsBrethHold = true;
            ConsumeBreath(ConsumeType.HOLD);
        }
        else
        {
            // 息止め終了
            IsBrethHold = false;
        }
    }

    /// <summary>
    /// 息切れ検知処理
    /// </summary>
    void Breathlessness()
    {
        if (!IsBreathlessness && ResidualAmount <= 0)
        {
            IsBreathlessness = true;
        }
        else if (ResidualAmount >= 100)
        {
            IsBreathlessness = false;
        }
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
                // ボタン連打
                if (Input.GetKeyDown(ReductionKey))
                {
                    ResidualAmount -= buttonPatienceDecrement;
                }
                // 何も押さない
                else
                {
                    ResidualAmount -= patienceDecrement;
                }
                break;
        }
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
