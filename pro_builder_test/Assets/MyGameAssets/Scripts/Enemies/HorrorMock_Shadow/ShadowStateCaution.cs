using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowState = ShadowParameter.StateType;

public class ShadowStateCaution : ShadowStateBase
{
    bool isReturnPosition = false;
    float waitCounter = 0;

    public override void Entry()
    {
        // 移動速度を設定
        agent.speed = parameter.CautionMoveSpeed;

        // 初期位置に戻るフラグをオフにする
        isReturnPosition = false;

    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Update()
    {
        if ((parameter.InitialPosition - agent.destination).magnitude > agent.stoppingDistance)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                animator.SetBool("IsWaiting", true);
            }

            if (animator.GetBool("IsWaiting"))
            {
                waitCounter += Time.deltaTime;
                if (waitCounter > parameter.CautionWaitTime)
                {
                    animator.SetBool("IsWaiting", false);
                    agent.SetDestination(parameter.InitialPosition);
                    waitCounter = 0;
                }
            }
        }
        else
        {
            if (agent.remainingDistance < agent.stoppingDistance &&
               (parameter.InitialPosition - agent.destination).magnitude < agent.stoppingDistance)
            {
                SetNextState((int)ShadowState.Normal);
            }
        }
    }

    /// <summary>
    /// 終了
    /// </summary>
    public override void Exit()
    {
        animator.SetBool("IsWaiting", false);
    }
}
