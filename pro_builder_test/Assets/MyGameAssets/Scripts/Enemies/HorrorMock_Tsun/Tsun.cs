﻿using UnityEngine;

/// <summary>
/// ツン。ほとんど見た目が変わった影人間
/// </summary>
public class Tsun : EnemyBase
{
    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new TsunStateNormal(),
            new TsunStateCaution(),
            new TsunStateFighting(),
        };

        // ステートマシンの初期化
        Initialize(states);
    }

    public void SetTargetPos(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
