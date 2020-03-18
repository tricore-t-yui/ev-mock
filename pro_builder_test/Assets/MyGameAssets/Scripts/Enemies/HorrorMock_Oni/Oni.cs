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

    /// <summary>
    /// ビューレンジ入った
    /// </summary>
    public override void OnEnterViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnDetectedPlayer(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ更新
    /// </summary>
    public override void OnStayViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnDetectPlayerStay(other.gameObject);
    }

    /// <summary>
    /// ビューレンジ抜けた
    /// </summary>
    /// <param name="other"></param>
    public override void OnExitViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        states[(int)currentState].OnMissingPlayer(other.gameObject);
    }

}
