using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class TsunStateFighting : StateBase
{
   // 待機カウンター
    float waitCounter = 0;
    // プレイヤーを見つけているか
    bool isDetactedPlayer = true;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        isDetactedPlayer = true;

        // 移動速度を設定
        agent.speed = parameter.FightingMoveSpeed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        // プレイヤーを見つけている
        if (isDetactedPlayer)
        {
            // 目標位置まできた
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                // 見失った
                isDetactedPlayer = false;
                // 待機
                animator.SetBool("IsWaiting", true);
            }
            else
            {
                // 待機状態を解除
                animator.SetBool("IsWaiting", false);
                waitCounter = 0;
                isDetactedPlayer = true;
            }
        }

        // 待機中
        if (animator.GetBool("IsWaiting"))
        {
            waitCounter += Time.deltaTime;
            if (isDetactedPlayer)
            {
                // しばらく待機したら解除
                if (waitCounter > parameter.AttackedWaitTime)
                {
                    // 初期化
                    animator.SetBool("IsWaiting", false);
                    animator.ResetTrigger("Attaking");
                    waitCounter = 0;

                    // 攻撃したら消えるタイプ
                    if (parameter.IsAttackedDisappear)
                    {
                        // 消してリスポーン
                        animator.gameObject.SetActive(false);
                        animator.gameObject.SetActive(parameter.IsRespawn ? true : false);
                    }
                }
            }
            else
            {
                // しばらく待機したら解除
                if (waitCounter > parameter.FightingWaitTime)
                {
                    // 警戒状態に戻る
                    animator.SetBool("IsWaiting", false);
                    SetNextState((int)StateType.Caution);
                    // 初期化
                    waitCounter = 0;
                    isDetactedPlayer = false;
                }
            }
        }
    }
}
