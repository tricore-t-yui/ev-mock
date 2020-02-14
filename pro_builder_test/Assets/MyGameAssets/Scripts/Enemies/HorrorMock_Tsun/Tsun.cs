using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class Tsun : StateMachine
{
    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        soundSpawner = GameObject.FindObjectOfType<SoundAreaSpawner>();

        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new TsunStateNormal(soundSpawner),
            new TsunStateCaution(),
            new TsunStateFighting(),
        };

        // ステートマシンの初期化
        Initialize(states);
    }
}
