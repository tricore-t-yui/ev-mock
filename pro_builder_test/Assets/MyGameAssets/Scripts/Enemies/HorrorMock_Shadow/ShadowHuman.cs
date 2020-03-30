using UnityEngine;
using StateType = EnemyParameter.StateType;

public class ShadowHuman : EnemyBase
{
    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new ShadowStateNormal(),
            new ShadowStateCaution(),
            new ShadowStateFighting(),
        };

        // ステートマシンの初期化
        Initialize(states);
    }

    void OnEnable()
    {
        // 状態によって生成処理を変える
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
