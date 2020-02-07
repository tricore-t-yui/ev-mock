using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowState = ShadowParameter.StateType;

public class ShadowStateFighting : ShadowStateBase
{
    bool isPlayerDetect = false;
    float waitCounter = 0;

    public override void Entry()
    {
        // プレイヤーを見つけた
        isPlayerDetect = true;
        // 移動速度を設定
        agent.speed = parameter.FightingMoveSpeed;
    }

    public override void Update()
    {
        // プレイヤーを見つけている
        if (isPlayerDetect)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                animator.SetBool("IsWaiting", true);
                isPlayerDetect = false;
                OnPlayerMissing();
            }
        }
        // プレイヤーを見失っている
        else
        {
            if (animator.GetBool("IsWaiting"))
            {
                waitCounter += Time.deltaTime;

                if (waitCounter > parameter.FightingWaitTime)
                {
                    animator.SetBool("IsWaiting", false);
                    waitCounter = 0;
                }
            }
        }
    }

    public override void Exit()
    {
        animator.SetBool("IsWaiting", false);
    }

    /// <summary>
    /// プレイヤーを見失った
    /// </summary>
    public void OnPlayerMissing()
    {
        // 警戒状態に変更
        SetNextState((int)ShadowState.Caution);
    }
}
