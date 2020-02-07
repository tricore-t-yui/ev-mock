using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStateNormal : ShadowStateBase
{
    public override void Entry()
    {
        // 移動速度を設定
        agent.speed = parameter.NormalMoveSpeed;
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
