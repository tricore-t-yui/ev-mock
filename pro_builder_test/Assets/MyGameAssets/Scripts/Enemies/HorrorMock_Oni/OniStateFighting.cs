using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniStateFighting : ShadowStateFighting
{
    PlayerStateController playerState;
    Transform playerTrans;
    public OniStateFighting(PlayerStateController _playerState, Transform _playerTrans) { playerState = _playerState; playerTrans = _playerTrans; }

    public override void Update()
    {
        UpdateEnemyDest();

        base.Update();
    }

    float prevEnemyDesideTime;
    void UpdateEnemyDest()
    {
        // 常に隠れていないプレイヤーを目標に
        if (!float.IsInfinity(agent.remainingDistance) && Time.timeSinceLevelLoad - prevEnemyDesideTime > 1.0f && playerState.State != PlayerStateController.ActionStateType.HIDE && !agent.pathPending)
        {
            agent.SetDestination(playerTrans.position);
            prevEnemyDesideTime = Time.timeSinceLevelLoad;
            currentTargetPos = playerTrans.position;
        }
        else if(agent.desiredVelocity == Vector3.zero)
        {
            agent.SetDestination(playerTrans.position);
            currentTargetPos = playerTrans.position;
        }
        //Debug.Log("agent.remainingDistance:" + agent.remainingDistance);

        // 残り距離がインフィニってたら加速
        if (agent.remainingDistance > 8.0f)
        {
            agent.speed += 0.03f;
        }
        else
        {
            agent.speed = parameter.FightingMoveSpeed;
        }
    }
}
