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
        // 常に隠れていないプレイヤーを目標に
        if (playerState.State != PlayerStateController.ActionStateType.HIDE)
        {
            agent.SetDestination(playerTrans.position);
        }

        base.Update();
    }

    public override void OnHearNoise(GameObject noise)
    {
        // ノイズ聞き取りで警戒をしない
    }
}
