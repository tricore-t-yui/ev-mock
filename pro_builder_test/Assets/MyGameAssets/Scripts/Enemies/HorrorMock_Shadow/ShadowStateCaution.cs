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
    }

    public override void Update()
    {
        if (isReturnPosition)
        {
            if (agent.remainingDistance < (parameter.InitialPosition - agent.transform.position).magnitude)
            {
                isReturnPosition = false;
                // 通常状態に変更
                SetNextState((int)ShadowState.Normal);
            }
        }
        else
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                animator.SetBool("IsWaiting", true);
                waitCounter += Time.deltaTime;

                if (waitCounter > parameter.CautionWaitTime)
                {
                    isReturnPosition = true;
                    // 初期位置に戻る
                    agent.SetDestination(parameter.InitialPosition);
                    animator.SetBool("IsWaiting", false);
                }
            }
            else
            {
                animator.SetBool("IsWaiting", false);
            }
           
        }
    }

    public override void Exit()
    {
        isReturnPosition = false;
    }
}
