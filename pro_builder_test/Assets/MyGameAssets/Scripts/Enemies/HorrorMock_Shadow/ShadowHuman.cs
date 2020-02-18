using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowHuman : StateMachine
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
            new ShadowStateNormal(soundSpawner),
            new ShadowStateCaution(),
            new ShadowStateFighting(),
        };

        // ステートマシンの初期化
        Initialize(states);
    }

    void OnEnable()
    {
        if (parameter.InitialState == StateType.Normal)
        {
            Spawn(StateType.Normal);
        }
        else if (parameter.InitialState == StateType.Caution)
        {
            Spawn(StateType.Caution, player.transform.position);
        }
        else if (parameter.InitialState == StateType.Fighting)
        {
            Spawn(StateType.Fighting, player.transform.position);
        }
    }
}
