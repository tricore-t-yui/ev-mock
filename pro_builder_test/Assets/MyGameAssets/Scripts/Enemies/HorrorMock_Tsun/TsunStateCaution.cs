using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class TsunStateCaution : StateBase
{
    float waitCounter = 0;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;

        // 移動速度を設定
        agent.speed = parameter.CautionMoveSpeed;

        animator.SetBool("IsDetectedNoise", true);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        // 発信源についたよ
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // その場でしばらく待機
            animator.SetBool("IsWaiting", true);
            animator.SetBool("IsDetectedNoise", false);
            waitCounter += Time.deltaTime;
            // しばらく待機したら
            if (waitCounter > parameter.CautionWaitTime)
            {
                // 待機フラグ解除
                animator.SetBool("IsWaiting", false);
                // 初期化
                waitCounter = 0;

                //初期位置まで距離が一定以上離れていたら
                if ((agent.transform.position - parameter.InitialPosition).magnitude > parameter.ReturnWarpDistance)
                {
                    // 初期位置まで瞬間移動する
                    agent.Warp(parameter.InitialPosition);
                }
                // 通常状態に戻る
                SetNextState((int)StateType.Normal);
            }
        }
        // 待機中に新たに音を聞いた
        else
        {
            // 待機を解除する
            animator.SetBool("IsWaiting", false);
            waitCounter = 0;
        }
    }

    /// <summary>
    /// 終了
    /// </summary>
    public override void Exit()
    {
        // 待機を解除する
        animator.SetBool("IsWaiting", false);
        // 音が聞こえなくなった
        animator.SetBool("IsDetectedNoise", false);
    }
}
