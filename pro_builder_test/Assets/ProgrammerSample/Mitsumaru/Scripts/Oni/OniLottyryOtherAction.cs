using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OniLottyryOtherAction : StateMachineBehaviour
{
    // 行動の抽選確率：移動続行
    [SerializeField,Range(0,100)]
    float moveContinue = 0;

    // 行動の抽選確率：待機
    [SerializeField, Range(0, 100)]
    float standing = 0;

    // 行動の抽選確率：あたりを見回す
    [SerializeField, Range(0, 100)]
    float turnAround = 0;

    /// <summary>
    /// ステートの開始
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // それぞれの確立をリストにセット
        List<float> rates = new List<float>();
        rates.Add(moveContinue);
        rates.Add(standing);
        rates.Add(turnAround);

        // 複数のレアリティの抽選率の合計を算出
        float total = rates.Sum();

        // 乱数を生成（０～抽選率の合計）
        float random = Random.Range(0, total);

        int kindId = 0;
        // 乱数をそれぞれのレアリティの抽選率で引いていく
        for (int i = 0; i < rates.Count; i++)
        {
            random -= rates[i];

            // 残りの乱数が０以下になったら、その時点のレアリティのIDを返す
            if (random < 0)
            {
                kindId = i;
                break;
            }
        }

        // 決まったidをパラメータにセット
        animator.SetInteger("searchingOtherActionKindId", kindId);
    }

    /// <summary>
    /// ステートの終了
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        // 徘徊開始のトリガーをリセット
        animator.ResetTrigger("lotteryStart");
    }
}
