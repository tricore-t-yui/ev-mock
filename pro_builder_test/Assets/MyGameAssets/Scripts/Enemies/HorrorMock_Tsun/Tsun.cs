using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = EnemyParameter.StateType;

public class Tsun : MonoBehaviour
{
    // ツンのステートマシン
    [SerializeField]
    StateMachine tsunStateMachine = default;

    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        // 各クラスのインスタンスを取得
        tsunStateMachine.player = GameObject.FindGameObjectWithTag("Player");
        tsunStateMachine.soundSpawner = GameObject.FindObjectOfType<SoundAreaSpawner>();
        tsunStateMachine.damageEvent = GameObject.FindObjectOfType<PlayerDamageEvent>();

        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new TsunStateNormal(tsunStateMachine.soundSpawner),
            new TsunStateCaution(),
            new TsunStateFighting(),

        };

        // ステートマシンの初期化
        tsunStateMachine.Initialize(states);
    }

    /// <summary>
    /// 開始
    /// </summary>
    void OnEnable()
    {
        // パラメーターの初期化
        tsunStateMachine.Entry();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // ステートマシンの更新
        tsunStateMachine.Update();
    }

    /// <summary>
    /// 現在のステートを取得
    /// </summary>
    public StateType GetCurrentState()
    {
        return tsunStateMachine.currentState;
    }

    /// <summary>
    /// 移動目標位置のセット
    /// </summary>
    public void SetTargetPos(Vector3 pos)
    {
        // 警戒状態に変更
        tsunStateMachine.SetNextState(StateType.Caution);
        // 移動目標位置をセット
        tsunStateMachine.agent.SetDestination(pos);
    }

    /// <summary>
    /// 音を聞いた！
    /// </summary>
    /// <param name="other"></param>
    public void OnHeardNoise(Collider other)
    {
        if (!tsunStateMachine.parameter.IsDetectNoiseToTransparent)
        {
            // 警戒状態のみ
            if (tsunStateMachine.currentState != StateType.Caution) { return; }
        }
        // ノイズのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Noise")) { return; }

        // 移動目標位置を発信源に
        tsunStateMachine.agent.SetDestination(other.transform.position);
        tsunStateMachine.animator.SetBool("IsAppear", true);
    }

    /// <summary>
    /// プレイヤーを発見した！
    /// </summary>
    /// <param name="other"></param>
    public void OnDetectedPlayer(Collider other)
    {
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // 戦闘状態に
        tsunStateMachine.SetNextState(StateType.Fighting);

        // プレイヤーを移動目標位置に
        tsunStateMachine.agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    public void ChasePlayer(Collider other)
    {
        // 戦闘状態のみ
        if (tsunStateMachine.currentState != StateType.Fighting) { return; }
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // プレイヤーを移動目標位置に
        tsunStateMachine.agent.SetDestination(other.transform.position);
    }

    /// <summary>
    /// プレイヤーに攻撃！
    /// </summary>
    /// <param name="other"></param>
    public void AttackPlayer(Collider other)
    {
        // 戦闘状態のみ
        if (tsunStateMachine.currentState != StateType.Fighting) { return; }

        // 攻撃の種類をセット
        tsunStateMachine.animator.SetInteger("AttackConditionType", tsunStateMachine.attackTypeId);
        // 攻撃トリガーをセット
        tsunStateMachine.animator.SetTrigger("Attaking");
        // 待機フラグを立てる
        tsunStateMachine.animator.SetBool("IsWaiting", true);
        // ダメージイベント
        tsunStateMachine.damageEvent.Invoke(transform, tsunStateMachine.parameter.Damage);
    }
}
