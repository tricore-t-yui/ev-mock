using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowState = ShadowParameter.StateType;

public class ShadowStateFighting : ShadowStateBase
{
    float waitCounter = 0;

    public override void Entry()
    {
        // 移動速度を設定
        agent.speed = parameter.FightingMoveSpeed;
    }

    public override void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            OnMissingPlayer();
        }

        if (animator.GetBool("IsWaiting"))
        {
            waitCounter += Time.deltaTime;
            if (waitCounter > parameter.FightingWaitTime)
            {
                animator.SetBool("IsWaiting", false);
                animator.ResetTrigger("Attaking");
                waitCounter = 0;
            }
        }
    }

    public override void Exit()
    {
        
    }

    public void OnMissingPlayer()
    {
        SetNextState((int)ShadowState.Caution);
    }
}
