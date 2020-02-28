using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowStateCaution : StateBase
{
    float waitCounter = 0;
    bool isTouchaku = false;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        isTouchaku = false;

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
            if (!isTouchaku)
            {
                // その場でしばらく待機
                animator.SetBool("IsWaiting", true);
                animator.SetBool("IsDetectedNoise", false);
                isTouchaku = true;
            }
            waitCounter += Time.deltaTime;
            // しばらく待機したら
            if (waitCounter > parameter.CautionWaitTime)
            {
                // 待機フラグ解除
                animator.SetBool("IsWaiting", false);

                 //初期位置まで距離が一定以上離れていたら
                 if ((agent.transform.position - parameter.InitialPosition).magnitude > parameter.ReturnWarpDistance)
                 {
                     // 初期位置まで瞬間移動する
                     agent.Warp(parameter.InitialPosition);
                 }
                 else
                 {
                    agent.SetDestination(parameter.InitialPosition);
                 }
                 if ((agent.transform.position - parameter.InitialPosition).magnitude < agent.stoppingDistance)
                 {
                    // 通常状態に戻る
                    SetNextState((int)StateType.Normal);
                    // 初期化
                    waitCounter = 0;
                }
            }
        }
        // 待機中に新たに音を聞いた
        else if (agent.remainingDistance > agent.stoppingDistance &&
                (agent.destination - parameter.InitialPosition).magnitude > agent.stoppingDistance)
        {
            // 待機を解除する
            animator.SetBool("IsWaiting", false);
            waitCounter = 0;
            isTouchaku = false;
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
        isTouchaku = false;
    }
}