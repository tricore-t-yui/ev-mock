using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class TsunStateCaution : StateBase
{
    float waitCounter = 0;
    bool isReturn = false;

    /// <summary>
    /// 開始
    /// </summary>
    public override void Entry()
    {
        // 初期化
        waitCounter = 0;
        isReturn = false;

        // 移動速度を設定
        agent.speed = parameter.CautionMoveSpeed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        // 音の発信源まで移動する
        if (!isReturn)
        {
            // 発信源についたよ
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                // その場でしばらく待機
                animator.SetBool("IsWaiting", true);
                waitCounter += Time.deltaTime;
                // しばらく待機したら
                if (waitCounter > parameter.CautionWaitTime)
                {
                    // 初期位置を目標位置にする
                    agent.SetDestination(parameter.InitialPosition);
                    // 待機フラグ解除
                    animator.SetBool("IsWaiting", false);
                    // 初期化
                    waitCounter = 0;
                    // 初期位置まで戻るフラグをオンにする
                    isReturn = true;
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
        // 初期位置まで戻る
        else
        {
            // 戻っている最中に新たに音を聞いたら
            if ((parameter.InitialPosition - agent.destination).magnitude > agent.stoppingDistance)
            {
                // 戻るフラグを解除する
                isReturn = false;
            }

            // 初期位置を目標位置にする
            agent.SetDestination(parameter.InitialPosition);

            // 初期位置まで距離が一定以上離れていたら
            if ((agent.transform.position - parameter.InitialPosition).magnitude > parameter.ReturnWarpDistance)
            {
                // 初期位置まで瞬間移動する
                agent.Warp(parameter.InitialPosition);
                // 通常状態に戻る
                SetNextState((int)StateType.Normal);
            }

            // 初期位置に着いたら
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                // 通常状態に戻る
                SetNextState((int)StateType.Normal);
            }
        }
    }

    /// <summary>
    /// 終了
    /// </summary>
    public override void Exit()
    {

    }
}
