using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class OniStateNormal : ShadowStateNormal
{
    PlayerStateController playerState;
    Transform playerTrans;
    public OniStateNormal(PlayerStateController _playerState, Transform _playerTrans) { playerState = _playerState; playerTrans = _playerTrans; }

    public override void Update()
    {
        // 常に隠れていないプレイヤーを目標に
        if (playerState.State != PlayerStateController.ActionStateType.HIDE)
        {
            agent.SetDestination(playerTrans.position);
            // 警戒に移行
            SetNextState((int)StateType.Caution);
        }

        base.Update();
    }

    public override void OnHearNoise(GameObject noise)
    {
        // ノイズ聞き取りで警戒をしない
    }
}
