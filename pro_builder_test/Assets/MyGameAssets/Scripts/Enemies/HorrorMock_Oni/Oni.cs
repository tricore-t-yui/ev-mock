using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oni : EnemyBase
{
    [SerializeField]
    Transform playerTrans;

    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new OniStateNormal(playerState, playerTrans),
            new OniStateCaution(playerState, playerTrans),
            new OniStateFighting(playerState, playerTrans),
        };

        // ステートマシンの初期化
        Initialize(states);
    }
}
