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
        base.Update();
    }
}
