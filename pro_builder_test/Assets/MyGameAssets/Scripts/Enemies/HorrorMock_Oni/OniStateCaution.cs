using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniStateCaution : ShadowStateCaution
{
    PlayerStateController playerState;
    Transform playerTrans;
    public OniStateCaution(PlayerStateController _playerState, Transform _playerTrans) { playerState = _playerState; playerTrans = _playerTrans; }

    public override void Update()
    {
        UpdateEnemyDest();

        base.Update();
    }

    public override void OnHearNoise(GameObject noise)
    {
        // ノイズ聞き取りで警戒をしない
    }

    float prevEnemyDesideTime;
    void UpdateEnemyDest()
    {
        // 常に隠れていないプレイヤーを目標に
        if (!float.IsInfinity(agent.remainingDistance) && Time.timeSinceLevelLoad - prevEnemyDesideTime > 1.0f && playerState.State != PlayerStateController.ActionStateType.HIDE && !agent.pathPending)
        {
            agent.SetDestination(playerTrans.position);
            prevEnemyDesideTime = Time.timeSinceLevelLoad;
        }
        //Debug.Log("agent.remainingDistance:" + agent.remainingDistance);

        // 残り距離がインフィニってたら加速
        if (agent.remainingDistance > 8.0f)
        {
            agent.speed += 0.01f;
        }
        else
        {
            agent.speed = parameter.FightingMoveSpeed;
        }
    }
}
